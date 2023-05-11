using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.IO;
using DynamicData;

namespace Asv.Mavlink;

public class FtpClientEx : DisposableOnceWithCancel, IFtpClientEx
{
    private uint _fileLength = 0; // burst size is 23900
    private string _lastBurstReadClientFileName;
    private string _lastBurstReadServerFileName;
    public IFtpClient Client { get; }
    
    public FtpClientEx(IFtpClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        
        Client = client;
        
        client.OnBurstReadPacket.Subscribe(_ => 
        {
            using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel);

            if (_lastBurstReadClientFileName != null)
            {
                using (FileStream fs = new FileStream(_lastBurstReadClientFileName, FileMode.OpenOrCreate))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.BaseStream.Position = _.Offset;
                    bw.Write(_.Data, 0, _.Size);
                }
            }
            
            if (_.BurstComplete && _fileLength > _.Size + _.Offset)
            {
                Task.WaitAll(BurstReadFile(_lastBurstReadServerFileName, _lastBurstReadClientFileName, cs.Token, _.Size + _.Offset));
            }

            if (_fileLength <= _.Size + _.Offset)
            {
                Task.WaitAll(Client.TerminateSession(_.Session, cs.Token));
            }
        }).DisposeItWith(Disposable);
    }

    public async Task ReadFile(string serverPath, string filePath, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        var openFileRo = await Client.OpenFileRO(serverPath, cs.Token).ConfigureAwait(false);

        if (openFileRo.OpCodeId == OpCode.NAK)
        {
            throw new Exception($"Server answered NAK with \"{(NakError)openFileRo.Data[0]}\"");
        }
        
        var fileLength = BitConverter.ToInt32(openFileRo.Data, 0);
        
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        using (BinaryWriter bw = new BinaryWriter(fs))
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
                offset += (uint)readFile.Size;
            }
        }
        await Client.TerminateSession(openFileRo.Session, cs.Token).ConfigureAwait(false);
    }
    
    public async Task BurstReadFile(string serverPath, string filePath, CancellationToken cancel, uint offset = 0)
    {
        _lastBurstReadServerFileName = serverPath;
        _lastBurstReadClientFileName = filePath;
        
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        var openFileRo = await Client.OpenFileRO(serverPath, cs.Token).ConfigureAwait(false);

        if (openFileRo.OpCodeId == OpCode.NAK)
        {
            if ((NakError)openFileRo.Data[0] == NakError.EOF) return;
            
            await Client.TerminateSession(openFileRo.Session, cs.Token).ConfigureAwait(false);
            
            await BurstReadFile(serverPath, filePath, cancel, offset).ConfigureAwait(false);
            
            return;
        }
        
        _fileLength = BitConverter.ToUInt32(openFileRo.Data, 0);
        
        await Client.BurstReadFile(251 - 12, offset, openFileRo.Session, cs.Token).ConfigureAwait(false);
    }

    public async Task UploadFile(string serverPath, string filePath, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);

        var createFile = await Client.CreateFile(serverPath, cs.Token).ConfigureAwait(false);

        if (createFile.OpCodeId == OpCode.NAK)
        {
            throw new Exception($"Server answered NAK with \"{(NakError)createFile.Data[0]}\"");
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
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

    public async Task<List<FtpFileListItem>> ListDirectory(string path, CancellationToken cancel)
    {
        using var cs = CancellationTokenSource.CreateLinkedTokenSource(DisposeCancel, cancel);
        
        var items = new List<FtpFileListItem>();
        
        byte offset = 0;
        
        StringBuilder sb = new StringBuilder();
        
        while (true)
        {
            var listDirectory = await Client.ListDirectory(path, offset, cs.Token).ConfigureAwait(false);
            
            if (listDirectory.OpCodeId == OpCode.NAK) break;
            
            sb.Append(Encoding.ASCII.GetString(listDirectory.Data));
            
            await Client.TerminateSession(listDirectory.Session, cs.Token).ConfigureAwait(false);
            
            offset += (byte)sb.ToString().Split("\0\0").Length;
        }
        
        var elements = sb.ToString().Split("\0\0").Where(_ => _ != "" & _ != "\0");

        foreach (var element in elements)
        {
            var item = new FtpFileListItem()
            {
                Type = (FileItemType)element.Split('\t')[0].First(),
                FileName = element.Split('\t')[0].Substring(1)
            };

            if(item.FileName == "." | item.FileName == "..") continue;
            
            if (element.Contains('\t') && int.TryParse(element.Split('\t')[1], out var itemSize))
            {
                item.Size = itemSize;
            }
            
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