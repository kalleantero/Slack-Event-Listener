using Microsoft.Extensions.Configuration;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Slack.EventListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Shared
{
    public class ConsoleBase
    {
        public static ILocalHueClient _client = null;
        public static SlackSettings _slackSettings = null;
        public static PhilipsHueSettings _philipsHueSettings = null;

        public static void InitSettings(string directory)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(directory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            _slackSettings = configuration.GetSection("SlackSettings").Get<SlackSettings>();
            _philipsHueSettings = configuration.GetSection("PhilipsHueSettings").Get<PhilipsHueSettings>();
        }

        public static async Task InitPhilipsBridgeClient()
        {
            IBridgeLocator locator = new HttpBridgeLocator();
            IEnumerable<LocatedBridge> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            if (bridgeIPs != null)
            {
                var firstBridge = bridgeIPs.FirstOrDefault();

                if (firstBridge != null)
                {
                    _client = new LocalHueClient(firstBridge.IpAddress);
                }
            }
        }
        public static async Task InitPhilipsBridgeClientWithKey(string applicationKey)
        {
            await InitPhilipsBridgeClient();
           _client.Initialize(applicationKey);            
        }

        public static async Task WaitForKeyPress()
        {
            Console.WriteLine("Press any key...");
            while (!Console.KeyAvailable)
                await Task.Yield();
            Console.ReadKey();
        }
    }
}
