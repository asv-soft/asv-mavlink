using System.Collections.Generic;
using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Mavlink.Test.ParamsExt.Client.Ex;

[TestSubject(typeof(ParamsExtClientEx))]
public class ParamsExtClientExTest(ITestOutputHelper log) : ClientTestBase<ParamsExtClientEx>(log)
{
    private readonly ParamsExtClientExConfig _config = new()
    {
        ReadAttemptCount = 5,
        ReadTimeouMs = 500,
        ReadListTimeoutMs = 100,
        ChunkUpdateBufferMs = 100
    };

    private IEnumerable<ParamExtDescription> ParamDescription { get; } = [];
    
    protected override ParamsExtClientEx CreateClient(MavlinkClientIdentity identity, CoreServices core) 
        => new(new ParamsExtClient(identity,_config, core), _config, ParamDescription);

    
}