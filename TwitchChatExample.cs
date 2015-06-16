using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TwitchIRC))]
public class TwitchChatExample : MonoBehaviour
{
    public int maxMessages = 100; //we start deleting UI elements when the count is larger than this var.
    private LinkedList<GameObject> messages =
        new LinkedList<GameObject>();
    public UnityEngine.UI.InputField inputField;
    public UnityEngine.UI.Button submitButton;
    public UnityEngine.RectTransform chatBox;
    public UnityEngine.UI.ScrollRect scrollRect;
    private TwitchIRC IRC;
    //when message is recieved from IRC-server or our own message.
    void OnChatMsgRecieved(string user, string msg)
    {
        if (messages.Count > maxMessages)
        {
            Destroy(messages.First.Value);
            messages.RemoveFirst();
        }

        GameObject go = new GameObject("twitchMsg");
        var text = go.AddComponent<UnityEngine.UI.Text>();
        var layout = go.AddComponent<UnityEngine.UI.LayoutElement>();
        go.transform.parent = chatBox;
        messages.AddLast(go);

        layout.minHeight = 20f;
        text.text = "<b>" + user + "</b>" + ": " + "<color=#32323E>" + msg + "</color>";
        text.color = ColorFromUsername(user);
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        scrollRect.velocity = new Vector2(0, 1000f);
    }
    //when Submit button is clicked or ENTER is pressed.
    public void OnSubmit()
    {
        if (inputField.text.Length > 0)
        {
            IRC.SendMessage(inputField.text);
            //invoke onchatmsgrecieved because the IRC server wont send the message to the sending client.
            OnChatMsgRecieved(IRC.nickName, inputField.text); //for local GUI only.
            inputField.text = "";
        }
    }
    Color ColorFromUsername(string username)
    {
        Random.seed = username.Length + (int)username[0] + (int)username[username.Length - 1];
        return new Color(Random.Range(0.25f, 0.55f), Random.Range(0.20f, 0.55f), Random.Range(0.25f, 0.55f));
    }
    // Use this for initialization
    void Start()
    {
        IRC = this.GetComponent<TwitchIRC>();
        IRC.messageRecievedEvent.AddListener(OnChatMsgRecieved);
    }
}
