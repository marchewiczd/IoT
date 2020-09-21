using System;
using System.Collections.Generic;

namespace IoT_RaspberryServer.Data
{
    public class Sprinkler
    {
        public Sprinkler()
        {
            this.Id = Convert.ToUInt64(DateTime.Now.ToString("yyMMddHHmmssff"));
            this.SprinkleTimeList = new List<SprinklerDateTime>();
            this.SkipNextSprinkle = false;
            this.SprinkleStatus = false;
        }

        public ulong Id { get; private set; }

        public int GpioPin { get; set; }

        public string Description { get; set; }

        public List<SprinklerDateTime> SprinkleTimeList { get; set; }

        public DateTime LastSuccessfulSprinkle { get; set; }

        public bool SkipNextSprinkle { get; set; }

        public bool SprinkleStatus { get; set; }

    }
}
