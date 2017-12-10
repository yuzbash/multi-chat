using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Threading;


namespace TcpServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static void ServerThread(Object StateInfo)
        {
            new Server((IPAddress)StateInfo);
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Label1.Content = textBox1.Text;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Thread thread = new Thread(new ParameterizedThreadStart(ServerThread));
            thread.Start(ip);

        }
    }
}
