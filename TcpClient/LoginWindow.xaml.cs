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
    //class for logging clients and connecting them to server
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        //if clients wants to login
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            IPAddress serverIp;
            int serverPort;
            string userName;
            string password;
            try//parse values of all textboxes
            {
               
                serverIp = IPAddress.Parse(TbServerIP.Text);
                serverPort = Int32.Parse(TbServerPort.Text);
                userName = TbUserName.Text;
                password = TbUserPassword.Text;
            }
            catch//show error message to user
            {
                LabelLoginResult.Content = "Error with ip or port";
                return;
            }
            //try to create client object
            Client client;
            try
            {
                client = new Client(serverPort, serverIp, userName);
            }
            catch
            {
                //show error message
                LabelLoginResult.Content = "Error with creating client";
                return;
            }
            //try connect
            try
            {
                if (client.Connect()) 
                {
                    //if we connected to the server
                    //send userName and password to him
                    client.SendServiceMessage("ath:name:" + userName + ":" + "pswd:" + password);
                    //wait for answer
                    string answer = client.RecieveMessage();
                    //if answer is true
                    if (answer == "true")
                    {
                        //show main widow to user
                        MainWindow window = new MainWindow(client);
                        window.Show();
                        this.Close();
                    }
                    else
                    {
                        //close client and show error message
                        client.Close();
                        LabelLoginResult.Content = "Error with authentication";
                    }
                }
                else
                {
                    //close client and show error message
                    client.Close();
                    LabelLoginResult.Content = "Error with connection";
                }
            }
            catch
            {
                LabelLoginResult.Content = "Error with parametres";
            }
        }
        //if client wants to create new user
        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            IPAddress serverIp;
            int serverPort;
            string userName;
            string password;
            //try to connect to parse textbox values first
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
            //try to create client
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
            //try to connect to server
            try
            {
                if (client.Connect()) 
                {
                    //if we connected
                    //show registration window
                    Registration reg = new Registration();

                    reg.ShowDialog();
                    if (reg.IsOk)
                    {
                        //if user pressed refistrate button
                        string msg = reg.ResultString;
                        //send messge to server
                        client.SendServiceMessage(msg);
                        //wait for answer
                        string answer = client.RecieveMessage();
                        //if anser is true  - registration succesfull
                        if (answer == "true")
                        {
                            LabelLoginResult.Content = "Succesfull registration";
                        }
                        else
                        {
                            //show error message
                            LabelLoginResult.Content = "Error with registration. Maybe such user exists";
                        }

                    }
                }
                else
                {
                    client.Close();
                    LabelLoginResult.Content = "Error with connection";
                }
            }
            catch
            {
                LabelLoginResult.Content = "Error with parametres";
            }
                   
        }
    }
}
