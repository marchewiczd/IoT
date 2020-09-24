using IoT_Api.Database;
using IoT_RaspberryServer.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_RaspberryControlApi.GPIO.Controllers
{
    public static class PiActionController
    {
        //make it thread safe
        private static Dictionary<ulong, SprinklerController> _sprinklerControllerList = new Dictionary<ulong, SprinklerController>();

        private static bool _stopTask = false;

        private static object _piActionLock = new object();

        private static object _controllerListLock = new object();

        private static bool _isInitialized = false;

        public static void Initialize()
        {
            foreach(var sprinklerData in new LiteDb<SprinklerData>(AppSettings.LiteDbFilePath).GetAll())
            {
                _sprinklerControllerList.Add(sprinklerData.Id, new SprinklerController(sprinklerData.GpioPin, sprinklerData.SprinkleStatus));
            }

            //StartPiActionTask();

            //_isInitialized = true;
        }

        public static void AddSprinklerController(ulong sprinkleDataId, SprinklerController sprinklerController)
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_controllerListLock)
            {
                _sprinklerControllerList.Add(sprinkleDataId, sprinklerController);
            }
        }

        public static void DeleteSprinklerController(ulong sprinklerDataId)
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_controllerListLock)
            {
                _sprinklerControllerList[sprinklerDataId].Dispose();
                _sprinklerControllerList.Remove(sprinklerDataId);
            }
        }

        public static void ClearSprinklerControllers()
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_controllerListLock)
            {
                foreach(var controller in _sprinklerControllerList)
                {
                    controller.Value.Dispose();
                }

                _sprinklerControllerList.Clear();
            }
        }

        public static void ToggleSprinklerController(ulong sprinklerDataId)
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_controllerListLock)
            {
                _sprinklerControllerList[sprinklerDataId].Toggle();
            }
        }

        public static void SetSprinklerControllerState(ulong sprinklerDataId, bool state)
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_controllerListLock)
            {
                _sprinklerControllerList[sprinklerDataId].SetState(state);
            }
        }

        public static void PiAction()
        {
            int tempCounter = 0;

            while (!_stopTask)
            {
                Console.WriteLine($"PiAction {tempCounter++}");
                lock (_controllerListLock)
                {
                    foreach(var sprinklerController in _sprinklerControllerList)
                    {
                        Console.WriteLine($"\tController {sprinklerController.Key} action. Current state: [{sprinklerController.Value._currentPinValue}]");
                    }
                }

                Thread.Sleep(10000);
            }

            lock (_piActionLock)
            {
                _stopTask = false;
            }
        }

        public static void StopPiActionTask()
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_piActionLock)
            {
                _stopTask = true;
            }
        }

        public static void StartPiActionTask()
        {
            if (!_isInitialized)
            {
                return;
            }

            lock (_piActionLock)
            {
                _stopTask = false;
            }

            Task.Run(PiAction);
        }
    }
}
