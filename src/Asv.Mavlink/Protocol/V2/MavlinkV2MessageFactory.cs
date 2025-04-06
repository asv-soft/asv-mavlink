using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

public class MavlinkV2MessageFactory : IProtocolMessageFactory<MavlinkMessage, int>
{
    private readonly ImmutableDictionary<int,Func<MavlinkMessage>> _factory;
    public static MavlinkV2MessageFactory Instance { get; } = new();

    private MavlinkV2MessageFactory()
    {
        var builder = ImmutableDictionary.CreateBuilder<int, Func<MavlinkMessage>>();
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
        _factory = builder.ToImmutable();
    }

    
    public MavlinkMessage? Create(int id) => _factory.TryGetValue(id, out var factory) ? factory() : null;

    public ProtocolInfo Info => MavlinkV2Protocol.Info;
    
    public IEnumerable<int> GetSupportedIds() => _factory.Keys.Select(x=>x);
}