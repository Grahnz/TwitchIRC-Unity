![img](http://johangrahn.se/wp-content/uploads/2015/06/twitchChat.jpg)
# TwitchIRC.cs 
Is a lightweight IRC client component for use with the Unity Engine.
## Prerequisites
In order to connect to Twitch IRC, you must have three pieces of information:

1. The name of channel that you want to join.
2. A Twitch account.
3. Oauth token from API or from a site using it: www.twitchapps.com/tmi

## Usage
Enter your twitch name, oauth token and the name of the channel you want to join in the inspector of TwitchIRC.
Drop a component that utilizes TwitchIRC.cs or try the TwitchChatExample.cs chat component.

## API
Sending:

- TwitchIRC.SendMsg(string msg)
- TwitchIRC.SendCommand(string cmd)

Reading:
 Add a listener via:
- TwitchIRC.messageRecievedEvent.AddListener(function(string))

For an example see TwitchChatExample.cs

## Known issues
- Please tell me if you find any!
