using System;

namespace Slack.EventListener.Models
{
    public class PhilipsHueSettings
    {
        public string ApplicationKey { get; set; }
        public string PresenceLightName { get; set; }
        public string BusyColorHex { get; set; }
        public string AvailableColorHex { get; set; }
        public string BusyStatusTextIndicators { get; set; }

    }
}
