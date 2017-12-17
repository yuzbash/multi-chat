using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;

namespace MyTcpClient
{
    public class Client
    {
        private string _userName;
        private int _serverPort;
        private IPAddress _clientAddress = IPAddress.Parse("127.0.0.1");
        private IPAddress _serverAddress;
        private TcpClient _client;       
        public Client(int serverPort, IPAddress serverIP, string userName)
        {
            _serverPort = serverPort;
            _serverAddress = serverIP;
            _userName = userName;
        }
        
        public TcpClient GetTcpClient()
        {
            return _client;
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
                //_window.Label1.Content = "Ошибка подключения клиента";
                return false;
            }
            return true;
        }
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("msg:" +_userName + ":" + message);
            _client.GetStream().Write(buffer, 0, buffer.Length);
        }
        public void SendServiceMessage(string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _client.GetStream().Write(buffer, 0, buffer.Length);
        }
        public string RecieveMessage()
        {
            byte[] bytes = new byte[_client.ReceiveBufferSize];
            int bytesRead = 0;
            while (bytesRead == 0)
            {
                bytesRead = _client.GetStream().Read(bytes, 0, _client.ReceiveBufferSize);
            }
            string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
            return message;
        }
        
        public void Close()
        {
            _client.Close();
        }
    }
}
