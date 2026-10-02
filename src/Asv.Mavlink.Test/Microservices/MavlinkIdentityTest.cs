using System;
using Asv.Mavlink;
using JetBrains.Annotations;
using Xunit;

namespace Asv.Mavlink.Test;

[TestSubject(typeof(MavlinkIdentity))]
public class MavlinkIdentityTest
{

    [Fact]
    public void Constructor_WithByteParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        byte systemId = 123;
        byte componentId = 45;
    
        // Act
        var identity = new MavlinkIdentity(systemId, componentId);
    
        // Assert
        Assert.Equal(systemId, identity.SystemId);
        Assert.Equal(componentId, identity.ComponentId);
    }

    [Fact]
    public void Constructor_WithUshortParameter_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        byte systemId = 123;
        byte componentId = 45;
        var fullId = MavlinkHelper.ConvertToFullId(componentId, systemId);
    
        // Act
        var identity = new MavlinkIdentity(fullId);
    
        // Assert
        Assert.Equal(systemId, identity.SystemId);
        Assert.Equal(componentId, identity.ComponentId);
        Assert.Equal(fullId, identity.FullId);
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(123, 45);
    
        // Act & Assert
        Assert.Equal(identity2, identity1);
        Assert.True(identity1.Equals(identity2));
        Assert.Equal(identity2.GetHashCode(), identity1.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(100, 50);
    
        // Act & Assert
        Assert.NotEqual(identity2, identity1);
        Assert.False(identity1.Equals(identity2));
    }

    [Fact]
    public void EqualityOperators_ShouldWorkCorrectly()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(123, 45);
        var identity3 = new MavlinkIdentity(100, 50);
    
        // Act & Assert
        Assert.True(identity1 == identity2);
        Assert.True(identity1 != identity3);
        Assert.False(identity1 != identity2);
        Assert.False(identity1 == identity3);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var identity = new MavlinkIdentity(123, 45);
    
        // Act
        var result = identity.ToString();
    
        // Assert
        Assert.Equal("[123.45]", result);
    }

    [Fact]
    public void Parse_WithValidInput_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("[123.45]");
    
        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(123, result.SystemId);
        Assert.Equal(45, result.ComponentId);
    }

    [Fact]
    public void Parse_WithoutBrackets_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("123.45");
    
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Parse_WithWhitespace_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("  [123.45]  ");
    
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Parse_WithNullOrEmpty_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var parseNull = () => MavlinkIdentity.Parse(null);
        Assert.Throws<ArgumentNullException>(parseNull);
    
        var parseEmpty = () => MavlinkIdentity.Parse("");
        Assert.Throws<ArgumentNullException>(parseEmpty);
    
        var parseWhitespace = () => MavlinkIdentity.Parse("   ");
        Assert.Throws<ArgumentNullException>(parseWhitespace);
    }

    [Fact]
    public void Parse_WithInvalidFormat_ShouldThrowArgumentException()
    {
        // Act & Assert
        var parseInvalid = () => MavlinkIdentity.Parse("invalid");
        Assert.Throws<ArgumentException>(parseInvalid);
    
        var parseInvalidSystemId = () => MavlinkIdentity.Parse("abc.123");
        Assert.Throws<ArgumentException>(parseInvalidSystemId);
    
        var parseInvalidComponentId = () => MavlinkIdentity.Parse("123.abc");
        Assert.Throws<ArgumentException>(parseInvalidComponentId);
    }
}
