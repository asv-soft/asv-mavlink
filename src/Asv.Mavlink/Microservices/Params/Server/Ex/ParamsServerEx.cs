using Asv.Common;

namespace Asv.Mavlink;

public interface IParamsServerEx
{
    
}

public class ParamsServerEx: DisposableOnceWithCancel, IParamsServerEx
{
    public ParamsServerEx(IParamsServer server)
    {
        
    }
}