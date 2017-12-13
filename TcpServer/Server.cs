using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace TcpServer
{
    class Server
    {

        //oblect for tcp clients
        private TcpListener _tcpListener;

        
        public Server(IPAddress iPAddress)
        {
            int port = 10000;
            //create listener
            _tcpListener = new TcpListener(iPAddress, port);
            _tcpListener.Start();
            while(true)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                //create thread
                Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                //start thread
                thread.Start(client);
            }
        }
        static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }
        
        ~Server()
        {
            //if listener was created
            if (_tcpListener != null)
            {
                //stop him
                _tcpListener.Stop();
            }
        }
    }
}
