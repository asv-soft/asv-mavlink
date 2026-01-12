using Xunit;

namespace Asv.Mavlink.Test;

/// <summary>
/// A custom attribute derived from <see cref="Xunit.FactAttribute"/> that is used to conditionally
/// skip test execution based on the environment. Specifically, it skips tests when they are
/// running in a Continuous Integration (CI) system such as GitHub Actions.
/// </summary>
/// <remarks>
/// This attribute is useful for marking tests that are designed to run only locally or under
/// specific conditions, as opposed to environments where instability is expected, like CI/CD pipelines.
/// <para>!!!Note!!!: Currently, this implementation specifically checks for the <c>GITHUB_ACTIONS</c> 
/// environment variable, making it effective primarily within GitHub Actions pipelines.</para>
/// </remarks>
public sealed class LocalFactAttribute : FactAttribute
{
    public LocalFactAttribute()
    {
        if (LocalAttributeHelper.IsRunInGithubActions())
        {
            Skip = LocalAttributeHelper.SkipMessage;
        }
    }
}