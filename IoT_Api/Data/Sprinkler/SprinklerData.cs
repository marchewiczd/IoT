using System;
using System.Collections.Generic;

namespace IoT_RaspberryServer.Data
{
    public class SprinklerData
    {
        private bool _sprinkleStatus;


        public SprinklerData(int gpioPin)
        {
            this.Id = Convert.ToUInt64(DateTime.Now.ToString("yyMMddHHmmssff"));
            this.SprinkleTimeList = new List<SprinklerDateTime>();
            this.SkipNextSprinkle = false;
            this.SprinkleStatus = false;
            this.GpioPin = gpioPin;
        }

        public SprinklerData(ulong id, int gpioPin)
        {
            this.Id = id;
            this.SprinkleTimeList = new List<SprinklerDateTime>();
            this.SkipNextSprinkle = false;
            this.SprinkleStatus = false;
            this.GpioPin = gpioPin;
        }

        public ulong Id { get; private set; }

        public int GpioPin { get; private set; }

        public string Description { get; set; }

        public List<SprinklerDateTime> SprinkleTimeList { get; set; }

        public DateTime LastSuccessfulSprinkle { get; set; }

        public bool SkipNextSprinkle { get; set; }

        public bool SprinkleStatus
        {
            get => this._sprinkleStatus;
            set => this._sprinkleStatus = value;
        }
    }
}
