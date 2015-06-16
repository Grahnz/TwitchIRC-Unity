using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwitchIRC : MonoBehaviour
{
    public string oath;
    public string nickName;
    public string channelName;
    private string server = "irc.twitch.tv";
    private int port = 6667;

    //event(nick,msg).
    public class MsgEvent : UnityEngine.Events.UnityEvent<string, string> { }
    public MsgEvent messageRecievedEvent = new MsgEvent();

    [HideInInspector()]
    public string buffer;
    private bool stopThread = false;
    private Queue<string> commandQueue = new Queue<string>();
    private List<KeyValuePair<string, string>> chatMsgs = new List<KeyValuePair<string, string>>();
    private void IRCProcedure()
    {
        System.Net.Sockets.TcpClient sock = new System.Net.Sockets.TcpClient();
        System.IO.TextReader input;
        System.IO.TextWriter output;
        //Connect to irc server and get input and output text streams from TcpClient.
        sock.Connect(server, port);
        if (!sock.Connected)
        {
            Debug.Log("Failed to connect!");
            return;
        }
        input = new System.IO.StreamReader(sock.GetStream());
        output = new System.IO.StreamWriter(sock.GetStream());

        //Send PASS & NICK.
        output.WriteLine("PASS " + oath);
        output.WriteLine("NICK " + nickName.ToLower());
        output.Flush();

        //Process each line received from irc server
        while (!stopThread)
        {
            buffer = input.ReadLine();
            if (buffer == null)
                continue;
            //Debug.Log(buffer);
            //Send pong reply to any ping messages
            if (buffer.StartsWith("PING "))
            {
                output.Write(buffer.Replace("PING", "PONG") + "\r\n");
                output.Flush();
            }
            if (buffer[0] != ':')
                continue;

            //was user message?
            if (buffer.Contains("PRIVMSG #"))
            {
                int msgIndex = buffer.IndexOf("PRIVMSG #");
                string msg = buffer.Substring(msgIndex + channelName.Length + 11);
                string user = buffer.Substring(1, buffer.IndexOf('!') - 1);

                OnMessageRecieved(user, msg);
                chatMsgs.Add(new KeyValuePair<string, string>(user, msg));
            }
            //do we have any commands to send?
            lock (commandQueue)
            {
                if (commandQueue.Count > 0)
                {
                    output.WriteLine(commandQueue.Peek());
                    output.Flush();
                    commandQueue.Dequeue();
                }
            }
            //After server sends 001 command, we can join a channel
            if (buffer.Split(' ')[1] == "001")
            {
                output.WriteLine("JOIN #" + channelName);
                output.Flush();
            }
        }
    }
    public void SendCommand(string cmd)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue(cmd);
        }
    }
    public void SendMessage(string msg)
    {
        lock (commandQueue)
        {
            commandQueue.Enqueue("PRIVMSG #" + channelName + " :" + msg);
        }
    }
    void OnMessageRecieved(string nick, string msg)
    {
        Debug.Log(nick + ": " + msg);
    }

    //MonoBehaviour Events.
    void Start()
    {
        System.Threading.Thread T = new System.Threading.Thread(IRCProcedure);
        stopThread = false;
        T.Start();
    }
    void OnDestroy()
    {
        stopThread = true;
    }
    void OnDisable()
    {
        stopThread = true;
    }
    void Update()
    {
        lock (chatMsgs)
        {
            if (chatMsgs.Count > 0)
            {
                for (int i = 0; i < chatMsgs.Count; i++)
                {
                    messageRecievedEvent.Invoke(chatMsgs[i].Key, chatMsgs[i].Value);
                }
                chatMsgs.Clear();
            }
        }
    }
}
