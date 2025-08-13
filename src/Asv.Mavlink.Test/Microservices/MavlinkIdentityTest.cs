using System;
using Asv.Mavlink;
using FluentAssertions;
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
        identity.SystemId.Should().Be(systemId);
        identity.ComponentId.Should().Be(componentId);
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
        identity.SystemId.Should().Be(systemId);
        identity.ComponentId.Should().Be(componentId);
        identity.FullId.Should().Be(fullId);
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(123, 45);
    
        // Act & Assert
        identity1.Should().Be(identity2);
        identity1.Equals(identity2).Should().BeTrue();
        identity1.GetHashCode().Should().Be(identity2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(100, 50);
    
        // Act & Assert
        identity1.Should().NotBe(identity2);
        identity1.Equals(identity2).Should().BeFalse();
    }

    [Fact]
    public void EqualityOperators_ShouldWorkCorrectly()
    {
        // Arrange
        var identity1 = new MavlinkIdentity(123, 45);
        var identity2 = new MavlinkIdentity(123, 45);
        var identity3 = new MavlinkIdentity(100, 50);
    
        // Act & Assert
        (identity1 == identity2).Should().BeTrue();
        (identity1 != identity3).Should().BeTrue();
        (identity1 != identity2).Should().BeFalse();
        (identity1 == identity3).Should().BeFalse();
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var identity = new MavlinkIdentity(123, 45);
    
        // Act
        var result = identity.ToString();
    
        // Assert
        result.Should().Be("[123.45]");
    }

    [Fact]
    public void Parse_WithValidInput_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("[123.45]");
    
        // Assert
        result.Should().Be(expected);
        result.SystemId.Should().Be(123);
        result.ComponentId.Should().Be(45);
    }

    [Fact]
    public void Parse_WithoutBrackets_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("123.45");
    
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Parse_WithWhitespace_ShouldReturnCorrectIdentity()
    {
        // Arrange
        var expected = new MavlinkIdentity(123, 45);
    
        // Act
        var result = MavlinkIdentity.Parse("  [123.45]  ");
    
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Parse_WithNullOrEmpty_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var parseNull = () => MavlinkIdentity.Parse(null);
        parseNull.Should().Throw<ArgumentNullException>();
    
        var parseEmpty = () => MavlinkIdentity.Parse("");
        parseEmpty.Should().Throw<ArgumentNullException>();
    
        var parseWhitespace = () => MavlinkIdentity.Parse("   ");
        parseWhitespace.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Parse_WithInvalidFormat_ShouldThrowArgumentException()
    {
        // Act & Assert
        var parseInvalid = () => MavlinkIdentity.Parse("invalid");
        parseInvalid.Should().Throw<ArgumentException>();
    
        var parseInvalidSystemId = () => MavlinkIdentity.Parse("abc.123");
        parseInvalidSystemId.Should().Throw<ArgumentException>();
    
        var parseInvalidComponentId = () => MavlinkIdentity.Parse("123.abc");
        parseInvalidComponentId.Should().Throw<ArgumentException>();
    }
}