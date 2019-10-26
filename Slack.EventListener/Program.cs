using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Slack.Shared;
using SlackNet;
using SlackNet.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters.Original;


namespace Slack.EventListener
{
    class Program: ConsoleBase
    {
    
        static async Task Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            InitSettings(currentDirectory);

            if (string.IsNullOrEmpty(_philipsHueSettings.ApplicationKey))
            {
                Console.WriteLine("Philips Hue Application key is missing from the appsettings!");
                return;
            }

            var applicationkey = _philipsHueSettings.ApplicationKey;

            await InitPhilipsBridgeClientWithKey(applicationkey);
            await StartListeningSlackEvents();           
        }    

        private static async Task SendCommandToPhilipsHueBridge(LightCommand command)
        {
            if (string.IsNullOrEmpty(_philipsHueSettings.PresenceLightName))
            {
                Console.WriteLine("Presence Light Name is missing from the appsettings!");
                return;
            }

            var lights = await _client.GetLightsAsync();

            if(lights == null)
            {
                Console.WriteLine("Lights not found!");
                return;
            }

            var presenceLight = lights.Where(x => x.Name == _philipsHueSettings.PresenceLightName).FirstOrDefault();

            if (presenceLight == null)
            {
                Console.WriteLine("Presence light not found. Check the light name from the appsettings!");
                return;
            }

            await _client.SendCommandAsync(command, new List<string> { presenceLight.Id });

            Console.WriteLine("Command sent to Philips Hue Bridge.");

        }

        private static async Task StartListeningSlackEvents()
        {
            if (string.IsNullOrEmpty(_slackSettings.Token))
            {
                Console.WriteLine("Slack token is missing from the appsettings!");
            }
            else
            {
                using (var rtmClient = new SlackRtmClient(_slackSettings.Token))
                {
                    await rtmClient.Connect().ConfigureAwait(false);
                    Console.WriteLine("Slack connected");
                    // subscribe user change events
                    var subscription = rtmClient.Events.Where(x => x.Type == "user_change").Subscribe(async args => await HandleSlackEvent(args));
                    await WaitForKeyPress().ConfigureAwait(false);
                }
            }
        }

        private static async Task<object> HandleSlackEvent(Event slackEvent)
        {
            var userChangeEvent = (UserChange)slackEvent;

            var command = new LightCommand();
            var busyColor = _philipsHueSettings.BusyColorHex ?? "ea0d0d";
            var availableColor = _philipsHueSettings.AvailableColorHex ?? "24d024";

            if (userChangeEvent?.User?.Profile?.StatusText == _philipsHueSettings.BusyStatusTextIndicators)
            {                
                command.TurnOn().SetColor(new RGBColor(busyColor));
            }
            else
            {
                command.TurnOn().SetColor(new RGBColor(availableColor));
            }

            await SendCommandToPhilipsHueBridge(command);

            return userChangeEvent;
        }       
    }    
}
