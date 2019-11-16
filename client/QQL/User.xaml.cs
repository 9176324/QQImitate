
using System;
using System.Threading;

using System.Windows;

using System.Windows.Input;


namespace QQL
{
    /// <summary>
    /// user.xaml 的交互逻辑
    /// </summary>
    public partial class user : Window
    {
        private static QQL.source.Friend friend = new source.Friend();
        private static source.ChatA chatA = new source.ChatA();
        private Thread t = new Thread(friend.clienGet);
        private Thread p = new Thread(friend.friendRefresh);
        private Thread tpp = new Thread(chatA.clientlistener);

        public user()
        {
            InitializeComponent();
            
            friend.clienPush();
            p.IsBackground = true;
            t.IsBackground = true;
            tpp.IsBackground = true;
            tpp.Start();
            t.Start();
            p.Start(this);

        }
        ~user()
        {

            tpp.Abort();
            p.Abort();
            t.Abort();
        }
        protected override void OnClosed(EventArgs e)
        {
            tpp.Abort();
            p.Abort();
            t.Abort();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            //Application.Current.Exit = 0;
            base.OnClosed(e);
            Environment.Exit(0);
        }
        public void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //int ppor = int.Parse(labelCo.Content.ToString().Split('|')[1]);
            string strl = labelCo.Content.ToString().Split(':')[0];
            Chat chat = new Chat(chatA ,strl,4455 );
            chat.Title = labelCo.Content.ToString().Split(':')[0] + ":" +labelCo.Content.ToString().Split('|')[1];
            chat.chatLabel.Content = chatA.allInfo;
            chat.Show();
        }
    }
}
