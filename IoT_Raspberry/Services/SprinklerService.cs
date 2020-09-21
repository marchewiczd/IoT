using IoT_Api.Database;
using IoT_RaspberryServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_RaspberryServer.Services
{
    public class SprinklerService
    {
        private LiteDb<Sprinkler> _liteDb;

        public SprinklerService()
        {
            this._liteDb = new LiteDb<Sprinkler>(AppSettings.LiteDbFilePath);
            this.Sprinklers = this._liteDb.GetAll();
        }

        public List<Sprinkler> Sprinklers { get; set; }

        public void AddSprinkler(Sprinkler sprinkler)
        {
            this._liteDb.Insert(sprinkler);
            this.Sprinklers = this._liteDb.GetAll();
        }

        public void ToggleSprinkler(Sprinkler sprinkler)
        {
            sprinkler.SprinkleStatus = sprinkler.SprinkleStatus ? false : true;
            this._liteDb.UpdateData(sprinkler);
        }

        public void ClearSprinklers()
        {
            this._liteDb.DeleteAll();
            this.Sprinklers = this._liteDb.GetAll();
        }

        public void DeleteSprinkler(Sprinkler sprinkler)
        {
            this._liteDb.Delete(sprinkler.Id);
            this.Sprinklers = this._liteDb.GetAll();
        }

        public void DeleteSprinkleTime(Sprinkler sprinkler, string key)
        {
            SprinklerDateTime elemToDelete = sprinkler.SprinkleTimeList.Find(x => x.ParsedDateTime == key);

            if(elemToDelete != null)
            {
                sprinkler.SprinkleTimeList.Remove(elemToDelete);
                this._liteDb.UpdateData(sprinkler);
                this.Sprinklers = this._liteDb.GetAll();
            }            
        }

        public void DeleteSprinkleTime(Sprinkler sprinkler, SprinklerDateTime entryToDelete)
        {
            sprinkler.SprinkleTimeList.Remove(entryToDelete);
            this._liteDb.UpdateData(sprinkler);
            this.Sprinklers = this._liteDb.GetAll();
        }

        public void AddSprinkleTime(Sprinkler sprinkler, DateTime dateTime, uint duration)
        {
            sprinkler.SprinkleTimeList.Add(new SprinklerDateTime
            {
                WateringDateTime = dateTime,
                WateringDuration = duration
            });

            this._liteDb.UpdateData(sprinkler);
            this.Sprinklers = this._liteDb.GetAll();
        }

        public void DeleteSprinkler(ulong id)
        {
            this._liteDb.Delete(id);
            this.Sprinklers = this._liteDb.GetAll();
        }
    }
}
