using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EcoClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket sock = null;
            try {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 인터페이스 결합(옵션)
                // 연결
                IPAddress addr = IPAddress.Parse("192.168.0.61");
                IPEndPoint iep = new IPEndPoint(addr, 10040);
                sock.Connect(iep);
            }catch (Exception ex) {
                sock?.Close();
                Console.WriteLine(ex.ToString());
                return;
            }

            string str;
            string str2;
            byte[] packet = new byte[1024];
            byte[] packet2 = new byte[1024];
            while (true)
            {
                Console.WriteLine("전송할 메세지:");
                str = Console.ReadLine();
                MemoryStream ms = new MemoryStream(packet);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(str);
                bw.Close();
                ms.Close();
                sock.Send(packet);

                if (str == "exit")
                    break;

                sock.Receive(packet2);
                MemoryStream ms2 = new MemoryStream(packet2);
                BinaryReader br = new BinaryReader(ms2);
                str2 = br.ReadString();
                Console.WriteLine("수신한 메세지:{0}", str2);
                br.Close();
                ms2.Close();
            }
            sock.Close();
        }
    }
}
