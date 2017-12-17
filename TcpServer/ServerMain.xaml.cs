﻿using System;
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
        private Server _server;
        private Thread _serverThread;
        
        
        
        public MainWindow()
        {
            InitializeComponent();                    
        }
        
        private void ButtonServerOn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                IPAddress ip = IPAddress.Parse(TBserverIP.Text);
                int port = Int32.Parse(TBserverPort.Text);
                _server = new Server(ip, port);
            }
            catch
            {
                LbResult.Content = "Invalid ip or port";
                return;
            }
            try
            {
                _serverThread = new Thread(_server.StartWorking);
                _serverThread.Start();
                ButtonServerOn.IsEnabled = false;
                
            }
            catch
            {
                LbResult.Content = "Error with server running";
                _server.StopServer();
                return;
            }
            DatabaseWorker dw = new DatabaseWorker();
            dw.SaveSession();
        }
        private void EndServerWork()
        {
            if (_server != null)
            {
                _server.StopServer();
                DatabaseWorker dw = new DatabaseWorker();
                dw.SaveSessionEnd();
            }
            Environment.Exit(0);
        }
        ~MainWindow()
        {
            EndServerWork();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EndServerWork();
        }
    }
}
