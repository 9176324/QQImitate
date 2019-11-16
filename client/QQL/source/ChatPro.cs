using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QQL.source
{
    public class ChatPro
    {
        public static String allInfo = "";
        public const int chatPort = 4466;
        public NetworkStream stream = null;
        // 主动连接调用

        public void SendTo(String ipa , int toPort ,String message)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(ipa, toPort);
                stream = client.GetStream();
                // 连接建立，重定向到流
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // 发送消息
                stream.Write(data, 0, data.Length);
                
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
                stream.Close();
                client.Close();
            }

        }
        public string GetItp()
        {
            try
            {
                Byte[] data = new Byte[256];

                String responseData = "";
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                return responseData;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return "";
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return "";
            }

        }
        ~ChatPro()
        {
            stream.Close();
        }
        // 独立线程 被动接入
        public void clientlistener()
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, chatPort);
                server.Start();

                while (true)
                {

                    TcpClient clientp = server.AcceptTcpClient();
                    // 监听连接


                    NetworkStream streamp = clientp.GetStream();
                    // 建立连接成功
                    int i = 0;
                    Byte[] bytes = new Byte[256];
                    if ((i = streamp.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        stream = streamp;
                        //Chat chat = new Chat();
                        //chat.chatLabel.Content += GetItp();
                        //chat.Show();
                        //stream.Close();
                        //clientp.Close();
                    }

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
