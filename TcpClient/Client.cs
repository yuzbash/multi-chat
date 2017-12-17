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
    interface IClient
    {
        //connect to server
        bool Connect();
        //send message to server
        void SendMessage(string message);
        //returns TCPclient object from our class
        TcpClient GetTcpClient();
        //send service message to server (registration or authentication)
        void SendServiceMessage(string message);
        //recieve message from server
        string RecieveMessage();
        //close client
        void Close();

    }
    //class for sending and recieving messages from server
    public class Client : IClient
    {
        //client user name
        private string _userName;
        //server port
        private int _serverPort;
        //client ip address (default local)
        private IPAddress _clientAddress = IPAddress.Parse("127.0.0.1");
        //server ip address
        private IPAddress _serverAddress;
        //TcpClient object
        private TcpClient _client;
        //constructor. define some values
        public Client(int serverPort, IPAddress serverIP, string userName)
        {
            _serverPort = serverPort;
            _serverAddress = serverIP;
            _userName = userName;
        }
        //get tcpClient object from our class
        public TcpClient GetTcpClient()
        {
            return _client;
        }
        //connect to server
        //returns true if connection was succesfull
        public bool Connect()
        {
            //random object
            Random rnd = new Random();
            //while we can't create client
            //for example, ports can busy
            while (true)
            {
                //random some int value
                int rand = rnd.Next(-1000, 1001);
                try
                {
                    //create end point with our ip and ransom port
                    IPEndPoint myEndPoint = new IPEndPoint(_clientAddress, rand + 10000);
                    //create new tcpClient
                    _client = new TcpClient(myEndPoint);
                    break;
                }
                catch//go to next iteretion
                {
                    continue;
                }
            }
            //create server endPoint
            IPEndPoint serverEndPoint;
            try
            {
                serverEndPoint = new IPEndPoint(_serverAddress, _serverPort);
            }
            catch
            {
                return false;
            }
            //tyr to connect
            try
            {
                _client.Connect(serverEndPoint);
            }
            catch//return false if error occured
            {
                return false;
            }
            //in other cases return true
            return true;
        }
        //send message to server
        public void SendMessage(string message)
        {
            //convert to bytes and send
            //write "msg:" first
            byte[] buffer = Encoding.ASCII.GetBytes("msg:" +_userName + ":" + message);
            _client.GetStream().Write(buffer, 0, buffer.Length);
        }
        //send service message (without "msg:" first)
        public void SendServiceMessage(string message)
        {
            //convert to bytes and send
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _client.GetStream().Write(buffer, 0, buffer.Length);
        }
        //recieve message from client
        
        public string RecieveMessage()
        {
            //get bytes from stream, convert and return
            byte[] bytes = new byte[_client.ReceiveBufferSize];
            int bytesRead = 0;
            //wait message
            while (bytesRead == 0)
            {
                bytesRead = _client.GetStream().Read(bytes, 0, _client.ReceiveBufferSize);
            }
            //if we recieved some byres - convert them
            string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
            //return message
            return message;
        }
        //close Tcp client
        public void Close()
        {
            _client.Close();
        }
    }
}
