This repository is related to blog post "Status notifier app powered by Philips Hue lights" - https://devaaja.fi/blog/status-notifier-app-powered-by-philips-hue-lights

This solution contains the following .NET Core console applications. Q42.HueApi and SlackNet nuget packages were used in the API communication.

# Slack Event Listener
Slack Event Listener App listens user status change events from Slack. Event is triggered when presence status of the user is changed in Slack. If status indicates that I’m in the meeting then App changes light colors to red or green by using Philips Hue APIs.

# Hue App Registration
Hue App Registration registers your application to Hue Bridge. After registration you got an application key value which is used in the Slack Event Listener app.
