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
                
        public Server(IPAddress iPAddress, int port)
        {
            //create listener
            _tcpListener = new TcpListener(iPAddress, port);            
        }
        public void StartWorking()
        {
            _tcpListener.Start();
            while (true)
            {
                try
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    //create thread
                    Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                    //start thread
                    thread.Start(client);
                }
                catch
                {
                    continue;
                }
            }
        }
        static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }
        public void StopServer()
        {
            //if listener was created
            if (_tcpListener != null)
            {
                //stop him
                _tcpListener.Stop();
            }
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
