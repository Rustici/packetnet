using System;
using System.Collections.Generic;
using System.Text;
using PacketDotNet.Lsa;
using PacketDotNet.Utils;

namespace PacketDotNet;

    /// <summary>
    /// Link State Request packets are OSPF packet type 3.
    /// The Link State Request packet is used to request the pieces of the
    /// neighbor's database that are more up-to-date.
    /// See http://www.ietf.org/rfc/rfc2328.txt for details.
    /// </summary>
    public sealed class OspfV2LinkStateRequestPacket : OspfV2Packet
    {
        /// <value>
        /// The packet type
        /// </value>
        public static OspfPacketType PacketType = OspfPacketType.LinkStateRequest;

        /// <summary>
        /// Constructs an OSPFv2 LSR packet
        /// </summary>
        public OspfV2LinkStateRequestPacket()
        {
            Type = PacketType;
            PacketLength = (ushort) Header.Bytes.Length;
        }

        /// <summary>
        /// Constructs an OSPFv2 LSR packet with link state requests
        /// </summary>
        /// <param name="linkStateRequests">List of the link state requests</param>
        public OspfV2LinkStateRequestPacket(List<LinkStateRequest> linkStateRequests)
        {
            var length = linkStateRequests.Count * LinkStateRequest.Length;
            var offset = OspfV2Fields.HeaderLength;
            var bytes = new byte[length + OspfV2Fields.HeaderLength];

            Array.Copy(Header.Bytes, bytes, Header.Length);
            foreach (var t in linkStateRequests)
            {
                Array.Copy(t.Bytes, 0, bytes, offset, LinkStateRequest.Length);
                offset += LinkStateRequest.Length;
            }

            Header = new ByteArraySegment(bytes);
            Type = PacketType;
            PacketLength = (ushort) Header.Bytes.Length;
        }

        /// <summary>
        /// Constructs an OSPFv2 LSR packet from ByteArraySegment
        /// </summary>
        /// <param name="byteArraySegment">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        public OspfV2LinkStateRequestPacket(ByteArraySegment byteArraySegment)
        {
            Header = new ByteArraySegment(byteArraySegment.Bytes);
        }

        /// <summary>
        /// Constructs a packet from bytes and offset
        /// </summary>
        /// <param name="bytes">
        /// A <see cref="byte" />
        /// </param>
        /// <param name="offset">
        /// A <see cref="int" />
        /// </param>
        public OspfV2LinkStateRequestPacket(byte[] bytes, int offset) :
            base(bytes, offset)
        {
            Type = PacketType;
        }

        /// <summary>
        /// A list of link state requests, contained in this packet
        /// </summary>
        /// See
        /// <see cref="LinkStateRequest" />
        public List<LinkStateRequest> Requests
        {
            get
            {
                var bytesNeeded = PacketLength - OspfV2Fields.LSRStart;
                if (bytesNeeded % LinkStateRequest.Length != 0)
                {
                    throw new Exception("Malformed LSR packet - bad size for the LS requests");
                }

                var ret = new List<LinkStateRequest>();
                var offset = Header.Offset + OspfV2Fields.LSRStart;
                var lsrCount = bytesNeeded / LinkStateRequest.Length;

                for (var i = 0; i < lsrCount; i++)
                {
                    var request = new LinkStateRequest(Header.Bytes, offset, LinkStateRequest.Length);
                    ret.Add(request);
                    offset += LinkStateRequest.Length;
                }

                return ret;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current <see cref="OspfV2LinkStateRequestPacket" />.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the current <see cref="OspfV2LinkStateRequestPacket" />.</returns>
        public override string ToString()
        {
            var packet = new StringBuilder();
            packet.Append(base.ToString());
            packet.Append(" ");
            packet.AppendFormat("LSR count: {0} ", Requests.Count);
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