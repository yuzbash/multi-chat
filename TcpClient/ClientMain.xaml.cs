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
using System.Net.Sockets;
namespace MyTcpClient
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

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int myPort = 11000;
            int serverPort = 10000;
            IPEndPoint myEndPoint = new IPEndPoint(ip, myPort);
            IPEndPoint serverEndPoint = new IPEndPoint(ip, serverPort);
            TcpClient client = new TcpClient(myEndPoint);
            client.Connect(serverEndPoint);
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
            byte[] bytesRecieved = new byte[bytesRead];
            bytesRecieved = bytes;
            string message = Encoding.UTF8.GetString(bytesRecieved);
            Label1.Content = message;
            client.Close();
        }
    }
}
