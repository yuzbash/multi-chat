using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MyTcpClient
{
    class Client
    {
        static int _clientPort;
        private int _serverPort;
        private IPAddress _clientAddress = IPAddress.Parse("127.0.0.1");
        private IPAddress _serverAddress = IPAddress.Parse("127.0.0.1");
        private MainWindow _window;
        private TcpClient _client;
        public Client(int serverPort, MainWindow window)
        {
            _serverPort = serverPort;
            _window = window;
            
        }
        public bool Connect()
        {
            Random rnd = new Random();
            while (true)
            {
                int rand = rnd.Next(-1000, 1001);
                try
                {
                    IPEndPoint myEndPoint = new IPEndPoint(_clientAddress, rand + 10000);
                    _client = new TcpClient(myEndPoint);
                    break;
                }
                catch
                {
                    continue;
                }
            }
        
            IPEndPoint serverEndPoint = new IPEndPoint(_serverAddress, _serverPort);
            
            try
            {
                _client.Connect(serverEndPoint);
            }
            catch
            {
                _window.Label1.Content = "Ошибка подключения клиента";
                return false;
            }
            return true;
        }
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _client.GetStream().Write(buffer, 0, buffer.Length);
        }
        public string RecieveMessage()
        {
            byte[] bytes = new byte[_client.ReceiveBufferSize];
            int bytesRead = _client.GetStream().Read(bytes, 0, _client.ReceiveBufferSize);
            byte[] bytesRecieved = new byte[bytesRead];
            string message = Encoding.UTF8.GetString(bytesRecieved).Substring(0, bytesRead);
            _window.Label1.Content = message;
            return message;
        }
        public void Close()
        {
            _client.Close();
        }
    }
}
