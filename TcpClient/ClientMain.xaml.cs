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
using System.Threading;
using System.ComponentModel;

namespace MyTcpClient
{
    //class for main window of client
    public partial class MainWindow : Window
    {
        //our client object
        Client _client;
        //background worker for recieving messages
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        //constructor to determine _client
        public MainWindow(Client client)
        {
            _client = client;
            //define work for our worker
            worker.DoWork += worker_DoWork;
            InitializeComponent();
            //run worker
            worker.RunWorkerAsync();
        }
        //function for backgroundworker
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //get TcpClient
            TcpClient client = _client.GetTcpClient();
            //for recieving bytes
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            //listen always
            while (true)
            {
                try
                {
                    //try ti recieve message from stream
                    while (bytesRead == 0)
                    {
                        bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                    }
                    //if message was recieved
                    string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
                    //with help of dispatcher write this message
                    Application.Current.Dispatcher.Invoke(new Action(() => TBchatBox.Text += message + "\n"));
                    //set bytes read to zero
                    bytesRead = 0;
                }
                catch
                {
                    //if was error with stream reading - show error message
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                                ResultLabelMain.Content =
                                                "Error with server. Try to restart tour client"));
                    //stop recieving messages
                    break;
                }
            }
            
        }
        //event to send message for server
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //try to send
                _client.SendMessage(TBmessage.Text);
            }
            catch
            {
                //if error occured
                ResultLabelMain.Content = "Error with server. Try to restart your client";
            }
        }
        
    }
}
