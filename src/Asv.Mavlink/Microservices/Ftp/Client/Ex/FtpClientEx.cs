using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using DynamicData;

namespace Asv.Mavlink;

public class FtpClientEx : DisposableOnceWithCancel, IFtpClientEx
{
    private uint _fileLength; // burst size is 23900
    private readonly TimeSpan _maxWaitTimeOfBurstPacket = TimeSpan.FromSeconds(5);

    public FtpClientEx(IFtpClient client)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    public IObservable<bool> OnBurstReading { get; set; }
    public IFtpClient Client { get; }

    public async Task ReadFile(string serverPath, string filePath, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        var openFileRo = await Client.OpenFileRO(serverPath, cs.Token).ConfigureAwait(false);

        if (openFileRo.OpCodeId == OpCode.NAK)
        {
            throw new Exception($"Server answered NAK with \"{(NakError)openFileRo.Data[0]}\"");
        }
        
        var fileLength = BitConverter.ToInt32(openFileRo.Data, 0);

        await using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        await using (BinaryWriter bw = new BinaryWriter(fs))
        {
            uint offset = 0;
            while (fileLength > offset)
            {
                var readFile = await Client.ReadFile(251 - 12, offset, openFileRo.Session, cs.Token).ConfigureAwait(false);
                
                if (readFile.OpCodeId == OpCode.NAK)
                {
                    break;
                }
                
                bw.Write(readFile.Data, 0, readFile.Size);
                offset += readFile.Size;
            }
        }
        await Client.TerminateSession(openFileRo.Session, cs.Token).ConfigureAwait(false);
    }
    
    public async Task BurstReadFile(string serverPath, string filePath, CancellationToken cancel, uint offset = 0)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        await using var fs = new FileStream(filePath, FileMode.OpenOrCreate);

        await using var bw = new BinaryWriter(fs);
        
        var openFileRo = await Client.OpenFileRO(serverPath, cs.Token).ConfigureAwait(false);
        
        _fileLength = BitConverter.ToUInt32(openFileRo.Data, 0);

        var tcs = new TaskCompletionSource();
        
        var lastBurstRead = DateTime.Now;
        
        using var obsTimer = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            if (DateTime.Now - lastBurstRead >= _maxWaitTimeOfBurstPacket)
            {
                tcs.TrySetCanceled();
                
                Client.TerminateSession(openFileRo.Session, cs.Token).Wait(cs.Token);
            }
        });
        
        using var burstSubscription = Client.OnBurstReadPacket.Where(_ => _.Session == openFileRo.Session).Subscribe(_ =>
        {
            lastBurstRead = DateTime.Now;
            
            bw.BaseStream.Position = _.Offset;
            bw.Write(_.Data, 0, _.Size);

            if (_.BurstComplete && _fileLength > _.Size + _.Offset)
            {
                Client.BurstReadFile(251 - 12, _.Size + _.Offset, openFileRo.Session, cs.Token).Wait(cs.Token);
            }

            if (_fileLength <= _.Size + _.Offset)
            {
                Client.TerminateSession(_.Session, cs.Token).Wait(cs.Token);
                
                tcs.TrySetResult();
            }
        });

        await Client.BurstReadFile(251 - 12, offset, openFileRo.Session, cs.Token).ConfigureAwait(false);

        await tcs.Task.ConfigureAwait(false);
    }

    public async Task UploadFile(string serverPath, string filePath, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        var createFile = await Client.CreateFile(serverPath, cs.Token).ConfigureAwait(false);

        if (createFile.OpCodeId == OpCode.NAK)
        {
            throw new Exception($"Server answered NAK with \"{(NakError)createFile.Data[0]}\"");
        }

        await using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader br = new BinaryReader(fs))
        {
            uint offset = 0;
            while (fs.Length > fs.Position)
            {
                var buffer = br.ReadBytes(251 - 12);
                var writeFile = await Client.WriteFile(buffer, offset, createFile.Session, cs.Token).ConfigureAwait(false);
                if (writeFile.OpCodeId == OpCode.NAK)
                {
                    break;
                }
                offset += (uint)buffer.Length;
            }
        }
        await Client.TerminateSession(createFile.Session, cs.Token).ConfigureAwait(false);
    }

    public async Task<List<FtpEntryItem>> ListDirectory(string path, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        
        var items = new List<FtpEntryItem>();
        
        var fileMatches = new List<Match>();
        var dirMatches = new List<Match>();
        
        while (true)
        {
            var listDirectory = await Client.ListDirectory(path, 
                (byte)(fileMatches.Count + dirMatches.Count), cs.Token).ConfigureAwait(false);

            if (listDirectory.OpCodeId == OpCode.NAK) break;

            var temp = Encoding.ASCII.GetString(listDirectory.Data);

            fileMatches.Add(Regex.Matches(temp, "F([a-z A-Z 0-9 . , _ -]+)\t(\\d+)\0\0"));
            dirMatches.Add(Regex.Matches(temp, "D([a-z A-Z 0-9 . , _ -]+)\0\0"));
            
            await Client.TerminateSession(listDirectory.Session, cs.Token).ConfigureAwait(false);
        }
        
        foreach (var dirMatch in dirMatches.Distinct(new CallbackComparer<Match>(_ => _.Value.GetHashCode(), 
                     (first, second) => first.Value.Equals(second.Value))))
        {
            var item = new FtpEntryItem()
            {
                Type = FtpEntryType.Directory,
                Name = dirMatch.Groups[1].Value
            };
            
            items.Add(item);
        }

        foreach (var fileMatch in fileMatches.Distinct(new CallbackComparer<Match>(_ => _.Value.GetHashCode(), 
                     (first, second) => first.Value.Equals(second.Value))))
        {
            var item = new FtpEntryItem()
            {
                Type = FtpEntryType.File,
                Name = fileMatch.Groups[1].Value
            };
            
            if (int.TryParse(fileMatch.Groups[2].Value, out var itemSize))
                item.Size = itemSize;
            
            items.Add(item);
        }
        
        return items;
    }

    public async Task CreateDirectory(string path, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        await Client.CreateDirectory(path, cs.Token).ConfigureAwait(false);
    }

    public async Task RemoveDirectory(string path, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        await Client.RemoveDirectory(path, cs.Token).ConfigureAwait(false);
    }
    
    public async Task RemoveFile(string path, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        await Client.RemoveFile(path, cs.Token).ConfigureAwait(false);
    }

    public async Task TruncateFile(string path, uint offset, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        await Client.TruncateFile(path, offset, cs.Token).ConfigureAwait(false);
    }
}