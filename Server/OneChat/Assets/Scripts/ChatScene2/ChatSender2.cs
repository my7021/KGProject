using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ChatSender2
{
    delegate void SendDele(string IP, int port, byte[] packet);

    public void SendMsgAsync(string IP, int port, byte[] packet)
    {
        SendDele dele = SendMsg;
        dele.BeginInvoke(IP, port, packet, null, null);
    }

    private void SendMsg(string IP, int port, byte[] packet)
    {
        try
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iePoint = new IPEndPoint(IPAddress.Parse(IP), port);
            socket.Connect(iePoint);

            socket.Send(packet);
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    #region
    //private byte[] MakeMsgPacket(string name, string msg)
    //{
    //    byte[] packet = new byte[1024];

    //    MemoryStream ms = new MemoryStream(packet);
    //    BinaryWriter bw = new BinaryWriter(ms);

    //    bw.Write(m_PacketId);
    //    bw.Write(name);
    //    bw.Write(msg);

    //    bw.Close();
    //    ms.Close();

    //    return packet;
    //}
    #endregion
}
