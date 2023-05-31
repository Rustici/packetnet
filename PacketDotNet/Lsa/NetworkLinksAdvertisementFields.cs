namespace PacketDotNet.Lsa;

    /// <summary>
    /// Represents the length (in bytes) and the relative position
    /// of the fields in a Network-LSA
    /// </summary>
    public struct NetworkLinksAdvertisementFields
    {
        /// <summary>The length of the AttachedRouter field in bytes</summary>
        public static readonly int AttachedRouterLength = 4;

        /// <summary>The relative position of the AttachedRouter field</summary>
        public static readonly int AttachedRouterPosition;

        /// <summary>The length of the NetworkMask field in bytes</summary>
        public static readonly int NetworkMaskLength = 4;

        /// <summary>The relative position of the NetworkMask field</summary>
        public static readonly int NetworkMaskPosition;

        static NetworkLinksAdvertisementFields()
        {
            NetworkMaskPosition = LinkStateFields.HeaderEnd;
            AttachedRouterPosition = NetworkMaskPosition + NetworkMaskLength;
        }
    }