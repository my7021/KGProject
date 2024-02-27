using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class ChatReceiver2
{
    [Serializable]
    public class PacketBase
    {
        public short packetID { get; set; }
        public short packetSize { get; set; }

        public PacketBase() { }
        public PacketBase(short packetID, short packetSize)
        {
            this.packetID = packetID;
            this.packetSize = packetSize;
        }
    }

    [Serializable]
    public class PacketData : PacketBase
    {
        public byte[] data { get; set; }

        public PacketData(short packetID, short packetSize, byte[] kData) : base(packetID, packetSize)
        {
            data = kData;
        }

        public PacketData() { }
    }

    public event EventHandler<PacketData> OnReceiveHandler = null;
    public const int DHeaderSize = 4;

    public string ipStr { get; private set; }
    public int port { get; private set; }
    Socket m_Socket = null;

    public bool isActive { get; set; } = true;

    public ChatReceiver2(string ip, int port)
    {
        ipStr = ip;
        this.port = port;
    }

    public bool Start()
    {
        try
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iepoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
            m_Socket.Bind(iepoint);
            m_Socket.Listen(5);

            AcceptLoopAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return false;
        }
    }

    public void CloseSocket()
    {
        m_Socket?.Close();
    }

    delegate void AcceptDale();
    public void AcceptLoopAsync()
    {
        AcceptDale dele = AcceptLoop;
        dele.BeginInvoke(null, null);
    }
    public void AcceptLoop()
    {
        Socket doSocket = null;

        while (isActive)
        {
            doSocket = m_Socket.Accept();
            DoItAsync(doSocket);
        }
    }

    delegate void DoItDele(Socket doSocket);

    private void DoItAsync(Socket doSocket)
    {
        DoItDele dele = DoIt;
        dele.BeginInvoke(doSocket, null, null);

        //DoIt(doSocket);
    }

    private void DoIt(Socket doSocket)
    {
        byte[] packet = new byte[1024];

        doSocket.Receive(packet);
        doSocket.Close();


        MemoryStream ms = new MemoryStream(packet);
        BinaryReader br = new BinaryReader(ms);

        short packetID = br.ReadInt16();
        short packetSize = br.ReadInt16();

        int bodySize = packetSize - DHeaderSize;
        byte[] kData = br.ReadBytes(bodySize);

        br.Close();
        ms.Close();

        // Thread 때문에 UI가 동작하지 않는다.
        if (OnReceiveHandler != null)
        {
            OnReceiveHandler(this, new PacketData(packetID, packetSize, kData));
        }
    }
}