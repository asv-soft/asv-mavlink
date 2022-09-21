using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Asv.Common;
using Asv.Mavlink.Payload.Digits;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Payload.Test
{


    public static class ParamDefinition
    {
        public const string GroupName = "Group1";

        [Export(typeof(Pv2ParamType))]
        public static Pv2IntParamType ParamInt1 = new(nameof(ParamInt1),$"Description of {nameof(ParamInt1)}", GroupName,"{0}","items");

        [Export(typeof(Pv2ParamType))]
        public static Pv2IntParamType ParamInt2 = new(nameof(ParamInt2), $"Description of {nameof(ParamInt2)}", GroupName, "{0}", "items", -10,20);


        [Export(typeof(Pv2ParamType))]
        public static Pv2UIntParamType ParamUint2 = new(nameof(ParamUint2), $"Description of {nameof(ParamInt2)}", GroupName, "{0}", "items", 0, 100);

        public static Pv2EnumParamType ParamEnum = new(nameof(ParamEnum),$"Description of {nameof(ParamEnum)}","Group1","Item1", Pv2ParamFlags.NoFlags,
            "Item1",
            "Item2"
        );

        public static Pv2FlagsParamType ParamFlags = new(nameof(ParamFlags), "Description", "Group0", "On","Off", Pv2ParamFlags.NoFlags,
            ("Aa1",true),
            ("Aa2",false)
        );


    }



    public class PayloadV2ParamsTest
    {
        private readonly ITestOutputHelper _output;

        public IEnumerable<Assembly> RegisterAssembly => new[]
        {
            typeof(PayloadV2ParamsTest).Assembly,
        };

        public PayloadV2ParamsTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ReadOnlyPramsWriteFail()
        {
            var i = 0;
            var j = 0;
            var values = new List<Pv2ParamType>
            {
                new Pv2IntParamType($"INT{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items", flags:Pv2ParamFlags.ReadOnly),
                new Pv2UIntParamType($"UINT{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items", flags:Pv2ParamFlags.ReadOnly),
                new Pv2DoubleParamType($"Double{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items", flags:Pv2ParamFlags.ReadOnly),
                new Pv2FloatParamType($"Float{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items", flags:Pv2ParamFlags.ReadOnly),
                new Pv2StringParamType($"String{i}", $"Description of param {i}", $"GROUP{j}", defaultValue:$"{i:N5}", flags:Pv2ParamFlags.ReadOnly),
                new Pv2EnumParamType($"Enum{i}", $"Description of param {i}", $"GROUP{j}", "ITEM1", Pv2ParamFlags.ReadOnly, "ITEM0", "ITEM1", "ITEM2"),
                new Pv2BoolParamType($"Bool{i}", $"Description of param {i}", $"GROUP{j}", flags:Pv2ParamFlags.ReadOnly),
                new Pv2FlagsParamType($"Flags{i}", $"Description of param {i}", $"GROUP{j}", "On", "Off", Pv2ParamFlags.ReadOnly,
                    ("Flag0", true),
                    ("Flag1", true),
                    ("Flag2", true),
                    ("Flag3", true),
                    ("Flag4", true),
                    ("Flag5", true)
                )
            };
            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);
            await clientParams.RequestAll();

            foreach (var value in values)
            {
                await Assert.ThrowsAsync<InternalPv2Exception>(async () =>
                {
                    await clientParams.Write(value, (type, paramValue) => { });
                });
            }
            

        }


        [Fact]
        public void ParamArgsError()
        {
            var param = new Pv2IntParamType("Param", string.Empty.PadLeft(Pv2ParamInterface.MaxDescriptionLength), "group1", string.Empty, string.Empty);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var param = new Pv2IntParamType("Param", string.Empty.PadLeft(Pv2ParamInterface.MaxDescriptionLength+1), "group1", string.Empty, string.Empty);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                var param = new Pv2IntParamType("1Param", string.Empty.PadLeft(Pv2ParamInterface.MaxDescriptionLength), "group1", string.Empty, string.Empty);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                var param = new Pv2IntParamType("Pa1ram", string.Empty.PadLeft(Pv2ParamInterface.MaxDescriptionLength), "1group1", string.Empty, string.Empty);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                var param = new Pv2IntParamType("Pa1ram", string.Empty.PadLeft(Pv2ParamInterface.MaxDescriptionLength), "1group1", string.Empty, string.Empty,20,10);
            });
        }

        [Theory]
        [InlineData(1,   1)]
        [InlineData(1,   3)]
        [InlineData(10,  1)]
        [InlineData(10,  3)]
        [InlineData(256, 1)]
        [InlineData(50,  3)]
        [InlineData(50, 10)]
        public async Task Over300Params(int count, int groups)
        {
            var values = new List<Pv2ParamType>(count);
            for (var j = 0; j < groups; j++)
            {
                for (var i = 0; i < count; i++)
                {
                    values.Add(new Pv2IntParamType($"INT{i}", $"Description of param {i}", $"GROUP{j}","{0} items","items"));
                    values.Add(new Pv2UIntParamType($"UINT{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items"));
                    values.Add(new Pv2DoubleParamType($"Double{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items"));
                    values.Add(new Pv2FloatParamType($"Float{i}", $"Description of param {i}", $"GROUP{j}", "{0} items", "items"));
                    values.Add(new Pv2StringParamType($"String{i}", $"Description of param {i}", $"GROUP{j}",$"{i:N5}"));
                    values.Add(new Pv2EnumParamType($"Enum{i}", $"Description of param {i}", $"GROUP{j}", "ITEM1", Pv2ParamFlags.NoFlags, "ITEM0","ITEM1","ITEM2"));
                    values.Add(new Pv2BoolParamType($"Bool{i}", $"Description of param {i}", $"GROUP{j}"));
                    values.Add(new Pv2FlagsParamType($"Flags{i}", $"Description of param {i}", $"GROUP{j}","On","Off", Pv2ParamFlags.NoFlags,
                        ("Flag0",true),
                        ("Flag1", true),
                        ("Flag2", true),
                        ("Flag3", true),
                        ("Flag4", true),
                        ("Flag5", true)
                    ));
                }
            }
            _output.WriteLine($"Total params {values.Count} items");
            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);
               
            //serverParams.IsSendUpdateEnabled = false;

            for (var i = 0; i < values.Count; i++)
            {
                switch (values[i].TypeEnum)
                {
                    case Pv2ParamTypeEnum.Unknown:
                        break;
                    case Pv2ParamTypeEnum.Int:
                        serverParams.WriteInt(values[i], 10 * (i % 2 == 0 ? -1 : 1));
                        var value = serverParams.ReadInt(values[i]);
                        Assert.Equal(10 * (i % 2 == 0 ? -1 : 1), value);
                        break;
                    case Pv2ParamTypeEnum.UInt:
                        serverParams.WriteUInt(values[i], (uint)(i * 20));
                        var value2 = serverParams.ReadUInt(values[i]);
                        Assert.Equal((uint)(i * 20), value2);
                        break;
                    case Pv2ParamTypeEnum.Double:
                        serverParams.WriteDouble(values[i], (i * double.Epsilon));
                        var value3 = serverParams.ReadDouble(values[i]);
                        Assert.Equal((i * double.Epsilon), value3);
                        break;
                    case Pv2ParamTypeEnum.Float:
                        serverParams.WriteFloat(values[i], (i * float.Epsilon));
                        var value4 = serverParams.ReadFloat(values[i]);
                        Assert.Equal((i * float.Epsilon), value4);
                        break;
                    case Pv2ParamTypeEnum.String:
                        serverParams.WriteString(values[i], (i * float.Epsilon).ToString());
                        var value5 = serverParams.ReadString(values[i]);
                        Assert.Equal((i * float.Epsilon).ToString(), value5);
                        break;
                    case Pv2ParamTypeEnum.Enum:
                        var enumStr = "ITEM" + (i % 3);
                        serverParams.WriteEnum(values[i], enumStr);
                        var value9 = serverParams.ReadEnum(values[i]);
                        Assert.Equal(enumStr, value9);
                        break;
                    case Pv2ParamTypeEnum.Bool:
                        var boolVal = i%2 == 0;
                        serverParams.WriteBool(values[i], boolVal);
                        var value10 = serverParams.ReadBool(values[i]);
                        Assert.Equal(boolVal, value10);
                        break;
                    case Pv2ParamTypeEnum.Flags:
                        var flagValue = new UintBitArray(Enumerable.Range(0,6).Select(_=>_%3==0));
                        serverParams.WriteFlagValue(values[i], flagValue);
                        var value11 = serverParams.ReadFlagValue(values[i]);
                        Assert.Equal(flagValue.Value, value11.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            var sw = new Stopwatch();
            while (serverParams.IsSendingUpdate.Value == false)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(30));
            }
            sw.Start();
            while (serverParams.IsSendingUpdate.Value == true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(30));
            }

            //serverParams.IsSendUpdateEnabled = true;
            _output.WriteLine($"Sending updated {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            await clientParams.RequestAll();

            sw.Stop();
            _output.WriteLine($"Request all by {sw.ElapsedMilliseconds} ms");
            for (var i = 0; i < values.Count; i++)
            {
                switch (values[i].TypeEnum)
                {
                    case Pv2ParamTypeEnum.Unknown:
                        break;
                    case Pv2ParamTypeEnum.Int:
                        var value = clientParams.ReadInt(values[i]);
                        var serverValue = serverParams.ReadInt(values[i]);
                        Assert.Equal(serverValue, value);
                        break;
                    case Pv2ParamTypeEnum.UInt:
                        var value2 = clientParams.ReadUInt(values[i]);
                        var serverValue2 = serverParams.ReadUInt(values[i]);
                        Assert.Equal(serverValue2, value2);
                        break;
                    case Pv2ParamTypeEnum.Double:
                        var value3 = clientParams.ReadDouble(values[i]);
                        var serverValue3 = serverParams.ReadDouble(values[i]);
                        Assert.Equal(serverValue3, value3);
                        break;
                    case Pv2ParamTypeEnum.Float:
                        var value4 = clientParams.ReadFloat(values[i]);
                        var serverValue4 = serverParams.ReadFloat(values[i]);
                        Assert.Equal(value4, serverValue4);
                        break;
                    case Pv2ParamTypeEnum.String:
                        var value5 = clientParams.ReadString(values[i]);
                        var serverValue5 = serverParams.ReadString(values[i]);
                        Assert.Equal(value5, serverValue5);
                        break;
                    case Pv2ParamTypeEnum.Enum:
                        var enumStr = "ITEM" + (i % 3);
                        var value9 = clientParams.ReadEnum(values[i]);
                        var serverValue9 = serverParams.ReadEnum(values[i]);
                        Assert.Equal(enumStr, value9);
                        Assert.Equal(enumStr, serverValue9);
                        break;
                    case Pv2ParamTypeEnum.Bool:
                        var boolVal = i % 2 == 0;
                        var value10 = clientParams.ReadBool(values[i]);
                        var serverValue10 = serverParams.ReadBool(values[i]);
                        Assert.Equal(boolVal, value10);
                        Assert.Equal(boolVal, serverValue10);
                        break;
                    case Pv2ParamTypeEnum.Flags:
                        var flagValue = new UintBitArray(Enumerable.Range(0, 6).Select(_ => _ % 3 == 0));
                        var value11 = clientParams.ReadFlagValue(values[i]);
                        var serverValue11 = serverParams.ReadFlagValue(values[i]);
                        Assert.Equal(flagValue.Value, value11.Value);
                        Assert.Equal(flagValue.Value, serverValue11.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            sw.Restart();
            await clientParams.RequestAll();
            _output.WriteLine($"Request only values {sw.ElapsedMilliseconds} ms");
            var rxBytesSrv = serverParams.Server.Server.MavlinkV2Connection.DataStream.RxBytes;
            var txBytesSrv = serverParams.Server.Server.MavlinkV2Connection.DataStream.TxBytes;
            var rxPacketsSrv = serverParams.Server.Server.MavlinkV2Connection.RxPackets;
            var txPacketsSrv = serverParams.Server.Server.MavlinkV2Connection.TxPackets;
            _output.WriteLine($"SERVER: TX {txBytesSrv:N} bytes ({txPacketsSrv:N} packets)");
            _output.WriteLine($"SERVER: RX {rxBytesSrv:N} bytes ({rxPacketsSrv:N} packets)");

            var rxBytesClient = clientParams.Client.Client.MavlinkV2Connection.DataStream.RxBytes;
            var txBytesClient = clientParams.Client.Client.MavlinkV2Connection.DataStream.TxBytes;
            var rxPacketsClient = clientParams.Client.Client.MavlinkV2Connection.RxPackets;
            var txPacketsClient = clientParams.Client.Client.MavlinkV2Connection.TxPackets;
            _output.WriteLine($"CLIENT: TX {txBytesClient:N} bytes ({txPacketsClient:N} packets)");
            _output.WriteLine($"CLIENT: RX {rxBytesClient:N} bytes ({rxPacketsClient:N} packets)");

        }

        [Fact]
        public void ServerReadWrite()
        {
            var container = new CompositionContainer(new AggregateCatalog(RegisterAssembly.Distinct().Select(_ => new AssemblyCatalog(_)).OfType<ComposablePartCatalog>()));
            var a = ParamDefinition.ParamInt1;
            var values = container.GetExportedValues<Pv2ParamType>();

            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);

            serverParams.WriteInt(ParamDefinition.ParamInt1, 10);
            var value = serverParams.ReadInt(ParamDefinition.ParamInt1);
            Assert.Equal(10, value);

            serverParams.WriteInt(ParamDefinition.ParamInt2, 5);
            var value2 = serverParams.ReadInt(ParamDefinition.ParamInt2);
            Assert.Equal(5, value2);

        }

        [Fact]
        public async Task ClientSyncParams()
        {
            var container = new CompositionContainer(new AggregateCatalog(RegisterAssembly.Distinct().Select(_ => new AssemblyCatalog(_)).OfType<ComposablePartCatalog>()));
            var values = container.GetExportedValues<Pv2ParamType>();

            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);

            serverParams.WriteInt(ParamDefinition.ParamInt1, 10);
            serverParams.WriteInt(ParamDefinition.ParamInt2, 5);
            await clientParams.RequestAll();
            var readedInt = clientParams.ReadInt(ParamDefinition.ParamInt1);
            Assert.Equal(10, readedInt);

            var readedInt2 = clientParams.ReadInt(ParamDefinition.ParamInt2);
            Assert.Equal(5, readedInt2);
        }

        [Fact]
        public async Task WriteParamByClient()
        {
            var container = new CompositionContainer(new AggregateCatalog(RegisterAssembly.Distinct().Select(_ => new AssemblyCatalog(_)).OfType<ComposablePartCatalog>()));
            var values = container.GetExportedValues<Pv2ParamType>();

            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);

            serverParams.WriteInt(ParamDefinition.ParamInt1, 10);
            serverParams.WriteInt(ParamDefinition.ParamInt2, 5);
            await clientParams.RequestAll();
            bool updated = false;
            var sw = new Stopwatch();
            serverParams.OnRemoteUpdated.Where(_=> _.Type.FullName == ParamDefinition.ParamInt1.FullName).Subscribe(_=>
            {
                _output.WriteLine($"Server rece {sw.ElapsedMilliseconds} ms");
                updated = true;
            });
            sw.Start();
            var clientRes20 = await clientParams.WriteInt(ParamDefinition.ParamInt1, 20);
            Assert.Equal(20, clientParams.ReadInt(ParamDefinition.ParamInt1));
            Assert.Equal(20,clientRes20);

            var serverRes20 = serverParams.ReadInt(ParamDefinition.ParamInt1);
            Assert.Equal(20, serverRes20);
            Assert.True(updated);
        }

        

        


        [Fact]
        public async Task UpdateRiseByServer()
        {
            var container = new CompositionContainer(new AggregateCatalog(RegisterAssembly.Distinct().Select(_ => new AssemblyCatalog(_)).OfType<ComposablePartCatalog>()));
            var values = container.GetExportedValues<Pv2ParamType>();

            PayloadV2TestHelper.CreateParams(out var clientParams, out var serverParams, values);
            await clientParams.RequestAll();
            var serverBefore = serverParams.ReadInt(ParamDefinition.ParamInt1);
            var clientBefore = clientParams.ReadInt(ParamDefinition.ParamInt1);
            Assert.Equal(serverBefore,clientBefore);
            serverParams.WriteInt(ParamDefinition.ParamInt1,100);
            await Task.Delay(2_000);
            var clientAfter = clientParams.ReadInt(ParamDefinition.ParamInt1);
            var serverAfter = serverParams.ReadInt(ParamDefinition.ParamInt1);
            Assert.Equal(clientAfter, serverAfter);
        }

        
    }
}
