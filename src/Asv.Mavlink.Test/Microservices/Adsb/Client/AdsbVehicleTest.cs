using System;
using Asv.Mavlink.V2.Common;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AdsbVehicle))]
public class AdsbVehicleTest
{

    [Fact]
    public void Ctor_ThrowsNullException_ArgIsNull()
    {
        AdsbVehiclePayload? payload = null;
        Assert.Throws<ArgumentNullException>(() =>
        {
            var vehicle = new AdsbVehicle(payload, 123);
        });
    }
}