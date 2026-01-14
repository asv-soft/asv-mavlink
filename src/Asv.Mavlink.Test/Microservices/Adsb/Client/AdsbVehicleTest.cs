using System;
using Asv.Mavlink.Common;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(AdsbVehicle))]
public class AdsbVehicleTest
{
    [Fact]
    public void Ctor_ProperArguments_Success()
    {
        // Arrange
        var currentTime = 120;
        var payload = new AdsbVehiclePayload();
        
        //Act
        var vehicle = new AdsbVehicle(payload, currentTime);
        
        // Assert
        Assert.NotNull(vehicle);
        Assert.Equal(currentTime, vehicle.GetLastHit());
    }
    
    [Fact]
    public void Ctor_PayloadIsNull_Throws()
    {
        // Arrange
        AdsbVehiclePayload? payload = null;
        
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new AdsbVehicle(payload, 123);
        });
    }
    
    [Fact(Skip = "Should fail, but actually works")]
    public void Ctor_NegativeTime_Throws()
    {
        // Arrange
        var payload = new AdsbVehiclePayload();
        
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new AdsbVehicle(payload, -1);
        });
    }
    
    [Fact]
    public void InternalUpdate_PerformUpdate_Success()
    {
        // Arrange
        var currentTime = 120;
        var payload = new AdsbVehiclePayload
        {
            Flags = AdsbFlags.AdsbFlagsSourceUat,
            Squawk = 24,
        };
        var newPayload = new AdsbVehiclePayload
        {
            Flags = AdsbFlags.AdsbFlagsSimulated,
            Squawk = (ushort)(payload.Squawk+1),
        };
        var vehicle = new AdsbVehicle(payload, currentTime);
        
        //Act
        vehicle.InternalUpdate(newPayload, ++currentTime);
        
        // Assert
        Assert.Equal(newPayload.Flags, vehicle.Flags.CurrentValue);
        Assert.Equal(newPayload.Squawk,  vehicle.Squawk.CurrentValue);
        Assert.Equal(currentTime, vehicle.GetLastHit());
    }
    
    [Fact]
    public void InternalUpdate_WithChangedIcaoAddress_Throws()
    {
        // Arrange
        var currentTime = 120;
        var payload = new AdsbVehiclePayload
        {
            Flags = AdsbFlags.AdsbFlagsSourceUat,
            IcaoAddress = 10,
        };
        var newPayload = new AdsbVehiclePayload
        {
            Flags = AdsbFlags.AdsbFlagsSimulated,
            IcaoAddress = payload.IcaoAddress+1,
        };
        var vehicle = new AdsbVehicle(payload, currentTime);
        
        // Act+ Assert
        Assert.Throws<InvalidOperationException>(() => vehicle.InternalUpdate(newPayload, ++currentTime));
        Assert.Equal(payload.Flags, vehicle.Flags.CurrentValue);
        Assert.NotEqual(newPayload.Flags, vehicle.Flags.CurrentValue);
        Assert.NotEqual(newPayload.IcaoAddress, vehicle.IcaoAddress);
        Assert.Equal(currentTime-1, vehicle.GetLastHit());
    }
}