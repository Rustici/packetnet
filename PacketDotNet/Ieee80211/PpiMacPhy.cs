using System.IO;

namespace PacketDotNet.Ieee80211;

    /// <summary>
    /// The 802.11n MAC + PHY Extension field contains radio information specific to 802.11n.
    /// </summary>
    public class PpiMacPhy : PpiFields
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PpiMacPhy" /> class from the
        /// provided stream.
        /// </summary>
        /// <remarks>
        /// The position of the BinaryReader's underlying stream will be advanced to the end
        /// of the PPI field.
        /// </remarks>
        /// <param name='br'>
        /// The stream the field will be read from
        /// </param>
        public PpiMacPhy(BinaryReader br)
        {
            AMpduId = br.ReadUInt32();
            DelimiterCount = br.ReadByte();
            ModulationCodingScheme = br.ReadByte();
            SpatialStreamCount = br.ReadByte();
            RssiCombined = br.ReadByte();
            RssiAntenna0Control = br.ReadByte();
            RssiAntenna1Control = br.ReadByte();
            RssiAntenna2Control = br.ReadByte();
            RssiAntenna3Control = br.ReadByte();
            RssiAntenna0Ext = br.ReadByte();
            RssiAntenna1Ext = br.ReadByte();
            RssiAntenna2Ext = br.ReadByte();
            RssiAntenna3Ext = br.ReadByte();
            ExtensionChannelFrequency = br.ReadUInt16();
            ExtensionChannelFlags = (RadioTapChannelFlags) br.ReadUInt16();
            DbmAntenna0SignalPower = br.ReadByte();
            DbmAntenna0SignalNoise = br.ReadByte();
            DbmAntenna1SignalPower = br.ReadByte();
            DbmAntenna1SignalNoise = br.ReadByte();
            DbmAntenna2SignalPower = br.ReadByte();
            DbmAntenna2SignalNoise = br.ReadByte();
            DbmAntenna3SignalPower = br.ReadByte();
            DbmAntenna3SignalNoise = br.ReadByte();
            ErrorVectorMagnitude0 = br.ReadUInt32();
            ErrorVectorMagnitude1 = br.ReadUInt32();
            ErrorVectorMagnitude2 = br.ReadUInt32();
            ErrorVectorMagnitude3 = br.ReadUInt32();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PpiMacPhy" /> class.
        /// </summary>
        public PpiMacPhy()
        { }

        /// <summary>
        /// Gets or sets the A-MPDU identifier.
        /// </summary>
        /// <value>
        /// the A-MPDU id.
        /// </value>
        public uint AMpduId { get; set; }

        /// <summary>
        /// Gets the field bytes. This doesn't include the PPI field header.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public override byte[] Bytes
        {
            get
            {
                var ms = new MemoryStream();
                var writer = new BinaryWriter(ms);

                writer.Write(AMpduId);
                writer.Write(DelimiterCount);
                writer.Write(ModulationCodingScheme);
                writer.Write(SpatialStreamCount);
                writer.Write(RssiCombined);
                writer.Write(RssiAntenna0Control);
                writer.Write(RssiAntenna1Control);
                writer.Write(RssiAntenna2Control);
                writer.Write(RssiAntenna3Control);
                writer.Write(RssiAntenna0Ext);
                writer.Write(RssiAntenna1Ext);
                writer.Write(RssiAntenna2Ext);
                writer.Write(RssiAntenna3Ext);
                writer.Write(ExtensionChannelFrequency);
                writer.Write((ushort) ExtensionChannelFlags);
                writer.Write(DbmAntenna0SignalPower);
                writer.Write(DbmAntenna0SignalNoise);
                writer.Write(DbmAntenna1SignalPower);
                writer.Write(DbmAntenna1SignalNoise);
                writer.Write(DbmAntenna2SignalPower);
                writer.Write(DbmAntenna2SignalNoise);
                writer.Write(DbmAntenna3SignalPower);
                writer.Write(DbmAntenna3SignalNoise);
                writer.Write(ErrorVectorMagnitude0);
                writer.Write(ErrorVectorMagnitude1);
                writer.Write(ErrorVectorMagnitude2);
                writer.Write(ErrorVectorMagnitude3);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the RF signal noise at antenna 0.
        /// </summary>
        /// <value>
        /// The signal noise.
        /// </value>
        public byte DbmAntenna0SignalNoise { get; set; }

        /// <summary>
        /// Gets or sets the RF signal power at antenna 0.
        /// </summary>
        /// <value>
        /// The signal power.
        /// </value>
        public byte DbmAntenna0SignalPower { get; set; }

        /// <summary>
        /// Gets or sets the RF signal noise at antenna 1.
        /// </summary>
        /// <value>
        /// The signal noise.
        /// </value>
        public byte DbmAntenna1SignalNoise { get; set; }

        /// <summary>
        /// Gets or sets the RF signal power at antenna 1.
        /// </summary>
        /// <value>
        /// The signal power.
        /// </value>
        public byte DbmAntenna1SignalPower { get; set; }

        /// <summary>
        /// Gets or sets the RF signal noise at antenna 2.
        /// </summary>
        /// <value>
        /// The signal noise.
        /// </value>
        public byte DbmAntenna2SignalNoise { get; set; }

        /// <summary>
        /// Gets or sets the RF signal power at antenna 2.
        /// </summary>
        /// <value>
        /// The signal power.
        /// </value>
        public byte DbmAntenna2SignalPower { get; set; }

        /// <summary>
        /// Gets or sets the RF signal noise at antenna 3.
        /// </summary>
        /// <value>
        /// The signal noise.
        /// </value>
        public byte DbmAntenna3SignalNoise { get; set; }

        /// <summary>
        /// Gets or sets the RF signal power at antenna 3.
        /// </summary>
        /// <value>
        /// The signal power.
        /// </value>
        public byte DbmAntenna3SignalPower { get; set; }

        /// <summary>
        /// Gets or sets the number of zero-length pad delimiters
        /// </summary>
        /// <value>
        /// The delimiter count.
        /// </value>
        public byte DelimiterCount { get; set; }

        /// <summary>
        /// Gets or sets the error vector magnitude for Chain 0.
        /// </summary>
        /// <value>
        /// The error vector magnitude.
        /// </value>
        public uint ErrorVectorMagnitude0 { get; set; }

        /// <summary>
        /// Gets or sets the error vector magnitude for Chain 1.
        /// </summary>
        /// <value>
        /// The error vector magnitude.
        /// </value>
        public uint ErrorVectorMagnitude1 { get; set; }

        /// <summary>
        /// Gets or sets the error vector magnitude for Chain 2.
        /// </summary>
        /// <value>
        /// The error vector magnitude.
        /// </value>
        public uint ErrorVectorMagnitude2 { get; set; }

        /// <summary>
        /// Gets or sets the error vector magnitude for Chain 3.
        /// </summary>
        /// <value>
        /// The error vector magnitude.
        /// </value>
        public uint ErrorVectorMagnitude3 { get; set; }

        /// <summary>
        /// Gets or sets the extension channel flags.
        /// </summary>
        /// <value>
        /// The extension channel flags.
        /// </value>
        public RadioTapChannelFlags ExtensionChannelFlags { get; set; }

        /// <summary>
        /// Gets or sets the extension channel frequency.
        /// </summary>
        /// <value>
        /// The extension channel frequency.
        /// </value>
        public ushort ExtensionChannelFrequency { get; set; }

        /// <summary>Type of the field</summary>
        public override PpiFieldType FieldType => PpiFieldType.PpiMacPhy;

        /// <summary>
        /// Gets or sets the 802.11n MAC extension flags.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        public PpiMacExtensionFlags Flags { get; set; }

        /// <summary>
        /// Gets the length of the field data.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public override int Length => 48;

        /// <summary>
        /// Gets or sets the modulation coding scheme.
        /// </summary>
        /// <value>
        /// The modulation coding scheme.
        /// </value>
        public byte ModulationCodingScheme { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 0, control channel.
        /// </summary>
        /// <value>
        /// The antenna 0 RSSI value.
        /// </value>
        public byte RssiAntenna0Control { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 0, extension channel
        /// </summary>
        /// <value>
        /// The antenna 0 extension channel RSSI value.
        /// </value>
        public byte RssiAntenna0Ext { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 1, control channel.
        /// </summary>
        /// <value>
        /// The antenna 1 control channel RSSI value.
        /// </value>
        public byte RssiAntenna1Control { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 1, extension channel
        /// </summary>
        /// <value>
        /// The antenna 1 extension channel RSSI value.
        /// </value>
        public byte RssiAntenna1Ext { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 2, control channel.
        /// </summary>
        /// <value>
        /// The antenna 2 control channel RSSI value.
        /// </value>
        public byte RssiAntenna2Control { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 2, extension channel
        /// </summary>
        /// <value>
        /// The antenna 2 extension channel RSSI value.
        /// </value>
        public byte RssiAntenna2Ext { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 3, control channel.
        /// </summary>
        /// <value>
        /// The antenna 3 control channel RSSI value.
        /// </value>
        public byte RssiAntenna3Control { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indication (RSSI) value for the antenna 3, extension channel
        /// </summary>
        /// <value>
        /// The antenna 3 extension channel RSSI value.
        /// </value>
        public byte RssiAntenna3Ext { get; set; }

        /// <summary>
        /// Gets or sets the combined Received Signal Strength Indication (RSSI) value
        /// from all the active antennas and channels.
        /// </summary>
        /// <value>
        /// The combined RSSI.
        /// </value>
        public byte RssiCombined { get; set; }

        /// <summary>
        /// Gets or sets the number of spatial streams.
        /// </summary>
        /// <value>
        /// The spatial stream count.
        /// </value>
        public byte SpatialStreamCount { get; set; }
    }