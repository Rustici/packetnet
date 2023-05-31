namespace PacketDotNet.Ieee80211;

    public struct AssociationRequestFields
    {
        public static readonly int CapabilityInformationLength = 2;
        public static readonly int CapabilityInformationPosition;
        public static readonly int InformationElement1Position;
        public static readonly int ListenIntervalLength = 2;
        public static readonly int ListenIntervalPosition;

        static AssociationRequestFields()
        {
            CapabilityInformationPosition = MacFields.SequenceControlPosition + MacFields.SequenceControlLength;
            ListenIntervalPosition = CapabilityInformationPosition + CapabilityInformationLength;
            InformationElement1Position = ListenIntervalPosition + ListenIntervalLength;
        }
    }