using System;

namespace IoT_RaspberryServer.Data
{
    public class SprinklerDateTime
    {
        private DateTime _wateringDateTime;

        public DateTime WateringDateTime 
        {
            get
            {
                return this._wateringDateTime;
            } 

            set
            {
                this._wateringDateTime = value;
                this.ParsedDateTime = value.ToString("HH:mm:ss");
            }
        }

        public string ParsedDateTime { get; private set; }

        public uint WateringDuration { get; set; }
    }
}
