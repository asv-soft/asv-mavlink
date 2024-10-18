using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using Asv.Mavlink.V2.Common;
using Asv.Mavlink.V2.Minimal;
using Xunit;

namespace Asv.Mavlink.Test;

public class PacketPrinter_Test
{
    private PacketPrinter printer;
    private Collection<IPacketPrinterHandler> handlers= new ();
    
    [Fact]
    private void CommonPacketHandlerTest()
    {
        var handler = new ParamValuePacketHandler();
        handlers.Add(handler);
        printer = new PacketPrinter(handlers);
      var result =   printer.Print(new ParamValuePacket()
      {
          ComponentId = 1,
          SystemId = 2, 
          Payload =
          {
                ParamId = "test".ToCharArray(),
                ParamType = MavParamType.MavParamTypeInt8,
                ParamValue = 1.0f,
                ParamCount = 1,
                ParamIndex = 1
          }
      });
      Assert.NotEqual(string.Empty,result);
    }
}