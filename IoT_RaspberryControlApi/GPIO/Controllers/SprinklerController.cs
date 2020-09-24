using IoT_RaspberryControlApi.Templates;
using System;
using System.Device.Gpio;

namespace IoT_RaspberryControlApi.GPIO.Controllers
{
    public class SprinklerController : GpioAddonTemplate
    {
        public PinValue _currentPinValue;

        public SprinklerController(int gpioPin, PinValue expectedPinValue) : base(gpioPin, PinMode.Output)
        {
            this._currentPinValue = expectedPinValue;
            PiGpioController.Write(gpioPin, this._currentPinValue);
        }

        public void Toggle()
        {
            if (this._currentPinValue == PinValue.Low)
            {
                PiGpioController.Write(this.gpioPin, this._currentPinValue = PinValue.High);
            }
            else
            {
                PiGpioController.Write(this.gpioPin, this._currentPinValue = PinValue.Low);
            }
        }

        public void SetState(bool state)
        {
            if (state)
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
