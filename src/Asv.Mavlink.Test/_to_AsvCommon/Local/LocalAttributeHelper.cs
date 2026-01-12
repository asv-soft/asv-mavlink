using System;

namespace Asv.Mavlink.Test;

public static class LocalAttributeHelper
{
    public const string SkipMessage = "This test is skipped in CI/CD environment because it's unstable.";
    
    public static bool IsRunInGithubActions()
    {
        return string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase)
               || string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true",
                   StringComparison.OrdinalIgnoreCase);
    }
}