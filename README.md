This solution contains the following .NET Core console applications:

# Slack Event Listener
Slack Event Listener App listens user change events from Slack. Event is triggered when presence status of the user is changed in Slack. If status indicates that I’m in the meeting then App changes light colors to red or green by using Philips Hue APIs.

# Hue App Registration
Hue App Registration registers your application to Hue Bridge. After registration you got an application key value which is used in the Slack Event Listener app.

# Third party nugets used

- Q42.HueApi
- SlackNet
