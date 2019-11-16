
using QQL.source;
using System;
using System.Threading;
using System.Windows;


namespace QQL
{
    /// <summary>
    /// Chat.xaml 的交互逻辑
    /// </summary>
    public partial class Chat : Window
    {
        public static ChatA chatA ;
        private Thread tp;
        private string remoteIpa;
        private int dpo;
        //public Chat()
        //{
        //    chatA = new ChatA();
        //    InitializeComponent();
        //}
        //public Chat(ChatA chatAp)
        //{
        //    this.chatA = chatAp;
        //    InitializeComponent();
        //}
        public Chat(ChatA chatp ,string remote,int tport)
        {
            chatA = chatp;
            remoteIpa = remote;
            dpo = tport;
            tp = new Thread(chatA.chatRefresh);
            tp.Start(this);
            
            //chatA.buildC(remoteIp, tport);
            InitializeComponent();
        }
        ~Chat()
        {
            
            tp.Abort();
        }
        protected override void OnClosed(EventArgs e)
        {
            //tpp.Abort();
            tp.Abort();

            base.OnClosed(e);
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            //System.Environment.Exit(0);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chatA.SendTo(this.remoteIpa,this.dpo,(string)chatText.Text);
            chatText.Text = "已发送";
            chatLabel.Content = chatA.allInfo;
        }
    }
}
