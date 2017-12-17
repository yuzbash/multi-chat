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
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        public MainWindow(Client client)
        {
            _client = client;
            worker.DoWork += worker_DoWork;
            InitializeComponent();
            worker.RunWorkerAsync();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            TcpClient client = _client.GetTcpClient();
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            while (true)
            {
                try
                {
                    while (bytesRead == 0)
                    {
                        bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                    }
                    string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
                    Application.Current.Dispatcher.Invoke(new Action(() => TBchatBox.Text += message + "\n"));
                    bytesRead = 0;
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                                ResultLabelMain.Content =
                                                "Error with server. Try to restart tour client"));
                    break;
                }
            }
            
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _client.SendMessage(TBmessage.Text);
            }
            catch
            {
                ResultLabelMain.Content = "Error with server. Try to restart your client";
            }
        }
        
    }
}
