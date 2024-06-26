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
            AsvRadioHelper.SetArgsForRadioOn(command, 123456, AsvRadioModulation.AsvRadioModulationAm, -50.5f, 10.0f, AsvAudioCodec.AsvAudioCodecUnknown);

            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes(123456u)), command.Param1);
            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvRadioModulation.AsvRadioModulationAm)), command.Param2);
            Assert.Equal(-50.5f, command.Param3);
            Assert.Equal(10.0f, command.Param4);
            Assert.Equal(BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvAudioCodec.AsvAudioCodecUnknown)), command.Param5);
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
                Param5 = BitConverter.ToSingle(BitConverter.GetBytes((uint)AsvAudioCodec.AsvAudioCodecUnknown)),
                Param6 = BitConverter.ToSingle(BitConverter.GetBytes(1u)),
            };

            AsvRadioHelper.GetArgsForRadioOn(command, out var frequencyHz, out var modulation, out var referenceRxPowerDbm, out var txPowerDbm, out var codec);

            Assert.Equal(123456u, frequencyHz);
            Assert.Equal(AsvRadioModulation.AsvRadioModulationFm, modulation);
            Assert.Equal(-50.5f, referenceRxPowerDbm);
            Assert.Equal(10.0f, txPowerDbm);
            Assert.Equal(AsvAudioCodec.AsvAudioCodecUnknown, codec);
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

    }