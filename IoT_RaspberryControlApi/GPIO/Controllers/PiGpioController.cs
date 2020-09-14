using System.Device.Gpio;

namespace IoT_RaspberryControlApi.GPIO.Controllers
{
    public static class PiGpioController
    {
        private static GpioController _gpioController = new GpioController();

        public static void OpenPin(int pinNumber, PinMode pinMode)
        {
            if (!_gpioController.IsPinOpen(pinNumber))
            {
                _gpioController.OpenPin(pinNumber, pinMode);
            }
        }

        public static PinValue ReadPin(int pinNumber)
        {
            if (_gpioController.IsPinOpen(pinNumber))
            {
                return _gpioController.Read(pinNumber);
            }

            return PinValue.Low;
        }

        public static void Write(int pinNumber, PinValue pinValue)
        {
            _gpioController.Write(pinNumber, pinValue);
        }
    }
}
