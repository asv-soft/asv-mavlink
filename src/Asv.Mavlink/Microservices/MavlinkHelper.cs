using System;

using System.Threading;
using System.Threading.Tasks;
using R3;

namespace Asv.Mavlink
{
    public static class MavlinkHelper
    {
        

      
        
        public static ushort ConvertToFullId(byte componentId, byte systemId) => (ushort)(componentId | systemId << 8);

        public static void ConvertFromId(ushort fullId, out byte componentId, out byte systemId)
        {
            componentId = (byte)(fullId & 0xFF);
            systemId = (byte)(fullId >> 8);
        }
        
    }
}
