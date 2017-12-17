﻿using System;
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
        private const int _codeLength = 4;
        public Client(TcpClient client)
        {
            if (_clientList == null)
                _clientList = new List<TcpClient>();
            _clientList.Add(client);
            while (true)
            {
                string message = WaitMessage(client);
                if (ShowRecieveOperation(message) == "msg:")
                {
                    SendToAll(message.Substring(_codeLength, message.Length - _codeLength));
                }
                else if(ShowRecieveOperation(message) == "ath:")
                {
                    string userName;
                    string password;
                    ParseAuthenticationMessage(message, out userName, out password);
                    if (userName != "" && password != "")
                    {
                        DatabaseWorker dw = new DatabaseWorker();
                        bool result = dw.CheckUserPassword(userName, password);
                        if (result)
                        {
                            SendToOne("true", client);
                        }
                        else
                        {
                            SendToOne("false", client);
                        }
                    }
                    else
                    {
                        SendToOne("false", client);
                    }

                }
            }
            //client.Close();                
        }

        //check if TcpClient is connected now
        private bool IsConnected(TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private string WaitMessage(TcpClient client)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            while (bytesRead == 0)
            {
                try
                {
                    bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                }
                catch
                {
                    continue;
                }
            }
            string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
            return message;
        }
        private void SendToAll(string message)
        {
            List<TcpClient> removeList = new List<TcpClient>();
            foreach (var client in _clientList)
            {
                if (IsConnected(client))
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    client.GetStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    removeList.Add(client);
                }
            }
            foreach(var client in removeList)
            {
                _clientList.Remove(client);
            }
            removeList.Clear();
        }
        private void SendToOne(string message, TcpClient client)
        {
            if (IsConnected(client))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
            else
            {
                _clientList.Remove(client);
            }
        }
        private string ShowRecieveOperation(string recieveMessage)
        {
            return recieveMessage.Substring(0, _codeLength);
        }
        private void ParseAuthenticationMessage(string recieveMessage, out string name, out string password)
        {
            name = "";
            password = "";
            string[] parseString = recieveMessage.Split(':');
            for (int i = 0; i < parseString.Length; i++)
            {
                if (parseString[i] == "name")
                {
                    name = parseString[i + 1];
                }
                if (parseString[i] == "pswd")
                {
                    password = parseString[i + 1];
                }
            }
        }
    }
}
