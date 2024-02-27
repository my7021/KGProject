using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using static MyDataNet;

class ChatData : DataReceiver.PacketHeader
{
    public string userName { get; set; } = "";
    public string msg { get; set; } = "";

    public int GetPacketSize()
    {
        int nSize = sizeof(short) * 2;
        nSize += GetBytesSendSize(userName);
        nSize += GetBytesSendSize(msg);
        return nSize;
    }

    public ChatData(string sUserName, string sMsg)
    {
        this.userName = sUserName;
        this.msg = sMsg;
        packetId = (short)MyDataNet.EPacket.Chat;
        this.size = (short)GetPacketSize();
    }

    public ChatData()
    {
        packetId = (short)MyDataNet.EPacket.Chat;
    }

    public void ReceivedData(byte[] data)
    {
        byte[] packet = new byte[1024];
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);
            packetId = br.ReadInt16();
            size = br.ReadInt16();
            userName = ReadString(br);
            msg = ReadString(br);
        br.Close();
        ms.Close();
    }

    public byte[] SendData()
    {
        byte[] packet = new byte[1024];
        this.size = (short)GetPacketSize();
        MemoryStream ms = new MemoryStream(packet);
        BinaryWriter bw = new BinaryWriter(ms);
        bw.Write(packetId);
        bw.Write(size);
        WriteString(bw, userName);
        WriteString(bw, msg);
        ms.Close();
        bw.Close();
        return packet;
    }
}


public class MyDataNet : MonoBehaviour
{
    static MyDataNet _inst = null;
    public static MyDataNet Inst
    {
        get
        {
            if(_inst == null)
            {
                GameObject go = new GameObject("MyDataNet");
                _inst = go.AddComponent<MyDataNet>();
                DontDestroyOnLoad(go);
            }
            return _inst;
        }
    }

    public enum EPacket
    {
        Chat = 1001,
    }
    DataSender m_DataSender = new DataSender();
    DataReceiver m_DataReceiver = null;
    Coroutine m_Coroutine = null;

    Queue<DataReceiver.PacketData> m_eventQueue = new Queue<DataReceiver.PacketData>();

    public MultiChatScene chatScene { get; set; } = null;
    public bool IsAction { get; set; } = true;

    public void StartServer(string ip, int port)
    {
        if (m_DataReceiver != null) return;
        m_DataReceiver = new DataReceiver(ip, port);
        m_DataReceiver.Start();
        m_DataReceiver.OnReceiveHandeler += onChat_ReceiveHandler;
        m_Coroutine = StartCoroutine(CorReceivedData());
    }
    public void CloseServer()
    {
        if (m_DataReceiver != null)
        {
            m_DataReceiver.OnReceiveHandeler -= onChat_ReceiveHandler;
            m_DataReceiver.CloseSocket();
        }
        IsAction = false;
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
        }
    }
    void onChat_ReceiveHandler(object sender, DataReceiver.PacketData data)
    {
        //Debug.Log(data);
        m_eventQueue.Enqueue(data);
    }

    IEnumerator CorReceivedData()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_eventQueue.Count > 0);

            DataReceiver.PacketData data = m_eventQueue.Dequeue();
            if (data != null)
            {
                Debug.Log(data);
                OnReceived_Data(data);
            }
        }
    }
    public void OnReceived_Data(DataReceiver.PacketData kData)
    {
        if (kData.packetId == (int)EPacket.Chat)
        {
            Receive_ChatData(kData);
        }
    }
    void Receive_ChatData(DataReceiver.PacketData kData)
    {
        MemoryStream ms = new MemoryStream(kData.packet);
        BinaryReader br = new BinaryReader(ms);

        short packetId = br.ReadInt16();
        short size = br.ReadInt16();

        string sName = kData.ReadString(br);
        string sMsg = kData.ReadString(br);

        br.Close();
        ms.Close();

        chatScene.m_ChatDlg.AddMessage(sName, sMsg);
    }
    private void SendChatMsg(string sIP, int nPort, string sName, string sMsg)
    {
        ChatData kData = new ChatData(sName, sMsg);
        byte[] packet = kData.SendData();
        m_DataSender.SendPacketAsync(sIP, nPort, packet);
    }

    public void SendBrodcastChatMsg(string sMsg)
    {
        var list = SocketIoMgr.Inst.m_UserInfoList.datas;
        var kMyUser = SocketIoMgr.Inst.m_MyUserInfo;
        ChatData kData = new ChatData(kMyUser.id, sMsg);
        byte[] packet = kData.SendData();
        for (int i = 0; i < list.Count; i++)
        {
            var kUser = list[i];
            if (kUser.id != kMyUser.id)
                m_DataSender.SendPacketAsync(kUser.ip, kUser.dataPort, packet);
        }
    }

    private void OnReceived_ChatData(DataReceiver.PacketData kData)
    {
        Debug.LogFormat("[SocketIO OnReceived_ChatData..!");
        ChatData kChat = new ChatData();
        kChat.ReceivedData(kData.packet);

    }
}
