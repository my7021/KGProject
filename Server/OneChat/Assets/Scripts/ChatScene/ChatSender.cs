using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ChatSender
{
    delegate void SendDele(string otherIP, int otherPort, string sName, string sMsg);

    private int m_PacketId = 0;
    public void SendMsgAsync(string otherIP, int otherPort, string sName, string sMsg)
    {
        SendDele dele = SendMsg;
        dele.BeginInvoke(otherIP, otherPort, sName, sMsg, null, null);
    }
    private void SendMsg(string otherIP, int otherPort, string sName, string sMsg)
    {
        try
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iepoint = new IPEndPoint(IPAddress.Parse(otherIP), otherPort);
            socket.Connect(iepoint);

            byte[] packet = MakeMsgPacket(sName, sMsg);
            socket.Send(packet);
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    private byte[] MakeMsgPacket(string sName, string sMsg)
    {
        byte[] packet = new byte[1024];
        MemoryStream ms = new MemoryStream(packet);
        BinaryWriter bw = new BinaryWriter(ms);

        bw.Write(m_PacketId);
        bw.Write(sName);
        bw.Write(sMsg);

        bw.Close();
        ms.Close();
        return packet;
    }
}
