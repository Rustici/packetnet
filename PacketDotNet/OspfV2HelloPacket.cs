using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PacketDotNet.Utils;
using PacketDotNet.Utils.Converters;

namespace PacketDotNet;

    /// <summary>
    /// Hello packets are OSPF packet type 1.  These packets are sent
    /// periodically on all interfaces (including virtual links) in order to
    /// establish and maintain neighbor relationships.
    /// See http://www.ietf.org/rfc/rfc2328.txt for details.
    /// </summary>
    public sealed class OspfV2HelloPacket : OspfV2Packet
    {
        /// <value>
        /// The packet type
        /// </value>
        public static OspfPacketType PacketType = OspfPacketType.Hello;

        /// <summary>
        /// Constructs an OSPFv2 Hello packet from ByteArraySegment
        /// </summary>
        /// <param name="byteArraySegment">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        public OspfV2HelloPacket(ByteArraySegment byteArraySegment)
        {
            Header = new ByteArraySegment(byteArraySegment.Bytes);
        }

        /// <summary>
        /// Constructs an OSPFv2 Hello packet from NetworkMask, HelloInterval and
        /// RouterDeadInterval.
        /// </summary>
        /// <param name="networkMask">The Network mask - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        /// <param name="helloInterval">The Hello interval - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        /// <param name="routerDeadInterval">The Router dead interval - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        public OspfV2HelloPacket
        (
            IPAddress networkMask,
            ushort helloInterval,
            ushort routerDeadInterval)
        {
            var b = new byte[OspfV2Fields.NeighborIDStart];
            Array.Copy(Header.Bytes, b, Header.Bytes.Length);
            Header = new ByteArraySegment(b, 0, OspfV2Fields.NeighborIDStart);
            Type = PacketType;

            NetworkMask = networkMask;
            HelloInterval = helloInterval;
            RouterDeadInterval = routerDeadInterval;
            PacketLength = (ushort) Header.Bytes.Length;
        }

        /// <summary>
        /// Constructs an OSPFv2 Hello packet from NetworkMask, HelloInterval and
        /// RouterDeadInterval and a list of neighbor routers.
        /// </summary>
        /// <param name="networkMask">The Network mask - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        /// <param name="helloInterval">The Hello interval - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        /// <param name="routerDeadInterval">The Router dead interval - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        /// <param name="neighbors">List of router neighbors - see http://www.ietf.org/rfc/rfc2328.txt for details.</param>
        public OspfV2HelloPacket
        (
            IPAddress networkMask,
            ushort helloInterval,
            ushort routerDeadInterval,
            IReadOnlyList<IPAddress> neighbors) : this(networkMask, helloInterval, routerDeadInterval)
        {
            var length = neighbors.Count * 4;
            var offset = OspfV2Fields.NeighborIDStart;
            var bytes = new byte[length + OspfV2Fields.NeighborIDStart];

            Array.Copy(Header.Bytes, bytes, Header.Length);
            foreach (var t in neighbors)
            {
                Array.Copy(t.GetAddressBytes(), 0, bytes, offset, 4); //4 bytes per address
                offset += 4;
            }

            Header = new ByteArraySegment(bytes);
            PacketLength = (ushort) Header.Bytes.Length;
        }

        /// <summary>
        /// Constructs an OSPFv2 Hello packet from given bytes and offset.
        /// </summary>
        /// <param name="bytes">
        /// A <see cref="byte" />
        /// </param>
        /// <param name="offset">
        /// A <see cref="int" />
        /// </param>
        public OspfV2HelloPacket(byte[] bytes, int offset) :
            base(bytes, offset)
        {
            Type = PacketType;
        }

        /// <summary>
        /// The identity of the Backup Designated Router for this network,
        /// in the view of the sending router.
        /// </summary>
        public IPAddress BackupRouterId
        {
            get
            {
                var val = EndianBitConverter.Little.ToUInt32(Header.Bytes, Header.Offset + OspfV2Fields.BackupRouterIDPosition);
                return new IPAddress(val);
            }
            set
            {
                var address = value.GetAddressBytes();
                Array.Copy(address,
                           0,
                           Header.Bytes,
                           Header.Offset + OspfV2Fields.BackupRouterIDPosition,
                           address.Length);
            }
        }

        /// <summary>
        /// The identity of the Designated Router for this network, in the
        /// view of the sending router.
        /// </summary>
        public IPAddress DesignatedRouterId
        {
            get
            {
                var val = EndianBitConverter.Little.ToUInt32(Header.Bytes, Header.Offset + OspfV2Fields.DesignatedRouterIDPosition);
                return new IPAddress(val);
            }
            set
            {
                var address = value.GetAddressBytes();
                Array.Copy(address,
                           0,
                           Header.Bytes,
                           Header.Offset + OspfV2Fields.DesignatedRouterIDPosition,
                           address.Length);
            }
        }

        /// <summary>
        /// The number of seconds between this router's Hello packets.
        /// </summary>
        public ushort HelloInterval
        {
            get => EndianBitConverter.Big.ToUInt16(Header.Bytes, Header.Offset + OspfV2Fields.HelloIntervalPosition);
            set => EndianBitConverter.Big.CopyBytes(value, Header.Bytes, Header.Offset + OspfV2Fields.HelloIntervalPosition);
        }

        /// <summary>
        /// The optional capabilities supported by the router. See http://www.ietf.org/rfc/rfc2328.txt for details.
        /// </summary>
        public byte HelloOptions
        {
            get => Header.Bytes[Header.Offset + OspfV2Fields.HelloOptionsPosition];
            set => Header.Bytes[Header.Offset + OspfV2Fields.HelloOptionsPosition] = value;
        }

        /// <summary>
        /// List of the Router IDs of each router from whom valid Hello packets have
        /// been seen recently on the network.  Recently means in the last
        /// RouterDeadInterval seconds. Can be zero or more.
        /// </summary>
        public List<IPAddress> NeighborIds
        {
            get
            {
                var ret = new List<IPAddress>();
                var bytesAvailable = PacketLength - OspfV2Fields.NeighborIDStart;

                if (bytesAvailable % 4 != 0)
                {
                    throw new Exception("malformed OSPFv2Hello Packet - bad NeighborIds size");
                }

                var offset = OspfV2Fields.NeighborIDStart;
                while (offset < PacketLength)
                {
                    long address = EndianBitConverter.Little.ToUInt32(Header.Bytes, Header.Offset + offset);
                    ret.Add(new IPAddress(address));
                    offset += 4;
                }

                return ret;
            }
        }

        /// <summary>
        /// The network mask associated with this interface.
        /// </summary>
        public IPAddress NetworkMask
        {
            get
            {
                var val = EndianBitConverter.Little.ToUInt32(Header.Bytes, Header.Offset + OspfV2Fields.NetworkMaskPosition);
                return new IPAddress(val);
            }
            set
            {
                var address = value.GetAddressBytes();
                Array.Copy(address,
                           0,
                           Header.Bytes,
                           Header.Offset + OspfV2Fields.NetworkMaskPosition,
                           address.Length);
            }
        }

        /// <summary>
        /// The number of seconds before declaring a silent router down.
        /// </summary>
        public uint RouterDeadInterval
        {
            get => EndianBitConverter.Big.ToUInt32(Header.Bytes, Header.Offset + OspfV2Fields.RouterDeadIntervalPosition);
            set => EndianBitConverter.Big.CopyBytes(value, Header.Bytes, Header.Offset + OspfV2Fields.RouterDeadIntervalPosition);
        }

        /// <summary>
        /// This router's Router Priority.
        /// </summary>
        public byte RtrPriority
        {
            get => Header.Bytes[Header.Offset + OspfV2Fields.RtrPriorityPosition];
            set => Header.Bytes[Header.Offset + OspfV2Fields.RtrPriorityPosition] = value;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current <see cref="OspfV2HelloPacket" />.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the current <see cref="OspfV2HelloPacket" />.</returns>
        public override string ToString()
        {
            var packet = new StringBuilder();
            packet.Append(base.ToString());
            packet.Append(" ");
            packet.AppendFormat("HelloOptions: {0} ", HelloOptions);
            packet.AppendFormat("RouterId: {0} ", RouterId);
            return packet.ToString();
        }

        /// <summary cref="Packet.ToString()">
        /// Output the packet information in the specified format
        /// Normal - outputs the packet info to a single line
        /// Colored - outputs the packet info to a single line with coloring
        /// Verbose - outputs detailed info about the packet
        /// VerboseColored - outputs detailed info about the packet with coloring
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="outputFormat">Output format.</param>
        public override string ToString(StringOutputType outputFormat)
        {
            return ToString();
        }
    }