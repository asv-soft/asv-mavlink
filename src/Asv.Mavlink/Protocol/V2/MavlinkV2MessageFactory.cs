using System;
using System.Collections.Immutable;
using Asv.IO;
using Asv.Mavlink.Ardupilotmega;
using Asv.Mavlink.AsvAudio;
using Asv.Mavlink.AsvChart;
using Asv.Mavlink.AsvGbs;
using Asv.Mavlink.AsvRadio;
using Asv.Mavlink.AsvRfsa;
using Asv.Mavlink.AsvRsga;
using Asv.Mavlink.AsvSdr;
using Asv.Mavlink.Avssuas;
using Asv.Mavlink.Common;
using Asv.Mavlink.Csairlink;
using Asv.Mavlink.Cubepilot;
using Asv.Mavlink.Icarous;
using Asv.Mavlink.Minimal;
using Asv.Mavlink.Storm32;
using Asv.Mavlink.Ualberta;
using Asv.Mavlink.Uavionix;
using Asv.Mavlink.UnitTestMessage;

namespace Asv.Mavlink;

public partial class MavlinkV2MessageFactory : IProtocolMessageFactory<MavlinkMessage, ushort>
{
    private readonly ImmutableDictionary<ushort,Func<MavlinkMessage>> _decoder;
    public static MavlinkV2MessageFactory Instance { get; } = new();

    private MavlinkV2MessageFactory()
    {
        var builder = ImmutableDictionary.CreateBuilder<ushort, Func<MavlinkMessage>>();
        builder.RegisterMinimalDialect();
        builder.RegisterCommonDialect();
        builder.RegisterArdupilotmegaDialect();
        builder.RegisterIcarousDialect();
        builder.RegisterUalbertaDialect();
        builder.RegisterStorm32Dialect();
        builder.RegisterAvssuasDialect();
        builder.RegisterUavionixDialect();
        builder.RegisterCubepilotDialect();
        builder.RegisterCsairlinkDialect();
        builder.RegisterAsvGbsDialect();
        builder.RegisterAsvSdrDialect();
        builder.RegisterAsvAudioDialect();
        builder.RegisterAsvRadioDialect();
        builder.RegisterAsvRfsaDialect();
        builder.RegisterAsvChartDialect();
        builder.RegisterAsvRsgaDialect();
        builder.RegisterUnitTestMessageDialect();
        _decoder = builder.ToImmutable();
    }

    
    public MavlinkMessage? Create(ushort id) => _decoder.TryGetValue(id, out var factory) ? factory() : null;

    public ProtocolInfo Info => MavlinkV2Protocol.Info;
}