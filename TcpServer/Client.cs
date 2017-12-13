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
            string message = "hello\n";
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            client.GetStream().Write(buffer, 0, buffer.Length);
            //client.Close();                
        }
    }
}
