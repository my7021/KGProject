using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChatNet : MonoBehaviour
{
    public enum EPacket
    {
        Chat = 0,
    }

    public static MyChatNet Inst = null;

    public ChatSender m_ChatSender = new ChatSender();
    public ChatReceiver m_ChatReceiver = null;
    Coroutine m_Coroutine = null;

    public Queue<ChatReceiver.PacketData> m_eventQueue = new Queue<ChatReceiver.PacketData>();

    public ChatDlg chatDlg;
    public bool IsAction { get; set; } = true;

    void Awake()
    {
        Inst = this;
    }

    public void StartServer(string ip, int port)
    {
        m_ChatReceiver = new ChatReceiver(ip, port);
        m_ChatReceiver.OnChatReceiveHandeler += onChatReceiveHandeler;
        m_ChatReceiver.Start();
        m_Coroutine = StartCoroutine(Enum_Receive());
    }

    void onChatReceiveHandeler(object sender, ChatReceiver.PacketData e)
    {
        m_eventQueue.Enqueue(e);
    }

    public ChatReceiver.ReceiveData Dequeue()
    {
         ChatReceiver.ReceiveData data = (ChatReceiver.ReceiveData)m_eventQueue.Dequeue();
        return data;
    }

    IEnumerator Enum_Receive()
    {
        while(IsAction)
        {
            yield return new WaitUntil(() => MyChatNet.Inst.m_eventQueue.Count > 0);
            ChatReceiver.ReceiveData data = Dequeue();
            string name = data.userName;
            string msg = data.msg;
            chatDlg.CreateMsg(name, msg);
        }
    }
}
