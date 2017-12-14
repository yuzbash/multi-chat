using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TcpServer
{
    class Client
    {
        private static List<TcpClient> _clientList;
        public Client(TcpClient client)
        {
            if (_clientList == null)
                _clientList = new List<TcpClient>();
            _clientList.Add(client);
            string message = WaitMessage(client);
            SendToAll(message);
            //client.Close();                
        }
        private string WaitMessage(TcpClient client)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
            byte[] bytesRecieved = new byte[bytesRead];
            string message = Encoding.UTF8.GetString(bytesRecieved).Substring(0, bytesRead);
            return message;
        }
        private void SendToAll(string message)
        {
            foreach (var client in _clientList)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }
    }
}
