using Slack.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Slack.RegisterHueApp
{
    class Program: ConsoleBase
    {
        static async Task Main(string[] args)
        {
            var applicationName = args[0];
            var deviceName = args[1];

            await InitPhilipsBridgeClient();

            var appKey = await RegisterPhilipsHueApp(applicationName, deviceName);

            Console.WriteLine($"Your app key is {appKey}. Save this key to the EventListener App's appsettings file!");
          
        }

        /// <summary>
        /// Registers Philips Hue application to bridge
        /// </summary>
        /// <returns></returns>
        private static async Task<string> RegisterPhilipsHueApp(string applicationName, string deviceName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                Console.WriteLine("Application name is empty!");
                return string.Empty;
            }
            if (string.IsNullOrEmpty(deviceName))
            {
                Console.WriteLine("Decice name is empty!");
                return string.Empty;
            }

            Console.WriteLine("Press the button on the bridge before pressing any key!");

            await WaitForKeyPress();

            //Make sure the user has pressed the button on the bridge before calling RegisterAsync
            //It will throw an LinkButtonNotPressedException if the user did not press the button
            var appKey = await _client.RegisterAsync(applicationName, deviceName);

            return appKey;
        }
    }
}
