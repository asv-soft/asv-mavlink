using Xunit;

namespace Asv.Mavlink.Test._to_AsvCommon;

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