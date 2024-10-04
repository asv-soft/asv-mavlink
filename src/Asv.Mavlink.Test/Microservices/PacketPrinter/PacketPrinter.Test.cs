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
    public PacketPrinter_Test()
    {
        
    }

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
          
      });
      Assert.NotEqual(string.Empty,result);
    }
}