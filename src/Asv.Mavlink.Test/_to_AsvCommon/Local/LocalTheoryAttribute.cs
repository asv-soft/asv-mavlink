using Xunit;

namespace Asv.Mavlink.Test._to_AsvCommon;

public sealed class LocalTheoryAttribute : TheoryAttribute
{
    public LocalTheoryAttribute()
    {
        if (LocalAttributeHelper.IsRunInGithubActions())
        {
            Skip = LocalAttributeHelper.SkipMessage;
        }
    }
}