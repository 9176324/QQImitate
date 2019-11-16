using System.Net.Sockets;
using System.Windows;
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
            user user = new user();
            user.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            this.Close();
        }
    }
}
