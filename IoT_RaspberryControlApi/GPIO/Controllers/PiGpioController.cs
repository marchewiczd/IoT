using System;
using System.Device.Gpio;
using System.Diagnostics;

namespace IoT_RaspberryControlApi.GPIO.Controllers
{
    public static class PiGpioController
    {
        private static GpioController controller;

        public static void Initialize()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Initializing PI GPIO Controller.");
            }
            else
            {
                Console.WriteLine($"Initializing PI GPIO Controller.");
                controller = new GpioController();
            }
        }

        public static void OpenPin(int pinNumber, PinMode pinMode)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Opening pin {pinNumber} with mode {pinMode}");
            }
            else
            {
                if (!controller.IsPinOpen(pinNumber))
                {
                    Console.WriteLine($"Opening pin {pinNumber} with mode {pinMode}");
                    controller.OpenPin(pinNumber, pinMode);
                }
                else
                {
                    Console.WriteLine($"Attempting to open pin {pinNumber} failed. Pin already open.");
                }
            }
        }

        public static void SetMode(int pinNumber, PinMode pinMode)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Setting pin {pinNumber} mode to {pinMode}");
            }
            else
            {
                Console.WriteLine($"Setting pin {pinNumber} mode to {pinMode}");
                controller.SetPinMode(pinNumber, pinMode);
            }
        }

        public static PinValue ReadPin(int pinNumber)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Reading value from pin {pinNumber}");
                return PinValue.Low;
            }
            else
            {
                Console.WriteLine($"Reading value from pin {pinNumber}");
                return controller.Read(pinNumber);
            }
        }

        public static void Write(int pinNumber, PinValue pinValue)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Writing {pinValue} to {pinNumber}");
            }
            else
            {
                Console.WriteLine($"Writing {pinValue} to {pinNumber}");
                controller.Write(pinNumber, pinValue);

            }
        }

        public static void ClosePin(int pinNumber)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine($"[DEBUG] Closing pin {pinNumber}");
            }
            else
            {
                Console.WriteLine($"Closing pin {pinNumber}");
                controller.ClosePin(pinNumber);
            }
        }
    }
}
