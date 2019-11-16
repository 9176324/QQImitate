using System.Net.Sockets;
using System.Windows.Input;

namespace QQL
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string userid = userLogin.Text;
            string uerpass = userPass.Password;

            TcpClient tcp = new TcpClient();



            tcp.Connect("127.0.0.1", 8080);//根据服务器的IP地址和侦听的端口连接

            if (tcp.Connected)
            {
                System.Environment.Exit(0);
            }
            //这里需要注意的是，不管是使用有参数的构造函数与服务器连接，或者是通过Connect()方法与服务器建立连接
            // 都是同步方法（或者说是阻塞的，英文叫block）。
            //它的意思是说，客户端在与服务端连接成功、从而方法返回，或者是服务端不存、从而抛出异常之前，是无法继续进行后继操作的。
            // 这里还有一个名为BeginConnect()的方法，用于实施异步的连接，这样程序不会被阻塞，可以立即执行后面的操作，这是因为可能由于网络拥塞等问题，连接需要较长时间才能完成。
            // 网络编程中有非常多的异步操作，凡事都是由简入难，关于异步操作，我们后面再讨论，现在只看同步操作。

            //建立连接服务端的数据流

            NetworkStream streamToServer = tcp.GetStream();
            //接收和发送数据

            //发送字符串
            string msg = userPass + ":" + userLogin;
            byte[] buffer = System.Text.Encoding.Unicode.GetBytes(msg); //msg为发送的字符串

            try

            {

                lock (streamToServer)

                {

                    streamToServer.Write(buffer, 0, buffer.Length); // 发往服务器

                }



                //接收字符串
                //int BufferSize = 1024;
                //buffer = new byte[BufferSize];

                //lock (streamToServer)

                //{

                // int bytesRead = streamToServer.Read(buffer, 0, BufferSize);

                //}

            }
            finally
            {
                System.Environment.Exit(0);
            }
        }
    }
}
