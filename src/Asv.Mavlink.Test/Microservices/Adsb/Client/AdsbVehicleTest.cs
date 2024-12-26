using System;
using Asv.Mavlink.Common;
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
#pragma warning disable CS8604 // Possible null reference argument.
            var vehicle = new AdsbVehicle(payload, 123);
#pragma warning restore CS8604 // Possible null reference argument.
        });
    }
}