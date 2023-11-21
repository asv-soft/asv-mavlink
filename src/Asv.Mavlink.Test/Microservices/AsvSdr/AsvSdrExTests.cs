#nullable enable
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.V2.AsvSdr;
using Asv.Mavlink.V2.Common;
using DynamicData;
using DynamicData.Binding;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test;

public class AsvSdrExTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    private async Task<(IAsvSdrClientEx, IAsvSdrServerEx)> SetUpConnection()
    {
        var link = new VirtualMavlinkConnection();
        var mavlinkClientIdentity = new MavlinkClientIdentity() { SystemId = 1, ComponentId = 1, TargetSystemId = 2, TargetComponentId = 2 };
        var heartBeatClient = new HeartbeatClient(link.Client, mavlinkClientIdentity, new PacketSequenceCalculator(), new HeartbeatClientConfig(), Scheduler.Default);
        var commandClient = new CommandClient(link.Client, mavlinkClientIdentity, new PacketSequenceCalculator(), new CommandProtocolConfig());
        var asvSdrClient = new AsvSdrClient(link.Client, mavlinkClientIdentity, new PacketSequenceCalculator());
        var asvSdrClientEx = new AsvSdrClientEx(asvSdrClient, heartBeatClient, commandClient, new AsvSdrClientExConfig());

        var pkt = new PacketSequenceCalculator();
        var mavlinkServerIdentity = new MavlinkServerIdentity() { SystemId = 2, ComponentId = 2 };
        var heartBeatServer = new HeartbeatServer(link.Server, pkt, mavlinkServerIdentity, new MavlinkHeartbeatServerConfig(), Scheduler.Default);
        var commandServer = new CommandServer(link.Server, pkt, mavlinkServerIdentity, Scheduler.Default);
        var commandLongServerEx = new CommandLongServerEx(commandServer);
        var status = new StatusTextServer(link.Server, pkt, mavlinkServerIdentity, new StatusTextLoggerConfig(), Scheduler.Default);
        var asvSdrServer = new AsvSdrServer(link.Server, mavlinkServerIdentity, new AsvSdrServerConfig(), pkt, Scheduler.Default);
        var asvSdrServerEx = new AsvSdrServerEx(asvSdrServer,status, heartBeatServer, commandLongServerEx);
        
        heartBeatServer.Start();
        asvSdrServer.Start();
        await heartBeatClient.Link.FirstAsync(_ => _ == LinkState.Connected);
        
        return (asvSdrClientEx, asvSdrServerEx);
    }

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
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.SetMode = (mode, hz, rate, ratio, refPower, cancel) =>
        {
            Assert.Equal(customMode, mode);
            Assert.Equal(11223U, hz);
            Assert.Equal(1, rate);
            Assert.Equal(1U, ratio);
            Assert.Equal(-90, refPower);
            return Task.FromResult(MavResult.MavResultAccepted);
        };

        var result = await asvSdrClientEx.SetMode(customMode, 11223, 1, 1, -90, CancellationToken.None);
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
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();
        
        asvSdrServerEx.SetMode = (_, _, _, _, _,_) => Task.FromResult(mavResult);

        var result = await asvSdrClientEx.SetMode(AsvSdrCustomMode.AsvSdrCustomModeLlz, 11223, 1, 1, -90.5f, CancellationToken.None);
        Assert.Equal(mavResult, result);
    }

    [Fact]
    public async Task Check_For_Successful_Record_Delete_Response()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,4).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid1.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid2.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test2");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid3.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test3");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid4.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test4");
            }, CancellationToken.None).Wait();
        });

        asvSdrServerEx.Base.OnRecordDeleteRequest.Subscribe(_ =>
        {
            Assert.Equal(guid2.ToByteArray(), _.RecordGuid);
            
            asvSdrServerEx.Base.SendRecordDeleteResponseSuccess(_).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var records = list.Items.ToList();
        var names = records.Select(record => record.Name.ToString()).ToList();

        Assert.Contains("Test1", names);
        Assert.Contains("Test2", names);
        Assert.Contains("Test3", names);
        Assert.Contains("Test4", names);
        
        await asvSdrClientEx.DeleteRecord(guid2, CancellationToken.None);
        
        await Task.Delay(100);
        
        asvSdrClientEx.Records.BindToObservableList(out list).Subscribe();
        records = list.Items.ToList();
        names = records.Select(record => record.Name.ToString()).ToList();
        
        Assert.DoesNotContain("Test2", names);
    }
    
    [Fact]
    public async Task Check_For_Failed_Record_Delete_Response()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,4).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid1.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid2.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test2");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid3.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test3");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                guid4.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test4");
            }, CancellationToken.None).Wait();
        });

        asvSdrServerEx.Base.OnRecordDeleteRequest.Subscribe(_ =>
        {
            Assert.Equal(guid2.ToByteArray(), _.RecordGuid);
            
            asvSdrServerEx.Base.SendRecordDeleteResponseFail(_, AsvSdrRequestAck.AsvSdrRequestAckFail).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
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
                await asvSdrClientEx.DeleteRecord(guid2, CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        });
    }

    [Fact]
    public async Task Check_Correct_Record_Request()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,2).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test1");
            }, CancellationToken.None).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var result = list.Items.ToList();

        var names = result.Select(record => record.Name.ToString()).ToList();
        Assert.Contains("Test", names);
        Assert.Contains("Test1", names);
    }
    
    [Fact]
    public async Task Check_Incomplete_Record_Request()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,5).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                Guid.NewGuid().TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecord(_ =>
            {
                Guid.NewGuid().TryWriteBytes(_.RecordGuid);
                MavlinkTypesHelper.SetString(_.RecordName, "Test1");
            }, CancellationToken.None).Wait();
        });

        var result = await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        Assert.False(result);
    }
    
    [Fact]
    public async Task Check_Correct_Record_Tag_Request()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,2).Wait();
            
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 2, CancellationToken.None);
        await results[0].DownloadTagList();
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
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();

        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,5).Wait();
            
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });
        
        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 5, CancellationToken.None);
        
        await results[0].DownloadTagList();
        foreach (var result in results)
        {
            await result.DownloadTagList();
        }
        
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
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,2).Wait();
            
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 2, CancellationToken.None);
        await results[0].DownloadTagList();
        results[0].Tags.BindToObservableList(out var tags).Subscribe();
        var tagList = tags.Items.ToList();
        
        Assert.Equal(2, tagList.Count);

        var names = tagList.Select(tag => tag.Name).ToList();
        Assert.Contains("TestTag1", names);
        Assert.Contains("TestTag2", names);

        asvSdrServerEx.Base.OnRecordTagDeleteRequest.Subscribe(_ =>
        {
            asvSdrClientEx.Records.BindToObservableList(out var innerList).Subscribe();
            var records = innerList.Items.ToList();
            records[0].DeleteTag(new TagId(recordTag1Guid, recordGuid), CancellationToken.None);
            asvSdrServerEx.Base.SendRecordTagDeleteResponseSuccess(_).Wait();
        });

        await asvSdrClientEx.Base.DeleteRecordTag(new TagId(recordTag1Guid, recordGuid), CancellationToken.None);
        
        await Task.Delay(100);
        
        await asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 1, CancellationToken.None);
        await results[0].DownloadTagList();
        await Task.Delay(100);
        
        asvSdrClientEx.Records.BindToObservableList(out list).Subscribe();
        results = list.Items.ToList();
        results[0].Tags.BindToObservableList(out tags).Subscribe();
        tagList = tags.Items.ToList();
        
        Assert.Single(tagList);

        names = tagList.Select(tag => tag.Name).ToList();
        
        Assert.DoesNotContain("TestTag1", names);
    }
    
    [Fact]
    public async Task Check_For_Failed_Record_Tag_Delete_Request()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        var recordGuid = Guid.NewGuid();
        var recordTag1Guid = Guid.NewGuid();
        var recordTag2Guid = Guid.NewGuid();
        var recordTag3Guid = Guid.NewGuid();
        
        asvSdrServerEx.Base.OnRecordRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordResponseSuccess(_,1).Wait();
            asvSdrServerEx.Base.SendRecord(__ =>
            {
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.RecordName, "Test");
            }, CancellationToken.None).Wait();
        });
        
        asvSdrServerEx.Base.OnRecordTagRequest.Subscribe(_ =>
        {
            asvSdrServerEx.Base.SendRecordTagResponseSuccess(_,2).Wait();
            
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag1Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag1");
            }, CancellationToken.None).Wait();
            asvSdrServerEx.Base.SendRecordTag(__ =>
            {
                recordTag2Guid.TryWriteBytes(__.TagGuid);
                recordGuid.TryWriteBytes(__.RecordGuid);
                MavlinkTypesHelper.SetString(__.TagName, "TestTag2");
            }, CancellationToken.None).Wait();
        });

        await asvSdrClientEx.DownloadRecordList(new Progress<double>(), CancellationToken.None);
        asvSdrClientEx.Records.BindToObservableList(out var list).Subscribe();
        var results = list.Items.ToList();
        
        await asvSdrClientEx.Base.GetRecordTagList(recordGuid, 0, 2, CancellationToken.None);
        await results[0].DownloadTagList();
        results[0].Tags.BindToObservableList(out var tags).Subscribe();
        var tagList = tags.Items.ToList();
        
        Assert.Equal(2, tagList.Count);

        var names = tagList.Select(tag => tag.Name).ToList();
        Assert.Contains("TestTag1", names);
        Assert.Contains("TestTag2", names);

        asvSdrServerEx.Base.OnRecordTagDeleteRequest.Subscribe(_ =>
        {
            asvSdrClientEx.Records.BindToObservableList(out var innerList).Subscribe();
            var records = innerList.Items.ToList();
            records[0].DeleteTag(new TagId(recordTag3Guid, recordGuid), CancellationToken.None);
            asvSdrServerEx.Base.SendRecordTagDeleteResponseFail(_, AsvSdrRequestAck.AsvSdrRequestAckFail).Wait();
        });

        var result = await asvSdrClientEx.Base.DeleteRecordTag(new TagId(recordTag3Guid, recordGuid), CancellationToken.None);
        
        Assert.True(result.Result == AsvSdrRequestAck.AsvSdrRequestAckFail);
    }

    [Fact]
    public async Task Check_For_Correct_StartRecord_Behavior()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.StartRecord = (name, cancel) =>
        {
            return name switch
            {
                "TestRecord" => Task.FromResult(MavResult.MavResultAccepted),
                _ => Task.FromResult<MavResult>(MavResult.MavResultFailed)
            };
        };

        var mavResult = await asvSdrClientEx.StartRecord("TestRecord", CancellationToken.None);
        Assert.True(mavResult == MavResult.MavResultAccepted);
    }
    
    [Fact]
    public async Task Check_For_Correct_StopRecord_Behavior()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.StopRecord = cancel => Task.FromResult(MavResult.MavResultAccepted);

        var mavResult = await asvSdrClientEx.StopRecord(CancellationToken.None);
        Assert.True(mavResult == MavResult.MavResultAccepted);
    }
    
    [Fact]
    public async Task Check_For_Correct_CurrentRecordSetTag_Behavior()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.CurrentRecordSetTag = (type, name, value, cancel) =>
        {
            AsvSdrHelper.CheckTagName(name);
            return Task.FromResult(MavResult.MavResultAccepted);
        };
        
        var nameArray = new byte[AsvSdrHelper.RecordTagValueLength];
        var mavResult = await asvSdrClientEx.CurrentRecordSetTag("test", AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, CancellationToken.None);
        Assert.True(mavResult == MavResult.MavResultAccepted);
    }
    
    [Fact]
    public async Task Check_For_Incorrect_StartRecord_Record_Name()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        await Assert.ThrowsAsync<Exception>( async () =>
        {
            try
            {
                await asvSdrClientEx.StartRecord("", CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                Assert.Equal("Record name is empty", e.Message);
                throw;
            }
        });
        
        await Assert.ThrowsAsync<Exception>( async () =>
        {
            try
            {
                await asvSdrClientEx.StartRecord("This string is too long to pass in command", CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                Assert.Equal($"Record name is too long. Max length is {AsvSdrHelper.RecordNameMaxLength}", e.Message);
                throw;
            }
        });
        
        await Assert.ThrowsAsync<ArgumentException>( async () =>
        {
            try
            {
                await asvSdrClientEx.StartRecord("Test*", CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                Assert.Equal("Record name 'Test*' not match regex '^[A-Za-z][A-Za-z0-9_\\- +]{2,28}$')", e.Message);
                throw;
            }
        });
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(9)]
    [InlineData(10)]
    public async Task Check_For_Incorrect_CurrentRecordSetTag_Raw_Value(int value)
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var nameArray = new byte[value];
            await asvSdrClientEx.CurrentRecordSetTag("test", AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, CancellationToken.None);
        });
    }
    
    [Fact]
    public async Task Check_For_Incorrect_CurrentRecordSetTag_Tag_Name()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();
        var nameArray = new byte[AsvSdrHelper.RecordTagValueLength];

        await Assert.ThrowsAsync<Exception>( async () =>
        {
            try
            {
                await asvSdrClientEx.CurrentRecordSetTag("", AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        });
        
        await Assert.ThrowsAsync<Exception>( async () =>
        {
            try
            {
                await asvSdrClientEx.CurrentRecordSetTag("This string is too long to pass in command", AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        });
        
        await Assert.ThrowsAsync<ArgumentException>( async () =>
        {
            try
            {
                await asvSdrClientEx.CurrentRecordSetTag("Test*", AsvSdrRecordTagType.AsvSdrRecordTagTypeString8, nameArray, CancellationToken.None);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
                throw;
            }
        });
    }

    [Fact]
    public async Task Check_System_Control_Action_Reboot_Behaviour()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.SystemControlAction = (action, cancel) =>
        {
            return action switch
            {
                AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot => Task.FromResult(
                    MavResult.MavResultAccepted),
                AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown => Task.FromResult(
                    MavResult.MavResultFailed),
                _ => Task.FromResult(MavResult.MavResultUnsupported)
            };
        };

        var mavRebootResult =
            await asvSdrClientEx.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot,
                CancellationToken.None);
        
        var mavShutdownResult = await asvSdrClientEx.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown,
            CancellationToken.None);
        
        Assert.True(mavRebootResult == MavResult.MavResultAccepted);
        Assert.True(mavShutdownResult == MavResult.MavResultFailed);
    }
    
    [Fact]
    public async Task Check_System_Control_Action_Shutdown_Behaviour()
    {
        var (asvSdrClientEx, asvSdrServerEx) = await SetUpConnection();

        asvSdrServerEx.SystemControlAction = (action, cancel) =>
        {
            return action switch
            {
                AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot => Task.FromResult(
                    MavResult.MavResultFailed),
                AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown => Task.FromResult(
                    MavResult.MavResultAccepted),
                _ => Task.FromResult(MavResult.MavResultUnsupported)
            };
        }; 

        var mavRebootResult =
            await asvSdrClientEx.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionReboot,
                CancellationToken.None);
        
        var mavShutdownResult = await asvSdrClientEx.SystemControlAction(AsvSdrSystemControlAction.AsvSdrSystemControlActionShutdown,
            CancellationToken.None);
        
        Assert.True(mavRebootResult == MavResult.MavResultFailed);
        Assert.True(mavShutdownResult == MavResult.MavResultAccepted);
    }
    
    
    [Theory]
    [InlineData(0)]
    [InlineData(256)]
    [InlineData(ushort.MaxValue)]
    public async Task Check_Start_Mission_Behaviour(ushort originIndex)
    {
        var (client, server) = await SetUpConnection();

        server.StartMission = (missionIndex, cancel) =>
        {
            Assert.Equal(originIndex, missionIndex);
            return Task.FromResult(MavResult.MavResultAccepted);
        };

        await client.StartMissionAndCheckResult(originIndex);
        
    }
    
    [Fact]
    public async Task Check_Stop_Mission_Behaviour()
    {
        var (client, server) = await SetUpConnection();

        server.StopMission = (cancel) => Task.FromResult(MavResult.MavResultAccepted);

        await client.StopMission();

    }
    
    
    [Fact]
    public async Task Client_call_start_calibration_server_respond()
    {
        var (client, server) = await SetUpConnection();
        server.StartCalibration = (cancel) => Task.FromResult(MavResult.MavResultAccepted);
        await client.StartCalibrationAndCheckResult();

    }
    
    [Fact]
    public async Task Client_call_unsupported_commands()
    {
        var (client, server) = await SetUpConnection();
        await Assert.ThrowsAsync<AsvSdrException>(async () =>
        {
            await client.StartCalibrationAndCheckResult();
        });
        await Assert.ThrowsAsync<AsvSdrException>(async () =>
        {
            await client.StopCalibrationAndCheckResult();
        });
    }

    [Fact]
    public async Task Client_read_calibration_table_list()
    {
        var (client, server) = await SetUpConnection();
        var date = DateTime.Now;
        server.Base.Set(_ => _.CalibTableCount = 2);
        server.TryReadCalibrationTableInfo =  (ushort index, out string? name,out ushort? size, out CalibrationTableMetadata? metadata) =>
        {
            switch (index)
            {
                case 0:
                    size = 10;
                    name = "Table 1";
                    metadata = new CalibrationTableMetadata(updated: date);
                    return true;
                case 1:
                    size = 20;
                    name = "Table 2";
                    metadata = new CalibrationTableMetadata(updated: date);
                    return true;
                default:
                    size = null;
                    name = null;
                    metadata = null;
                    return false;
            }
        };
        var status = await client.Base.Status.FirstAsync();
        await client.ReadCalibrationTableList();
        var sub = client.CalibrationTables.Bind(out var list).Subscribe();
        Assert.Equal(2, list.Count);
        Assert.Equal("Table 1", list[0].Name);
        Assert.Equal("Table 2", list[1].Name);
        Assert.Equal(10, list[0].RemoteSize.Value);
        Assert.Equal(20, list[1].RemoteSize.Value);
        Assert.Equal(0, list[0].Index);
        Assert.Equal(1, list[1].Index);
        Assert.True(date - list[0].Metadata.Value.Updated < TimeSpan.FromMilliseconds(100));
    }
    [Fact]
    public async Task Client_download_calibration_table()
    {
        var (client, server) = await SetUpConnection();
        var date = DateTime.Now;
        server.Base.Set(_ => _.CalibTableCount = 2);
        server.TryReadCalibrationTableInfo = (ushort index, out string? name,out ushort? size, out CalibrationTableMetadata? metadata) =>
        {
            switch (index)
            {
                case 0:
                    size = 10;
                    name = "Table 1";
                    metadata = new CalibrationTableMetadata(updated: date);
                    return true;
                case 1:
                    size = 20;
                    name = "Table 2";
                    metadata = new CalibrationTableMetadata(updated: date);
                    return true;
                default:
                    size = null;
                    name = null;
                    metadata = null;
                    return false;
            }
        };
        var status = await client.Base.Status.FirstAsync();
        await client.ReadCalibrationTableList();
        var sub = client.CalibrationTables.Bind(out var list).Subscribe();
        Assert.Equal(2, list.Count);
        Assert.Equal("Table 1", list[0].Name);
        Assert.Equal("Table 2", list[1].Name);
        Assert.Equal(10, list[0].RemoteSize.Value);
        Assert.Equal(20, list[1].RemoteSize.Value);
        Assert.Equal(0, list[0].Index);
        Assert.Equal(1, list[1].Index);
        Assert.True(date - list[0].Metadata.Value.Updated < TimeSpan.FromSeconds(1));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task Client_read_calibration_table_rows(ushort count)
    {
        var (client, server) = await SetUpConnection();
        var originName = "Table 1";
        var date = DateTime.Now;
        server.Base.Set(_ => _.CalibTableCount = 1);
        server.TryReadCalibrationTableInfo = (ushort index, out string? name,out ushort? size, out CalibrationTableMetadata? metadata) =>
        {
            if (index == 0)
            {
                size = count;
                name = originName;
                metadata = new CalibrationTableMetadata(updated: date);
                return true;
            }
            else
            {
                name = null;
                size = null;
                metadata = null;
                return false;
            }
        };
        server.TryReadCalibrationTableRow = (ushort tableIndex, ushort rowIndex, out CalibrationTableRow? row) => 
        {
            if (tableIndex != 0)
            {
                row = null;
                return false;
            }
            row = new CalibrationTableRow(rowIndex,rowIndex*0.1f,rowIndex*20,rowIndex*10);
            return true;

        };
        var status = await client.Base.Status.FirstAsync();
        await client.ReadCalibrationTableList();
        var sub = client.CalibrationTables.Bind(out var list).Subscribe();
        var table = await client.GetCalibrationTable(originName);
        Assert.NotNull(table);
        Assert.Equal(originName, table.Name);
        Assert.Equal(count, table.RemoteSize.Value);
        Assert.Equal(0, table.Index);
        Assert.True(date - table.Metadata.Value.Updated < TimeSpan.FromMilliseconds(100));
        var items = await table.Download();
        Assert.Equal(count, items.Length);
        for (int i = 0; i < count; i++)
        {
            Assert.Equal((ulong)i, items[i].FrequencyHz);
            Assert.Equal(i*0.1f, items[i].RefPower);
            Assert.Equal(i*20, items[i].RefValue);
            Assert.Equal(i*10, items[i].MeasuredValue);
        }
    }
    
    
    
    [Fact]
    public async Task Client_upload_and_download_with_server_error()
    {
        var (client, server) = await SetUpConnection();
        var originName = "Table 1";
        var date = DateTime.Now;

        server.Base.Set(_ => _.CalibTableCount = 1);
        server.TryReadCalibrationTableInfo = (ushort index, out string? name,out ushort? size, out CalibrationTableMetadata? metadata) =>
        {
            if (index == 0)
            {
                size = 10;
                name = originName;
                metadata = new CalibrationTableMetadata(updated: date);
                return true;
            }

            name = null;
            size = null;
            metadata = null;
            return false;
        };
        server.WriteCalibrationTable = (tableIndex, items) =>
        {
            throw new Exception("FATAL");
        };
        server.TryReadCalibrationTableRow =  (ushort tableIndex, ushort rowIndex, out CalibrationTableRow? row) => 
        {
            throw new Exception("FATAL");
        };
        
        
        
        
        var status = await client.Base.Status.FirstAsync();
        await client.ReadCalibrationTableList();
        var sub = client.CalibrationTables.Bind(out var list).Subscribe();
        var table = await client.GetCalibrationTable(originName);
        Assert.NotNull(table);
        await Assert.ThrowsAsync<AsvSdrException>(async () =>
        {
            var resultEmpty = await table.Download();
        });
        await Assert.ThrowsAsync<AsvSdrException>(async () =>
        {
            await table.Upload(new CalibrationTableRow[]{new CalibrationTableRow(100,0,0,0) });
        });
        
       
    }

}
