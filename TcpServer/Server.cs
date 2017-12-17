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
    //interface for server class
    interface IServer
    {
        //start working
        void StartWorking();
        //stop listening
        void StopServer();
    }
    class Server : IServer
    {

        //oblect for tcp clients
        private TcpListener _tcpListener;
        //list of threads for each connected client
        private List<Thread> _myThreads;
        //constructor
        public Server(IPAddress iPAddress, int port)
        {
            //create listener
            _tcpListener = new TcpListener(iPAddress, port);
            //declare threadlist
            _myThreads = new List<Thread>();
        }
        //start working
        public void StartWorking()
        {
            //start lisnet new clients
            _tcpListener.Start();
            while (true)
            {
                try//try accept new client
                {
                    
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    //create thread for each new client
                    Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                    _myThreads.Add(thread);
                    //start thread
                    thread.Start(client);
                }
                catch //if some error occured - go next iteration
                {
                    continue;
                }
            }
        }
        //method for client thread
        static void ClientThread(Object StateInfo)
        {
            //create new client
            new Client((TcpClient)StateInfo);
        }
        //stopping server
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
