using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TcpServer
{
    //interface for client class
    interface IClient
    {
        //send to all function
        void SendToAll(string message);
        //send to one client
        void SendToOne(string message, TcpClient client);
        //wait message
        string WaitMessage(TcpClient client);
    }
    class Client : IClient
    {
        //list of clients
        private static List<TcpClient> _clientList;
        //number of sembols in my simple protocol - 4 ("reg:", "msg:" or "ath:")
        private const int _codeLength = 4;
        public Client(TcpClient client)
        {
            //create new list if he is empty
            if (_clientList == null)
                _clientList = new List<TcpClient>();
            //add our client
            _clientList.Add(client);
            //create new dataBase worker
            DatabaseWorker dw = new DatabaseWorker();
            while (true)
            {
                //wait message from client
                string message = WaitMessage(client);
                //if we recieved usual message with text
                if (ShowRecieveOperation(message) == "msg:")
                {
                    string text;
                    string user;
                    //parse text and user name
                    ParseMessage(message,out text,out user);
                    //save to database
                    dw.SaveMessage(text, user);
                    //send message to all users
                    SendToAll(message.Substring(_codeLength, message.Length - _codeLength));
                }
                //if our recieved message is message with authentication
                else if(ShowRecieveOperation(message) == "ath:")
                {
                    string userName;
                    string password;
                    //parse user name and password
                    ParseAuthenticationMessage(message, out userName, out password);
                    //if they are not empty
                    if (userName != "" && password != "")
                    {
                        //check password and user name
                        bool result = dw.CheckUserPassword(userName, password);
                        if (result)
                        {
                            //if true sent "true" to this client
                            SendToOne("true", client);
                            //adding ta database
                            dw.AddAuthentication(userName);
                        }
                        //send "false" in all other cases
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
                //if we recieved message with registration
                else if(ShowRecieveOperation(message) == "reg:")
                {
                    string userName;
                    string password;
                    string userSurname;
                    string userPassportName;
                    string userEmail;
                    string userCity;
                    //parse this message
                    ParseRegistrationString(message,out userName,out password, out userSurname,
                                                out userPassportName,out userEmail,out userCity);
                    //check if such user name is not exist
                    if (dw.CheckUser(userName))
                    {
                        SendToOne("false", client);
                    }
                    //else send "true" to this client
                    else
                    {
                        SendToOne("true", client);
                        //add to database
                        dw.AddUser(userName, password, userSurname, userPassportName, userEmail, userCity);
                    }
                }
            }               
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
        //waiting message from client
        public string WaitMessage(TcpClient client)
        {
            //declare byte array for recieve
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int bytesRead = 0;
            //while we read 0 bytes
            while (bytesRead == 0)
            {
                try//try to get bytes from stream
                {
                    bytesRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                }
                catch
                {
                    continue;
                }
            }
            //if we recieved more than 0 bytes - endode it to string and return
            string message = Encoding.UTF8.GetString(bytes).Substring(0, bytesRead);
            return message;
        }
        //sending message to all clients in _clientList
        public void SendToAll(string message)
        {
            //list for latter removing
            List<TcpClient> removeList = new List<TcpClient>();
            //run with all clients in our list
            foreach (var client in _clientList)
            {
                //if client is connected now
                if (IsConnected(client))
                {
                    //write mesage for him
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    client.GetStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //else add this client to remove list
                    removeList.Add(client);
                }
            }
            //remove all clients in removeList from _clientList
            foreach(var client in removeList)
            {
                _clientList.Remove(client);
            }
            removeList.Clear();
        }
        //sending message to client
        public void SendToOne(string message, TcpClient client)
        {
            //if client is connected
            if (IsConnected(client))
            {
                //send message to him
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
            else//remove this client from _clientList
            {
                _clientList.Remove(client);
            }
        }
        //return the first 4 symbols
        private string ShowRecieveOperation(string recieveMessage)
        {
            return recieveMessage.Substring(0, _codeLength);
        }
        //get Username and password from recieve message
        private void ParseAuthenticationMessage(string recieveMessage, out string name, out string password)
        {
            name = "";
            password = "";
            //split recieved message
            string[] parseString = recieveMessage.Split(':');
            for (int i = 0; i < parseString.Length; i++)
            {
                //save name and password
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
        //save text and userName from message with text
        private void ParseMessage(string recieveMessage, out string text, out string userName)
        {
            text = "";
            userName = "";
            //split, find and save
            string[] parseString = recieveMessage.Split(':');
            userName = parseString[1];
            for (int i = 2; i < parseString.Length; i++)
            {
                text += parseString[i];
            }
        }
        //save all parametres of new client in message with registration
        private void ParseRegistrationString(string recieveMessage,out string userName,
                                            out string password, out string userSurname,
                                            out string userPasportName, out string userEmail,
                                            out string userCity)
        {
            userName = "";
            password = "";
            userSurname = "";
            userPasportName = "";
            userEmail = "";
            userCity = "";
            //split, find and save
            string[] parseArray = recieveMessage.Split(':');

            for (int i = 0; i < parseArray.Length; i++)
            {
                if (parseArray[i] == "user" && userName == "")
                    userName = parseArray[i + 1];
                if (parseArray[i] == "pswd" && password == "")
                    password = parseArray[i + 1];
                if (parseArray[i] == "srnm" && userSurname == "")
                    userSurname = parseArray[i + 1];
                if (parseArray[i] == "pasp" && userPasportName == "")
                    userPasportName = parseArray[i + 1];
                if (parseArray[i] == "emal" && userEmail == "")
                    userEmail = parseArray[i + 1];
                if (parseArray[i] == "city" && userCity == "")
                    userCity = parseArray[i + 1];
            }
        }
    }
}
