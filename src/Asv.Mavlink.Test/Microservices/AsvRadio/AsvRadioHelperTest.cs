using System;
using System.Collections.Immutable;
using System.Linq;
using Asv.Mavlink.V2.AsvAudio;
using Asv.Mavlink.V2.AsvRadio;
using Asv.Mavlink.V2.Common;
using Xunit;
using MavCmd = Asv.Mavlink.V2.Common.MavCmd;

namespace Asv.Mavlink.Test;

public class AsvRadioHelperTests
    {
        [Fact]
        public void SetArgsForRadioOn_CorrectlySetsParameters()
        {
            var command = new CommandLongPayload();
            AsvRadioHelper.SetArgsForRadioOn(command, 123456, AsvRadioModulation.AsvRadioModulationAm, -50.5f, 10.0f, AsvAudioCodec.AsvAudioCodecRaw, 1);

            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes(123456u)), command.Param1);
            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvRadioModulation.AsvRadioModulationAm)), command.Param2);
            Assert.Equal(-50.5f, command.Param3);
            Assert.Equal(10.0f, command.Param4);
            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvAudioCodec.AsvAudioCodecRaw)), command.Param5);
            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes(1u)), command.Param6);
        }

        [Fact]
        public void GetArgsForRadioOn_CorrectlyRetrievesParameters()
        {
            var command = new CommandLongPayload
            {
                Command = (MavCmd)V2.AsvRadio.MavCmd.MavCmdAsvRadioOn,
                Param1 = BitConverter.ToSingle(BitConverter.GetBytes(123456u)),
                Param2 = BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvRadioModulation.AsvRadioModulationFm)),
                Param3 = -50.5f,
                Param4 = 10.0f,
                Param5 = BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvAudioCodec.AsvAudioCodecOpus)),
                Param6 = BitConverter.ToSingle(BitConverter.GetBytes(1u)),
            };

            AsvRadioHelper.GetArgsForRadioOn(command, out uint frequencyHz, out AsvRadioModulation modulation, out float referenceRxPowerDbm, out float txPowerDbm, out AsvAudioCodec codec, out byte codecConfig);

            Assert.Equal(123456u, frequencyHz);
            Assert.Equal(AsvRadioModulation.AsvRadioModulationFm, modulation);
            Assert.Equal(-50.5f, referenceRxPowerDbm);
            Assert.Equal(10.0f, txPowerDbm);
            Assert.Equal(AsvAudioCodec.AsvAudioCodecOpus, codec);
            Assert.Equal(1, codecConfig);
        }

        [Fact]
        public void SetAndGetModulation_CorrectlyProcessesModulations()
        {
            var payload = new AsvRadioCapabilitiesResponsePayload();
            var modulations = new[] { AsvRadioModulation.AsvRadioModulationAm, AsvRadioModulation.AsvRadioModulationFm };

            AsvRadioHelper.SetModulation(payload, modulations);
            var result = AsvRadioHelper.GetModulation(payload).ToImmutableArray();

            Assert.Contains(AsvRadioModulation.AsvRadioModulationAm, result);
            Assert.Contains(AsvRadioModulation.AsvRadioModulationFm, result);
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void SetModulation_ThrowsException_WhenModulationOutOfRange()
        {
            var payload = new AsvRadioCapabilitiesResponsePayload();
            var modulations = new[] { (AsvRadioModulation)999 }; // Assuming 999 is out of range

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => AsvRadioHelper.SetModulation(payload, modulations));
            Assert.Contains("out of range", exception.Message);
        }
        
        [Fact]
        public void SetAndGetModulation_ComplexScenario_CorrectlyProcessesModulations()
        {
            var payload = new AsvRadioCapabilitiesResponsePayload();
            var modulations = new[] { AsvRadioModulation.AsvRadioModulationAm, AsvRadioModulation.AsvRadioModulationFm};

            AsvRadioHelper.SetModulation(payload, modulations);
            var retrievedModulations = AsvRadioHelper.GetModulation(payload).ToImmutableArray();

            Assert.Equal(modulations.Length, retrievedModulations.Length);
            foreach (var modulation in modulations)
            {
                Assert.Contains(modulation, retrievedModulations);
            }
        }

        [Fact]
        public void SetAndGetCodecs_ComplexScenario_CorrectlyProcessesCodecs()
        {
            var payload = new AsvRadioCapabilitiesResponsePayload();
            var codecs = new[] { AsvAudioCodec.AsvAudioCodecRaw, AsvAudioCodec.AsvAudioCodecOpus, AsvAudioCodec.AsvAudioCodecAac, AsvAudioCodec.AsvAudioCodecPcmu };

            AsvRadioHelper.SetCodecs(payload, codecs);
            var retrievedCodecs = AsvRadioHelper.GetCodecs(payload).ToImmutableArray();

            Assert.Equal(codecs.Length, retrievedCodecs.Length);
            foreach (var codec in codecs)
            {
                Assert.Contains(codec, retrievedCodecs);
            }
        }

        [Fact]
        public void SetAndGetCodecsOptions_ComplexScenario_CheckEquality()
        {
            var payload = new AsvRadioCodecCfgResponsePayload();
            var codecOptions = new byte[] { 1, 3, 5, 8, 15 };

            AsvRadioHelper.SetCodecsOptions(payload, codecOptions);
            var retrievedCodecOptions = AsvRadioHelper.GetCodecsOptions(payload).ToImmutableArray();

            Assert.Equal(codecOptions.Length, retrievedCodecOptions.Length);
            Assert.True(codecOptions.SequenceEqual(retrievedCodecOptions));
        }

    }