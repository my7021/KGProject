using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace EcoServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket sock = null;
            try {
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 인터페이스와 결합
                IPAddress addr = IPAddress.Parse("192.168.0.61");
                IPEndPoint iep = new IPEndPoint(addr, 10040);
                sock.Bind(iep);

                // 백로그 큐 크기 설정
                sock.Listen(5);
                Socket dosock;
                while (true)
                {               // AcceptLoop
                    dosock = sock.Accept();
                    DoIt(dosock);// DoIt
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            finally {
                sock?.Close();
            }
        }

        // 비동기 쓰레드 사용
        delegate void DoItDele(Socket dosock);
        private static void DoItAsync(Socket dosock)
        {
            DoItDele dele = DoIt;
            dele.BeginInvoke(dosock, null, null);
        }

        private static void DoIt(Socket dosock)
        {
            try
            {
                byte[] packet = new byte[1024];
                IPEndPoint iep = dosock.RemoteEndPoint as IPEndPoint;

                while (true)
                {
                    dosock.Receive(packet);

                    MemoryStream ms = new MemoryStream(packet);
                    BinaryReader br = new BinaryReader(ms);
                    string msg = br.ReadString();
                    br.Close();
                    ms.Close();

                    Console.WriteLine("{0};{1} -> {2}", iep.Address, iep.Port, msg);
                    if (msg == "exit")
                    {
                        break;
                    }
                    dosock.Send(packet);
                }
            }catch (Exception e) {
                Console.WriteLine(e.ToString());
            }finally {
                dosock.Close();
            }
        }
    }
}
