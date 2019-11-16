using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QQL.source
{
    class Friend
    {
        private const int listenPort = 3366;
        private const string localip = "127.0.0.1";
        private const string remoteip = "127.0.0.1";
        public string allMac = "";
        private const int serverPort = 3344;
        public void clienPush()
        {
            this.sendP(remoteip, "push " + localip + ':' + listenPort + '|' + ChatA.chatPort);
        }
        public void clienGet()
        {

            // 创建listener
            UdpClient listener = new UdpClient(listenPort);
            // 监听策略
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    // 请求 get + local_ip
                    this.sendP(remoteip, "get  " + localip + ':' + listenPort);
                    // 以 groupEP策略开始监听
                    byte[] bytes = listener.Receive(ref groupEP);
                    string strc = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    if (strc != "")
                    {
                        allMac = strc;
                    }
                    Thread.Sleep(500);
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
        public void friendRefresh(object usp)
        {
            user us = (user)usp;

            while (true)
            {
                string strc = allMac;
                string ppath = "../Images/qq.png";
                
                string[] pall = strc.Split(' ');
                if(pall.Length <= 1)
                {
                    continue;
                }
                string[] sall = new string[pall.Length - 2];
                for (int i = 1, j = 0; i < pall.Length; i++)
                {
                    if (pall[i] == localip + ':' + listenPort + '|' + ChatA.chatPort)
                    {
                        continue;
                    }
                    sall[j] = pall[i];
                    j++;
                }
                int count = 0;
                Action action1 = () =>
                {
                    us.canvas.Children.Clear();
                    foreach (string i in sall)
                    {
                        // 输出显示到user界面中


                        Image im = new Image
                        {
                            Source = new BitmapImage(new Uri(ppath, UriKind.RelativeOrAbsolute)),
                            Height = 45,
                            Width = 53,
                            Cursor = System.Windows.Input.Cursors.Hand,
                            VerticalAlignment = 0,

                        };
                        Label lb = new Label
                        {
                            Content = i,
                            Width = 200,
                            FontSize = 18,
                            HorizontalAlignment = 0,
                            Name = "labelCo"
                        };

                        im.MouseLeftButtonUp += us.Image_MouseLeftButtonUp;
                        Canvas.SetLeft(lb, 85);
                        Canvas.SetLeft(im, 25);
                        Canvas.SetTop(lb, 40 + 45 * count);
                        Canvas.SetTop(im, 30 + 45 * count);
                        us.canvas.Children.Add(lb);
                        us.canvas.Children.Add(im);
                        count++;
                    }

                    //us.Show();
                };
                us.Dispatcher.BeginInvoke(action1);
                Thread.Sleep(1000);
            }
        }
        private void sendP(string ipa, string strc)
        {
            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(ipa);
            IPEndPoint ep = new IPEndPoint(broadcast, serverPort);

            byte[] sendbuf = Encoding.ASCII.GetBytes(strc);

            s.SendTo(sendbuf, ep);
            s.Close();
        }
    }
}
