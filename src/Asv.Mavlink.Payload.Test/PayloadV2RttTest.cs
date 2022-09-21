using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Asv.Common;
using DeepEqual.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Payload.Test
{

    public static class IlsRtt
    {
        public static readonly Pv2RttRecordDesc IlsRecord1 =
            new(0,"ILS", "ILS record", Pv2RttRecordFlags.NoFlags);

        public static readonly Pv2RttRecordDesc IlsRecord2 =
            new(1, "ILS", "ILS record", Pv2RttRecordFlags.NoFlags);

        public static Pv2RttFixedPointDesc Power11 =
            new(0, nameof(Power11), "ILS power field description", "dBm","{0:F3}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01,double.NaN, IlsRecord1);
        public static Pv2RttFixedPointDesc Power12 =
            new(1, nameof(Power12), "ILS power field description", "dBm", "{0:F2}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01, double.NaN, IlsRecord1);
        public static Pv2RttFixedPointDesc Power13 =
            new(2, nameof(Power13), "ILS power field description", "dBm", "{0:F1}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01, double.NaN, IlsRecord1);


        public static Pv2RttFixedPointDesc Power21 =
            new(0, nameof(Power21), "ILS power field description", "dBm", "{0:F3}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01, double.NaN, IlsRecord2);
        public static Pv2RttFixedPointDesc Power22 =
            new(1, nameof(Power22), "ILS power field description", "dBm", "{0:F2}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01, double.NaN, IlsRecord2);
        public static Pv2RttFixedPointDesc Power23 =
            new(2, nameof(Power23), "ILS power field description", "dBm", "{0:F1}", Pv2RttFieldFlags.NoFlags, BitSizeEnum.S14Bits, -50, 0.01, double.NaN, IlsRecord2);



        public static IEnumerable<Pv2RttRecordDesc> Records
        {
            get
            {
                yield return IlsRecord1;
                yield return IlsRecord2;
            }
        }

        public static IEnumerable<Pv2RttFieldDesc> Fields
        {
            get
            {
                yield return Power11;
                yield return Power12;
                yield return Power13;
                yield return Power21;
                yield return Power22;
                yield return Power23;

            }
        }
        
    }


    public class PayloadV2RttTest
    {
        private readonly ITestOutputHelper _output;

        public PayloadV2RttTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(1000,30)]
        [InlineData(5000, 30)]
        [InlineData(10000, 30)]
        [InlineData(10000, 1000)]
        public async Task ReadData(int delayMs, uint recTimeMs)
        {
            var (client, server) = await PayloadV2TestHelper.CreateServerAndClientDevices(IlsRtt.Records, IlsRtt.Fields, recTimeMs, _output.WriteLine);
            server.Rtt[IlsRtt.Power11] = -70.0;
            server.Rtt[IlsRtt.Power12] = -60.0;
            server.Rtt[IlsRtt.Power13] = -50.0;
            server.Rtt[IlsRtt.Power21] = -40.0;
            server.Rtt[IlsRtt.Power22] = -20.0;
            server.Rtt[IlsRtt.Power23] = -10.0;
            client.Rtt.OnUpdate.Subscribe(_ =>
            {
                if ((_.Metadata.Flags & Pv2StreamFlags.RecordEnabled) != 0)
                {
                    
                }
            } );
            var result = await client.Rtt.StartRecord(new SessionSettings("Rec1", "Tag1"));
            _output.WriteLine($"Start record and wait {delayMs} sec");
            await Task.Delay(delayMs);
            _output.WriteLine("Stop record");
            await client.Rtt.StopRecord();
            var sw = new Stopwatch();
            sw.Start();
            var storeInfo = await client.Rtt.GetStoreInfo();
            Assert.Equal(1U, storeInfo.SessionCount);
            var sessionInfo = await client.Rtt.GetSessionInfo(0U);
            Assert.Equal((int)sessionInfo.FieldsCount, IlsRtt.Fields.Count());
            result.Session.WithDeepEqual(sessionInfo.Metadata).Assert();
            var fieldsIds = new List<(uint id,uint count)>();
            for (var i = 0; i < sessionInfo.FieldsCount; i++)
            {
                var recordInfo = await client.Rtt.GetSessionFieldInfo(sessionInfo.Metadata.SessionId, (uint)i);
                _output.WriteLine($"{recordInfo.Metadata.Settings.Name, -20} [{recordInfo.Metadata.Settings.Id}]: {recordInfo.Count} ({recordInfo.SizeInBytes:N} bytes)");
                fieldsIds.Add((recordInfo.Metadata.Settings.Id, recordInfo.Count));
                Assert.Contains(IlsRtt.Fields, _ => _.FullId == recordInfo.Metadata.Settings.Id);
            }
            sw.Stop();
            _output.WriteLine($"Readed all store description by {sw.Elapsed.TotalSeconds:F1} sec");
            sw.Restart();
            foreach (var item in fieldsIds)
            {
                var values = new List<object>();
                var sw2 = new Stopwatch();
                sw2.Start();
                var readCount = 0;
                while (values.Count < item.count)
                {
                    var data = await client.Rtt.GetSessionFieldData(result.Session.SessionId, item.id, (uint)values.Count, byte.MaxValue);
                    readCount++;
                    foreach (var valueTuple in data.data)
                    {
                        values.Add(valueTuple.value);
                        if (data.desc.FullId == IlsRtt.Power11.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power11], valueTuple.value);
                        }
                        if (data.desc.FullId == IlsRtt.Power12.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power12], valueTuple.value);
                        }
                        if (data.desc.FullId == IlsRtt.Power13.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power13], valueTuple.value);
                        }
                        if (data.desc.FullId == IlsRtt.Power21.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power21], valueTuple.value);
                        }
                        if (data.desc.FullId == IlsRtt.Power22.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power22], valueTuple.value);
                        }
                        if (data.desc.FullId == IlsRtt.Power23.FullId)
                        {
                            Assert.Equal(server.Rtt[IlsRtt.Power23], valueTuple.value);
                        }
                    }

                }
                sw2.Stop();
                _output.WriteLine($"[{item.id}] readed {values.Count} records by {sw2.Elapsed.TotalSeconds:F2} sec ({readCount} read)");
            }
            sw.Stop();
            _output.WriteLine($"Readed all store data by {sw.Elapsed.TotalSeconds:F1} sec");
        }


        [Fact]
        public async Task StartStopMultiple()
        {
            var (client,server) = await PayloadV2TestHelper.CreateServerAndClientDevices(IlsRtt.Records, IlsRtt.Fields, 1000);
            var settings = new SessionSettings($"name1");
            var recordInfo = await client.Rtt.StartRecord(settings);
            Assert.True(recordInfo.IsEnabled);
            recordInfo.Session.Settings.WithDeepEqual(settings);
            Assert.True(await client.Rtt.StopRecord());

            var session2 = await client.Rtt.StartRecord(new SessionSettings($"name2"));
            Assert.True(await client.Rtt.StopRecord());

            //Pv2RttSessionStoreInfo info = client.Rtt.GetStoreInfo();

        }

        [Fact]
        public async Task StartStopRecord()
        {
            var server = PayloadV2TestHelper.CreateServerDevice(out var port, IlsRtt.Records, IlsRtt.Fields,  out var rttFolder, 10);
            var client = PayloadV2TestHelper.CreateClientDevice(port);
            server.Rtt[IlsRtt.Power11] = 1000.0;
            var sw = new Stopwatch();
            sw.Start();
            await client.InitState.Where(_ => _ == VehicleInitState.Complete).FirstAsync();
            _output.WriteLine($"Connected by {sw.ElapsedMilliseconds:N0} ms");
            var result = await client.Rtt.StartRecord(new SessionSettings("session1", "tag1"));
            Assert.Equal("session1",result.Session.Settings.Name);
            Assert.Contains("tag1", result.Session.Settings.Tags);
            Assert.NotEqual(Guid.Empty,result.Session.SessionId.Guid);
            Assert.True(result.IsEnabled);
            _output.WriteLine("Wait 10 sec");
            await Task.Delay(TimeSpan.FromSeconds(10));
            _output.WriteLine("Stop record");
            var stoped = await client.Rtt.StopRecord();
            Assert.True(stoped);
            
            _output.WriteLine($"Folder: {rttFolder}");
        }

        [Fact]
        public async Task ConnectionTest()
        {
            var server =  PayloadV2TestHelper.CreateServerDevice(out var port, IlsRtt.Records, IlsRtt.Fields, out var rttFolder,1000);
            var client = PayloadV2TestHelper.CreateClientDevice(port);
            
            await client.Client.Client.Heartbeat.Link.Where(_ => _ == LinkState.Connected).FirstAsync();

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                server.Rtt[IlsRtt.Power11] = -120 + _*0.01;
                server.Rtt[IlsRtt.Power12] = -120 + _ * 0.05;
                server.Rtt[IlsRtt.Power13] = -120 + _ * 0.5;

                server.Rtt[IlsRtt.Power21] = 30 - _ * 0.01;
                server.Rtt[IlsRtt.Power22] = 30 - _ * 0.10;
                server.Rtt[IlsRtt.Power23] = 30 - _ * 0.5;
            });
            server.Rtt[IlsRtt.Power11] = -70.01;
            server.Rtt[IlsRtt.Power12] = -70.01;
            server.Rtt[IlsRtt.Power13] = -70.01;

            server.Rtt[IlsRtt.Power21] = -70.01;
            server.Rtt[IlsRtt.Power22] = -70.01;
            server.Rtt[IlsRtt.Power23] = -70.01;

            var tcs = new TaskCompletionSource<Unit>();
            var rttPass = false;
           
            client.Rtt.OnUpdate.Subscribe(_ =>
            {
                rttPass = true;
                if (rttPass) tcs.TrySetResult(Unit.Default);
            });
            await tcs.Task;
            // server.Rtt[IlsRtt.Power] = 100.1;
        }
    }
}
