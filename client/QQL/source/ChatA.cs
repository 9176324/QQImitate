using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace QQL.source
{
    public class ChatA
    {
        public String allInfo = "";
        public const int chatPort = 4466;

        //private string ipad;
        //private int pdort;


        //主动连接调用
        //public void buildC(String ipa, int toPort)
        //{
        //    ipad = ipa;
        //    pdort = toPort;
        //    try
        //    {
        //        client = new TcpClient(ipa, toPort);
        //        stream = client.GetStream();
        //        // 连接建立，重定向到流
        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }

        //}
        public void SendTo(String ipa, int toPort, String message)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {

                client = new TcpClient(ipa, toPort);
                stream = client.GetStream();
                Thread.Sleep(500);

                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
                // 发送消息
                stream.Write(data, 0, data.Length);
                allInfo += "我方说：" + message + "\n";

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                client.Close();
                stream.Close();

            }

        }
        //public void GetItp()
        //{
        //    try
        //    {
        //        Byte[] data = new Byte[256];

        //        String responseData = "";
        //        Int32 bytes = stream.Read(data, 0, data.Length);
        //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //        allInfo += responseData;

        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //        //return "";
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //        //return "";
        //    }

        //}
        ~ChatA()
        {

        }
        // 独立线程 被动接入
        public void clientlistener()
        {
            TcpListener server = null;
            TcpClient clientp = null;
            NetworkStream streamp = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, chatPort);
                server.Start();
                
                while (true)
                {
                    int i = 0;
                    Byte[] data = new Byte[256];
                    Thread.Sleep(1000);
                   

                    clientp = server.AcceptTcpClient();
                    // 监听连接
                    streamp = clientp.GetStream();
                    // 建立连接成功

                    if ((i = streamp.Read(data, 0, data.Length)) != 0)
                    {
                        String responseData = "";
                        responseData = System.Text.Encoding.Unicode.GetString(data, 0, i);
                        allInfo += "对方说：" + responseData + "\n";
                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                clientp.Close();
                streamp.Close();
                server.Stop();
            }

        }
        public void chatRefresh(object csp)
        {
            Chat cs = (Chat)csp;
            while (true)
            {

                Thread.Sleep(1000);

                Action action1 = () =>
                {
                    cs.chatLabel.Content = allInfo;
                };
                cs.Dispatcher.BeginInvoke(action1);


            }
        }
    }
}
