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
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace MyTcpClient
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            IPAddress serverIp;
            int serverPort;
            string userName;
            string password;
            try
            {
                serverIp = IPAddress.Parse(TbServerIP.Text);
                serverPort = Int32.Parse(TbServerPort.Text);
                userName = TbUserName.Text;
                password = TbUserPassword.Text;
            }
            catch
            {
                LabelLoginResult.Content = "Error with ip or port";
                return;
            }
            Client client;
            try
            {
                client = new Client(serverPort, serverIp, userName);
            }
            catch
            {
                LabelLoginResult.Content = "Error with creating client";
                return;
            }
            
            if (client.Connect()) // and can login in future
            {
                client.SendMessage("ath:name:" + userName + ":" + "pswd:" + password);
                string answer = client.RecieveMessage();
                if (answer == "true")
                {
                    MainWindow window = new MainWindow(client);
                    window.Show();
                    this.Close();
                }
                else
                {
                    client.Close();
                }
            }
            else
            {
                client.Close();
                LabelLoginResult.Content = "Error with connection";                
            }
        }
    }
}
