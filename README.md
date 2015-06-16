# TwitchIRC.cs 
Is a lightweight IRC client component for use with the Unity Engine.
## Prerequisites
In order to connect to Twitch IRC, you must have three pieces of information:

1. The name of channel that you want to join.
2. A Twitch account.
3. Oauth token from API or from a site using it: www.twitchapps.com/tmi

## Usage
Enter your twitch name, oath token and the name of the channel you want to join in the inspector of TwitchIRC.
Drop a component that utilizes TwitchIRC.cs or try the TwitchChatExample.cs chat component.

## API
Sending:

- TwitchIRC.SendMessage(string msg)
- TwitchIRC.SendCommand(string cmd)

Reading:
 Add a listener via:
- TwitchIRC.messageRecievedEvent.AddListener(function)

For an example see TwitchChatExample.cs

## Known issues
- If you send more than 20 commands or messages to the server within a 30 second period, you will get locked out for 8 hours automatically. These are **not** lifted so please be careful when working with IRC!
