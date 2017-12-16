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
        Client _client;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Client client)
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {

            Client client = new Client(10000, this);
            if (client.Connect())
            {
                client.SendMessage(TBmessage.Text);
                TBchatBox.Text += client.RecieveMessage();
            }
            client.Close();
        }
    }
}
