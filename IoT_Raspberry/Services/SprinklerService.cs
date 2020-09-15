using IoT_Api.Database;
using IoT_RaspberryServer.Data;
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

        public Sprinkler FindByGpio(int gpioPin)
        {
            return this.Sprinklers.Find(x => x.GpioPin == gpioPin);
        }

        public void DeleteSprinkler(Sprinkler sprinkler)
        {
            this._liteDb.Delete(sprinkler.Id);
            this.Sprinklers = this._liteDb.GetAll();
        }
        
        public void DeleteSprinkler(ulong id)
        {
            this._liteDb.Delete(id);
            this.Sprinklers = this._liteDb.GetAll();
        }
    }
}
