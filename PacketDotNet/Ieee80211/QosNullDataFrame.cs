/*
This file is part of PacketDotNet

PacketDotNet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PacketDotNet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with PacketDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 * Copyright 2012 Alan Rushforth <alan.rushforth@gmail.com>
 */

using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;

namespace PacketDotNet.Ieee80211
{
    /// <summary>
    /// The Qos null data frame serves the same purpose as <see cref="NullDataFrame" /> but also includes a
    /// quality of service control field.
    /// </summary>
    public sealed class QosNullDataFrame : DataFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QosNullDataFrame" /> class.
        /// </summary>
        /// <param name="byteArraySegment">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        public QosNullDataFrame(ByteArraySegment byteArraySegment)
        {
            Header = new ByteArraySegment(byteArraySegment);

            FrameControl = new FrameControlField(FrameControlBytes);
            Duration = new DurationField(DurationBytes);
            SequenceControl = new SequenceControlField(SequenceControlBytes);
            QosControl = QosControlBytes;
            ReadAddresses();

            Header.Length = FrameSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QosNullDataFrame" /> class.
        /// </summary>
        public QosNullDataFrame()
        {
            FrameControl = new FrameControlField();
            Duration = new DurationField();
            SequenceControl = new SequenceControlField();

            AssignDefaultAddresses();

            FrameControl.SubType = FrameControlField.FrameSubTypes.QosNullData;
        }

        /// <summary>
        /// Length of the frame header.
        /// This does not include the FCS, it represents only the header bytes that would
        /// would proceed any payload.
        /// </summary>
        public override int FrameSize
        {
            get
            {
                //if we are in WDS mode then there are 4 addresses (normally it is just 3)
                var numOfAddressFields = FrameControl.ToDS && FrameControl.FromDS ? 4 : 3;

                return MacFields.FrameControlLength +
                       MacFields.DurationIDLength +
                       (MacFields.AddressLength * numOfAddressFields) +
                       MacFields.SequenceControlLength +
                       QosNullDataFrameFields.QosControlLength;
            }
        }

        /// <summary>
        /// Gets or sets the qos control field.
        /// </summary>
        /// <value>
        /// The qos control field.
        /// </value>
        public ushort QosControl { get; set; }

        private ushort QosControlBytes
        {
            get
            {
                if (Header.Length >= QosNullDataFrameFields.QosControlPosition + QosNullDataFrameFields.QosControlLength)
                {
                    return EndianBitConverter.Little.ToUInt16(Header.Bytes,
                                                              Header.Offset + QosNullDataFrameFields.QosControlPosition);
                }

                return 0;
            }
            set => EndianBitConverter.Little.CopyBytes(value,
                                                       Header.Bytes,
                                                       Header.Offset + QosNullDataFrameFields.QosControlPosition);
        }

        /// <summary>
        /// Writes the current packet properties to the backing ByteArraySegment.
        /// </summary>
        public override void UpdateCalculatedValues()
        {
            if (Header == null || Header.Length > Header.BytesLength - Header.Offset || Header.Length < FrameSize)
            {
                Header = new ByteArraySegment(new byte[FrameSize]);
            }

            FrameControlBytes = FrameControl.Field;
            DurationBytes = Duration.Field;
            SequenceControlBytes = SequenceControl.Field;
            QosControlBytes = QosControl;
            WriteAddressBytes();
        }
    }
}