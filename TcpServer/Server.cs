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
        TcpListener tcpListener;
         
        public Server(IPAddress iPAddress)
        {
            int port = 10000;
            //create listener
            tcpListener = new TcpListener(iPAddress, port);
            tcpListener.Start();
            while(true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
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
            if (tcpListener != null)
            {
                //stop him
                tcpListener.Stop();
            }
        }
    }
}
