using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData.Binding;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.AsvSdr;

public class AsvSdrExTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    #region Client-Server link setup
    private static readonly VirtualLink Link = new ();
        
    private static readonly MavlinkClientIdentity MavlinkClientIdentity = 
        new () { SystemId = 1, ComponentId = 1, TargetSystemId = 2, TargetComponentId = 2 };
    private static readonly HeartbeatClient HeartBeatClient = new (Link.Client, MavlinkClientIdentity, new PacketSequenceCalculator(),
        Scheduler.Default, new HeartbeatClientConfig());
    private static readonly CommandClient CommandClient = new (Link.Client, MavlinkClientIdentity, new PacketSequenceCalculator(),
        new CommandProtocolConfig(), Scheduler.Default);
    private static readonly AsvSdrClient AsvSdrClient = new (Link.Client, MavlinkClientIdentity, new PacketSequenceCalculator(),
        Scheduler.Default);
        
    private readonly AsvSdrClientEx _asvSdrClientEx = new (AsvSdrClient, HeartBeatClient, CommandClient, new AsvSdrClientExConfig());

    private static readonly MavlinkServerIdentity MavlinkServerIdentity = new () { SystemId = 2, ComponentId = 2 };
    private static readonly HeartbeatServer HeartBeatServer = new (Link.Server, new PacketSequenceCalculator(), MavlinkServerIdentity,
        new MavlinkHeartbeatServerConfig(), Scheduler.Default);
    private static readonly CommandServer CommandServer = new (Link.Server, new PacketSequenceCalculator(), MavlinkServerIdentity,
        Scheduler.Default);
    private static readonly CommandLongServerEx CommandLongServerEx = new (CommandServer);
    private static readonly AsvSdrServer AsvSdrServer = new (Link.Server, MavlinkServerIdentity, new AsvSdrServerConfig(),
        new PacketSequenceCalculator(), Scheduler.Default);

    private readonly AsvSdrServerEx _asvSdrServerEx = new (AsvSdrServer, HeartBeatServer, CommandLongServerEx);
    #endregion

    public AsvSdrExTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeLlz)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeGp)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeIdle)]
    [InlineData(AsvSdrCustomMode.AsvSdrCustomModeVor)]
    public async Task Check_For_AsvCustomMode_Set(AsvSdrCustomMode customMode)
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        _asvSdrServerEx.SetMode = (mode, hz, rate, ratio, cancel) =>
        {
            Assert.Equal(customMode, mode);
            Assert.Equal(11223U, hz);
            Assert.Equal(1, rate);
            Assert.Equal(1, ratio);
            return Task.FromResult(MavResult.MavResultAccepted);
        };

        var result = await _asvSdrClientEx.SetMode(customMode, 11223, 1, 1, CancellationToken.None);
        Assert.Equal(MavResult.MavResultAccepted, result);
    }

    [Theory]
    [InlineData(MavResult.MavResultUnsupported)]
    [InlineData(MavResult.MavResultAccepted)]
    [InlineData(MavResult.MavResultDenied)]
    [InlineData(MavResult.MavResultFailed)]
    [InlineData(MavResult.MavResultInProgress)]
    [InlineData(MavResult.MavResultTemporarilyRejected)]
    public async Task Check_For_MavResult_Value(MavResult mavResult)
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);
        
        _asvSdrServerEx.SetMode = (_, _, _, _, _) => Task.FromResult(mavResult);

        var result = await _asvSdrClientEx.SetMode(AsvSdrCustomMode.AsvSdrCustomModeLlz, 11223, 1, 1, CancellationToken.None);
        Assert.Equal(mavResult, result);
    }

    [Fact]
    public async Task Check_For_Successful_Record_Delete_Response()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,4).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid1.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid2.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test2");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid3.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test3");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid4.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test4");
            }, CancellationToken.None).Wait();
        });

        _asvSdrServerEx.Base.OnRecordDeleteRequest.Subscribe(_ =>
        {
            Assert.Equal(guid2.ToByteArray(), _.RecordGuid);
            
            _asvSdrServerEx.Base.SendRecordDeleteResponseSuccess(_).Wait();
        });

        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var records = list.Items.ToList();
        var names = records.Select(record => record.Name.ToString()).ToList();

        Assert.Contains("Test1", names);
        Assert.Contains("Test2", names);
        Assert.Contains("Test3", names);
        Assert.Contains("Test4", names);
        
        await _asvSdrClientEx.DeleteRecord(guid2, CancellationToken.None);
        
        await Task.Delay(100);
        
        _asvSdrClientEx.Records.BindToObservableList(out list).Subscribe();
        records = list.Items.ToList();
        names = records.Select(record => record.Name.ToString()).ToList();
        
        Assert.DoesNotContain("Test2", names);
    }
    
    [Fact]
    public async Task Check_For_Failed_Record_Delete_Response()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,4).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid1.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid2.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test2");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid3.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test3");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid4.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test4");
            }, CancellationToken.None).Wait();
        });

        _asvSdrServerEx.Base.OnRecordDeleteRequest.Subscribe(_ =>
        {
            Assert.Equal(guid2.ToByteArray(), _.RecordGuid);
            
            _asvSdrServerEx.Base.SendRecordDeleteResponseFail(_, AsvSdrRequestAck.AsvSdrRequestAckFail).Wait();
        });

        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var records = list.Items.ToList();
        var names = records.Select(record => record.Name.ToString()).ToList();

        Assert.Contains("Test1", names);
        Assert.Contains("Test2", names);
        Assert.Contains("Test3", names);
        Assert.Contains("Test4", names);

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            try
            {
                await _asvSdrClientEx.DeleteRecord(guid2, CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                Assert.Equal("Request fail", e.Message);
                throw;
            }
        });
    }

    [Fact]
    public async Task Check_Correct_Record_Request()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,2).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
        });

        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var result = list.Items.ToList();

        var names = result.Select(record => record.Name.ToString()).ToList();
        Assert.Contains("Test", names);
        Assert.Contains("Test1", names);
    }
    
    [Fact]
    public async Task Check_Incomplete_Record_Request()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,5).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecord(_ =>
            {
                Guid.NewGuid().TryWriteBytes(_.RecordGuid);
                MavlinkTypesHelper.SetString(_.RecordName, "Test1");
            }, CancellationToken.None).Wait();
        });

        var result = await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        Assert.False(result);
    }
    
    [Fact]
    public async Task Check_Correct_Record_Tag_Request()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        _asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,2).Wait();
            
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });

        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await _asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 2, CancellationToken.None);
        await results[0].UploadTagList();
        results[0].Tags.BindToObservableList(out var tags).Subscribe();
        var tagList = tags.Items.ToList();
        
        Assert.Equal(2, tagList.Count);

        var names = tagList.Select(tag => tag.Name).ToList();
        Assert.Contains("TestTag1", names);
        Assert.Contains("TestTag2", names);
    }
    
    [Fact]
    public async Task Check_Incorrect_Record_Tag_Request()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();

        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        _asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,5).Wait();
            
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });
        
        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await _asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 5, CancellationToken.None);
        await Task.Delay(500);
        await results[0].UploadTagList(); // bug: infinite loop if received tags count is less than requested tags count
        // foreach (var result in results)
        // {
        //     await result.UploadTagList(); // bug: change UploadTagList name to DownloadTagList
        // }
        
        results[0].Tags.BindToObservableList(out var tags).Subscribe();
        var tagList = tags.Items.ToList();
        
        Assert.Equal(2, tagList.Count);
        
        var names = tagList.Select(tag => tag.Name).ToList();
        Assert.Contains("TestTag1", names);
        Assert.Contains("TestTag2", names);
    }
    
    [Fact]
    public async Task Check_For_Successful_Record_Tag_Delete_Request()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();
        
        _asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            _asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        _asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            _asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,2).Wait();
            
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            _asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });

        await _asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        _asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await _asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 2, CancellationToken.None);
        await results[0].UploadTagList();
        results[0].Tags.BindToObservableList(out var tags).Subscribe();
        var tagList = tags.Items.ToList();
        
        Assert.Equal(2, tagList.Count);

        var names = tagList.Select(tag => tag.Name).ToList();
        Assert.Contains("TestTag1", names);
        Assert.Contains("TestTag2", names);

        _asvSdrServerEx.Base.OnRecordTagDeleteRequest.Subscribe(_ =>
        {
            _asvSdrClientEx.Records.BindToObservableList(out var innerList).Subscribe();
            var records = innerList.Items.ToList();
            records[0].DeleteTag(new TagId(recordTag1Guid, recordGuid), CancellationToken.None);
            _asvSdrServerEx.Base.SendRecordTagDeleteResponseSuccess(_).Wait();
        });

        await _asvSdrClientEx.Base.DeleteRecordTag(new TagId(recordTag1Guid, recordGuid), CancellationToken.None);
        
        await Task.Delay(100);
        
        //await _asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 1, CancellationToken.None);
        await results[0].UploadTagList();
        //await Task.Delay(100);
        
        _asvSdrClientEx.Records.BindToObservableList(out list).Subscribe();
        results = list.Items.ToList();
        results[0].Tags.BindToObservableList(out tags).Subscribe();
        tagList = tags.Items.ToList();
        
        Assert.Single(tagList);

        names = tagList.Select(tag => tag.Name).ToList();
        
        Assert.DoesNotContain("TestTag1", names);
    }

    [Fact]
    public async Task Check_For_Correct_StartRecord_Behavior()
    {
        HeartBeatServer.Start();
        AsvSdrServer.Start();
        await HeartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);

        var mavResult = await _asvSdrClientEx.StartRecord("TestRecord", CancellationToken.None);
        await Task.Delay(100);
        
    }
}