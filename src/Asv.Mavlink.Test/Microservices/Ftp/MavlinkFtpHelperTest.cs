using Xunit;

namespace Asv.Mavlink.Test;

public class MavlinkFtpHelperTest
{
    [Theory]
    [InlineData("/", "/", "", "")]
    [InlineData("/logs", "/logs/", "/", "logs")]
    [InlineData("/logs/", "/logs/", "/", "logs")]
    [InlineData("logs", "logs/", "", "logs")]
    [InlineData("logs/", "logs/", "", "logs")]
    [InlineData("@SYS", "@SYS/", "", "@SYS")]
    [InlineData("/a/b", "/a/b/", "/a/", "b")]
    [InlineData("a/b/", "a/b/", "a/", "b")]
    public void CreateFtpDirectoryFromPath_AnyPathForm_ReturnsCanonicalDirectory(
        string path,
        string expectedPath,
        string expectedParentPath,
        string expectedName
    )
    {
        // Act
        var entry = MavlinkFtpHelper.CreateFtpDirectoryFromPath(path);

        // Assert
        Assert.Equal(FtpEntryType.Directory, entry.Type);
        Assert.Equal(expectedPath, entry.Path);
        Assert.Equal(expectedParentPath, entry.ParentPath);
        Assert.Equal(expectedName, entry.Name);
    }
}
