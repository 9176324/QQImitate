using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Server
{
    class Program
    {
        private const int listenPort = 3344;
        private static String online = "";
        // sever listen call send1 or send2
        public static void ServerListen()
        {
            // 创建listener
            UdpClient listener = new UdpClient(listenPort);
            // 监听策略
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            
            try
            {
                while (true)
                {
                    
                    // 以 groupEP策略开始监听
                    byte[] bytes = listener.Receive(ref groupEP);
                    string strc = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    if (strc.Substring(0,5) == "get  ")
                    {
                        GetOnline(strc.Substring(5,strc.Length-5));
                        Console.WriteLine(strc);
                        strc = null;
                    }else if(strc.Substring(0,5) == "push "){
                        PushOnline(strc.Substring(5,strc.Length-5));
                        Console.WriteLine(strc);
                        strc = null;
                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                
                listener.Close();
                
            }
        }

        private static void GetOnline(string strc)
        {
            send(strc.Split(':')[0], int.Parse(strc.Split(':')[1]),online);
            
        }

        private static void PushOnline(string strc)
        {
            online +=  " " + strc ;
        }
        private static void send(string ipa ,int lport, string sp){
           
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(ipa, lport);

           
            Byte[] sendBytes = Encoding.ASCII.GetBytes(sp);

            udpClient.Send(sendBytes, sendBytes.Length);
        }
        static void Main(string[] args)
        {

            ServerListen();
            
        }
    }
}
