# Packet code generation

Generate C# code for packet serialization\deserialization

> To test this command, you may need to use the [code gen example](code-gen-example.md) command first to generate basic files

You can run code generation in a console with this command:
```bash
Asv.Mavlink.Shell gen -t=[mavlink-xml-file] -i=[mavlink-xml-folder] -o=[output-folder] -e=cs -tmpl [path-to-liquid-template]/csharp.liquid
```

This command load XML file with mavlink packet definition

```xml
<message id="0" name="HEARTBEAT">
   <description>The heartbeat message shows that a system or component is present and responding. The type and autopilot fields (along with the message component id), allow the receiving system to treat further messages from this system appropriately (e.g. by laying out the user interface based on the autopilot). This microservice is documented at https://mavlink.io/en/services/heartbeat.html</description>
   <field type="uint8_t" name="type" enum="MAV_TYPE">Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.</field>
   <field type="uint8_t" name="autopilot" enum="MAV_AUTOPILOT">Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.</field>
   <field type="uint8_t" name="base_mode" enum="MAV_MODE_FLAG" display="bitmask">System mode bitmap.</field>
   <field type="uint32_t" name="custom_mode">A bitfield for use for autopilot-specific flags</field>
   <field type="uint8_t" name="system_status" enum="MAV_STATE">System status flag.</field>
   <field type="uint8_t_mavlink_version" name="mavlink_version">MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version</field>
</message>
```

And generate CSharp file, like this:

```c#
    /// <summary>
    ///  HEARTBEAT
    /// <summary>
    public class HeartbeatPayload : IPayload
    {
        public byte GetMaxByteSize() => 9; // Sum of byte sized of all fields (include extended)
        public byte GetMinByteSize() => 9; // of byte sized of fields (exclude extended)
        public int GetByteSize()
        {
            var sum = 0;
            sum+=4; //CustomMode
            sum+= 1; // Type
            sum+= 1; // Autopilot
            sum+= 1; // BaseMode
            sum+= 1; // SystemStatus
            sum+=1; //MavlinkVersion
            return (byte)sum;
        }
        public void Deserialize(ref ReadOnlySpan&#x3C;byte> buffer)
        {
            CustomMode = BinSerialize.ReadUInt(ref buffer);
            Type = (MavType)BinSerialize.ReadByte(ref buffer);
            Autopilot = (MavAutopilot)BinSerialize.ReadByte(ref buffer);
            BaseMode = (MavModeFlag)BinSerialize.ReadByte(ref buffer);
            SystemStatus = (MavState)BinSerialize.ReadByte(ref buffer);
            MavlinkVersion = (byte)BinSerialize.ReadByte(ref buffer);
        }

        public void Serialize(ref Span&#x3C;byte> buffer)
        {
            BinSerialize.WriteUInt(ref buffer,CustomMode);
            BinSerialize.WriteByte(ref buffer,(byte)Type);
            BinSerialize.WriteByte(ref buffer,(byte)Autopilot);
            BinSerialize.WriteByte(ref buffer,(byte)BaseMode);
            BinSerialize.WriteByte(ref buffer,(byte)SystemStatus);
            BinSerialize.WriteByte(ref buffer,(byte)MavlinkVersion);
            /* PayloadByteSize = 9 */;
        }
        /// <summary>
        /// A bitfield for use for autopilot-specific flags
        /// OriginName: custom_mode, Units: , IsExtended: false
        /// <summary>
        public uint CustomMode { get; set; }
        /// <summary>
        /// Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.
        /// OriginName: type, Units: , IsExtended: false
        /// <summary>
        public MavType Type { get; set; }
        /// <summary>
        /// Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.
        /// OriginName: autopilot, Units: , IsExtended: false
        /// <summary>
        public MavAutopilot Autopilot { get; set; }
        /// <summary>
        /// System mode bitmap.
        /// OriginName: base_mode, Units: , IsExtended: false
        /// <summary>
        public MavModeFlag BaseMode { get; set; }
        /// <summary>
        /// System status flag.
        /// OriginName: system_status, Units: , IsExtended: false
        /// <summary>
        public MavState SystemStatus { get; set; }
        /// <summary>
        /// MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version
        /// OriginName: mavlink_version, Units: , IsExtended: false
        /// <summary>
        public byte MavlinkVersion { get; set; }
    }
```