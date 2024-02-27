using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MyChatNet2 : MonoBehaviour
{
    public enum EPacket
    {
        chat = 1001,
    }

    public static MyChatNet2 Inst = null;

    ChatSender2 m_ChatSender = new ChatSender2();
    ChatReceiver2 m_ChatReceiver = null;
    Coroutine m_Coroutine = null;

    Queue<ChatReceiver2.PacketData> m_EventQueue = new Queue<ChatReceiver2.PacketData>();

    public ChatScene2 m_ChatScene { get; set; } = null;
    public ChatDlg2 m_ChatDlg = null;
    public bool isActive { get; set; } = true;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        m_Coroutine = StartCoroutine(IEnum_Receive());
    }

    public void StartServer(string IP, int Port)
    {
        if (m_ChatReceiver == null)
        {
            m_ChatReceiver = new ChatReceiver2(IP, Port);

            m_ChatReceiver.OnReceiveHandler += ReceivePacketData;
            m_ChatReceiver.Start();
        }
    }

    public void CloseServer()
    {
        if (m_ChatReceiver != null)
        {
            m_ChatReceiver.OnReceiveHandler -= ReceivePacketData;
            m_ChatReceiver.CloseSocket();
            isActive = false;
        }

        if (m_Coroutine != null)
            StopCoroutine(m_Coroutine);
    }

    void ReceivePacketData(object sender, ChatReceiver2.PacketData data)
    {
        m_EventQueue.Enqueue(data);
    }

    IEnumerator IEnum_Receive()
    {
        while (isActive)
        {
            yield return new WaitUntil(() => m_EventQueue.Count > 0);

            var kData = m_EventQueue.Dequeue();
            if (kData != null)
            {
                OnReceived_Data(kData);
            }
        }
    }

    private void OnReceived_Data(ChatReceiver2.PacketData kData)
    {
        if (kData.packetID == (short)EPacket.chat)
        {
            MemoryStream ms = new MemoryStream(kData.data);
            BinaryReader br = new BinaryReader(ms);

            short nameSize = br.ReadInt16();
            byte[] nameBuf = br.ReadBytes(nameSize);

            short msgSize = br.ReadInt16();
            byte[] msgBuf = br.ReadBytes(msgSize);

            br.Close();
            ms.Close();

            string sName = Encoding.UTF8.GetString(nameBuf);
            string sMsg = Encoding.UTF8.GetString(msgBuf);

            m_ChatDlg.AddMessage(sName, sMsg);
        }
    }

    public void SendMessage(string otherIP, int otherPort, string name, string msg)
    {
        byte[] packet = new byte[1024];

        short packetID = (short)EPacket.chat;

        byte[] nameBuf = Encoding.UTF8.GetBytes(name);
        short nameSize = (short)nameBuf.Length;
        byte[] msgBuf = Encoding.UTF8.GetBytes(msg);
        short msgSize = (short)msgBuf.Length;

        short size = (short)(ChatReceiver2.DHeaderSize + name.Length * 3 + msg.Length * 3 + sizeof(short) * 2);

        MemoryStream ms = new MemoryStream(packet);
        BinaryWriter bw = new BinaryWriter(ms);

        bw.Write(packetID);
        bw.Write(size);

        bw.Write(nameSize);
        bw.Write(nameBuf);

        bw.Write(msgSize);
        bw.Write(msgBuf);

        bw.Close();
        ms.Close();

        m_ChatSender.SendMsgAsync(otherIP, otherPort, packet);
    }

    private void OnDestroy()
    {
        CloseServer();
    }
}
