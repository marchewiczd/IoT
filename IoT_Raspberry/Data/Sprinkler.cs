using System;
using System.Collections.Generic;

namespace IoT_RaspberryServer.Data
{
    public class Sprinkler
    {
        public Sprinkler()
        {
            this.Id = Convert.ToUInt64(DateTime.Now.ToString("yyMMddHHmmssff"));
            this.SprinkleTimeDict = new Dictionary<DateTime, uint>();
            this.SkipNextSprinkle = false;
            this.SprinkleStatus = false;
        }

        public ulong Id { get; private set; }

        public int GpioPin { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// datetime: sprinkle time
        /// uint: sprinkle length
        /// </summary>
        public Dictionary<DateTime, uint> SprinkleTimeDict { get; set; }

        public DateTime LastSuccessfulSprinkle { get; set; }

        public bool SkipNextSprinkle { get; set; }

        public bool SprinkleStatus { get; set; }

    }
}
