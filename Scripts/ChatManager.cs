using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    [SerializeField] string userID;

    [SerializeField] Text chatText;
    [SerializeField] InputField textMessage;
    [SerializeField] InputField textUserName;

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"{level},{message}");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state);
    }

    public void OnConnected()
    {
        chatText.text += "\n �� ������������ � ����!";
        chatClient.Subscribe("globalChat");
    }

    public void OnDisconnected()
    {
        chatClient.Unsubscribe(new string[] { "globalChat" });
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            chatText.text += $"\n[{channelName}] {senders[i]}: {messages[i]}";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            chatText.text += $"�� ������������ � {channels[i]}";
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            chatText.text += $"�� ��������� �� {channels[i]}";
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        chatText.text += $"������������ {user} ����������� � {channel}";
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        chatText.text += $"������������ {user} ���������� �� {channel}";
    }

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
    }

    void Update()
    {
        chatClient.Service();
    }

    public void SendButton()
    {
        if(textUserName.text == "")
        {
            chatClient.PublishMessage("globalChat", textMessage.text);
        }
    }
}
