using IoT_RaspberryControlApi.GPIO.Controllers;
using System;
using System.Device.Gpio;

namespace IoT_RaspberryControlApi.Templates
{
    public abstract class GpioAddonTemplate : IDisposable
    {
        protected int gpioPin;

        protected PinMode pinMode;

        public GpioAddonTemplate(int gpioPin, PinMode pinMode)
        {
            this.gpioPin = gpioPin;
            this.pinMode = pinMode;

            PiGpioController.OpenPin(gpioPin, pinMode);
        }

        public void Dispose()
        {
            PiGpioController.ClosePin(this.gpioPin);
        }
    }
}