using System;

namespace IoT_RaspberryServer.Data
{
    public class SprinkleStatusData
    {
        public DateTime LastSuccesfulSprinkle { get; set; }

        public DateTime LastPlannedSprinkle { get; set; }

        public uint LastSuccesfulSprinkleDuration { get; set; }

        public uint LastPlannedSprinkleDuration { get; set; }
    }
}
