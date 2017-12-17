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
        //our server
        private Server _server;
        //our thread for server work
        private Thread _serverThread;
        //constructor        
        public MainWindow()
        {
            InitializeComponent();                    
        }
        //on server on button
        private void ButtonServerOn_Click(object sender, RoutedEventArgs e)
        {
            //try to parse values and create server
            try
            {
                IPAddress ip = IPAddress.Parse(TBserverIP.Text);
                int port = Int32.Parse(TBserverPort.Text);
                _server = new Server(ip, port);
            }
            catch//if error with arguments
            {
                LbResult.Content = "Invalid ip or port";
                return;
            }
            try//try run new thread with server
            {
                _serverThread = new Thread(_server.StartWorking);
                _serverThread.Start();
                ButtonServerOn.IsEnabled = false;
                ButtonServerOff.IsEnabled = true;
                
            }
            catch//inform user in bad case
            {
                LbResult.Content = "Error with server running";
                _server.StopServer();
                return;
            }
            //create new database worker and save session begin
            DatabaseWorker dw = new DatabaseWorker();
            dw.SaveSession();
        }
        //ending server work
        private void EndServerWork()
        {
            try
            {
                //stop listening
                if (_server != null)
                {
                    _server.StopServer();
                    //save session end to database
                    DatabaseWorker dw = new DatabaseWorker();
                    dw.SaveSessionEnd();
                }
               
            }
            catch
            {
                //inform user in bad case
                LbResult.Content = "Error with finish";
            }
            
        }
        //destructor epty cause all server off procedure is done with event MainWindow_Closing
        ~MainWindow()
        {           
        }
        //on closing window
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //end server work if he is on
            if (ButtonServerOff.IsEnabled)
                EndServerWork();
            //kill all threads and exit
            Environment.Exit(0);
        }

        private void ButtonShowSession_Click(object sender, RoutedEventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker();
            databaseInfo.ItemsSource = dw.GetSessions();
        }

        private void ButtonShowMessages_Click(object sender, RoutedEventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker();
            databaseInfo.ItemsSource = dw.GetMessages();
        }

        private void ButtonShowUsers_Click(object sender, RoutedEventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker();
            databaseInfo.ItemsSource = dw.GetUsers();
        }

        private void ButtonShowAuthentication_Click(object sender, RoutedEventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker();
            databaseInfo.ItemsSource = dw.GetAuthentications();
        }

        private void ButtonServerOff_Click(object sender, RoutedEventArgs e)
        {
            EndServerWork();
            ButtonServerOff.IsEnabled = false;
            ButtonServerOn.IsEnabled = true;
        }
    }
}
