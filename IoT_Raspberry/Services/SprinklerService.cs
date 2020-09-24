using IoT_Api.Database;
using IoT_RaspberryControlApi.GPIO.Controllers;
using IoT_RaspberryServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT_RaspberryServer.Services
{
    public class SprinklerService
    {
        private LiteDb<SprinklerData> _liteDb;

        public SprinklerService()
        {
            this._liteDb = new LiteDb<SprinklerData>(AppSettings.LiteDbFilePath);
            this.Sprinklers = this._liteDb.GetAll();
            this.InitializeControllers();
        }

        public List<SprinklerData> Sprinklers { get; set; }

        public void InitializeControllers()
        {
            if (!this.Sprinklers.Any())
            {
                return;
            }
        }

        #region Sprinkle Modification

        public void AddSprinkler(SprinklerData sprinkler)
        {
            this._liteDb.Insert(sprinkler);
            this.Sprinklers.Add(sprinkler);
            PiActionController.AddSprinklerController(sprinkler.Id, new SprinklerController(sprinkler.GpioPin, sprinkler.SprinkleStatus));
        }

        public void ToggleSprinkler(SprinklerData sprinkler)
        {
            sprinkler.SprinkleStatus = sprinkler.SprinkleStatus ? false : true;
            this._liteDb.UpdateData(sprinkler);
            PiActionController.ToggleSprinklerController(sprinkler.Id);
        }

        public void ClearSprinklers()
        {
            this._liteDb.DeleteAll();
            this.Sprinklers.Clear();
            PiActionController.ClearSprinklerControllers();
        }

        public void DeleteSprinkler(SprinklerData sprinkler)
        {
            this._liteDb.Delete(sprinkler.Id);
            this.Sprinklers.Remove(sprinkler);
            PiActionController.DeleteSprinklerController(sprinkler.Id);
        }

        public void DeleteSprinkler(ulong id)
        {
            SprinklerData sprinkler = this.Sprinklers.Find(x => x.Id == id);
            this._liteDb.Delete(id);
            this.Sprinklers.Remove(sprinkler);
            PiActionController.DeleteSprinklerController(id);
        }

        #endregion

        #region Sprinkle Time Modification

        public void DeleteSprinkleTime(SprinklerData sprinkler, string key)
        {
            SprinklerDateTime elemToDelete = sprinkler.SprinkleTimeList.Find(x => x.ParsedDateTime == key);

            if(elemToDelete != null)
            {
                sprinkler.SprinkleTimeList.Remove(elemToDelete);
                this._liteDb.UpdateData(sprinkler);
                this.Sprinklers.Find(x => x.Id == sprinkler.Id).SprinkleTimeList.Remove(elemToDelete);
            }            
        }

        public void DeleteSprinkleTime(SprinklerData sprinkler, SprinklerDateTime entryToDelete)
        {
            sprinkler.SprinkleTimeList.Remove(entryToDelete);
            this._liteDb.UpdateData(sprinkler);
            this.Sprinklers.Find(x => x.Id == sprinkler.Id).SprinkleTimeList.Remove(entryToDelete);
        }

        public void AddSprinkleTime(SprinklerData sprinkler, DateTime dateTime, uint duration)
        {
            SprinklerDateTime newSprinkleTime = new SprinklerDateTime
            {
                WateringDateTime = dateTime,
                WateringDuration = duration
            };

            sprinkler.SprinkleTimeList.Add(newSprinkleTime);
            this._liteDb.UpdateData(sprinkler);
            this.Sprinklers.Find(x => x.Id == sprinkler.Id).SprinkleTimeList.Add(newSprinkleTime);
        }

        #endregion
    }
}
