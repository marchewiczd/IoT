using IoT_RaspberryControlApi.Templates;
using System.Device.Gpio;

namespace IoT_RaspberryControlApi.GPIO.Controllers
{
    public class SprinklerController : GpioAddonTemplate
    {
        private PinValue _currentPinValue;

        public SprinklerController(int gpioPin) : base(gpioPin, PinMode.Output)
        {
            PiGpioController.Write(this.gpioPin, this._currentPinValue = PinValue.Low);
        }

        public void Toggle()
        {
            if(this._currentPinValue == PinValue.Low)
            {
                PiGpioController.Write(this.gpioPin, this._currentPinValue = PinValue.High);
            }
            else
            {
                PiGpioController.Write(this.gpioPin, this._currentPinValue = PinValue.Low);
            }
        }
    }
}
