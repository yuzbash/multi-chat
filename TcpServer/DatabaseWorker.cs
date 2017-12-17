using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;
using System.Data.SqlTypes;

namespace TcpServer
{
    //interdace for database worker
    interface IDatabaseWorker
    {
        //save session beggining in database
        void SaveSession();
        //save session end in database
        void SaveSessionEnd();
        //get session id of current session
        int GetCurrentSessionID();
        //save message
        void SaveMessage(string text, string userName);
        //check user password
        bool CheckUserPassword(string userName, string password);
        //check if user userName exists in database
        bool CheckUser(string userName);
        //save authentication
        void AddAuthentication(string userName);
        //save users
        void AddUser(string userName, string password, string userSurname,
                            string userPasportName, string userEmail, string userCity);
        //gets 4 tables of our database
        List<ServerSession> GetSessions();
        List<Authentication> GetAuthentications();
        List<User> GetUsers();
        List<Message> GetMessages();

    }
    
    class DatabaseWorker :IDatabaseWorker
    {
        //our database context
        private DataContext _context;
        public DatabaseWorker()
        {
            //create new context
            _context = new DataContext();
            
        }
        //saving beggining of user session
        public void SaveSession( )
        {
            //add new entry with current datetime
            _context.ServerSessions.Add(new ServerSession
            {
                BeginningDate = DateTime.UtcNow
            });
            _context.SaveChanges();
        }
        //return sessionID of current session
        public int GetCurrentSessionID()
        {
            //get list of all sessions and get session id of the last one
            List<ServerSession> sessions = _context.ServerSessions.ToList();
            int sessionsSize = sessions.Count;
            var currentSession = sessions[sessionsSize - 1];
            return currentSession.SessionID;
        }
        //saves session ending with currend date
        public void SaveSessionEnd()
        {
            //get last session in list of sessions and update entry
            List<ServerSession> sessions = _context.ServerSessions.ToList();
            int sessionsSize = sessions.Count;
            var currentSession = sessions[sessionsSize - 1];
            currentSession.EndingDate = DateTime.UtcNow;
            _context.SaveChanges();
        }
        //save message with text text of user userName
        public void SaveMessage(string text, string userName)
        {
            //get current session id
            int sessionID = GetCurrentSessionID();
            //get user with such user name
            User usr = (from user in _context.Users
                        where user.UserName == userName
                        select user).ToList()[0];
            //get session with such session id
            ServerSession ss = (from session in _context.ServerSessions
                                where session.SessionID == sessionID
                                select session).ToList()[0];
            //add and save
            _context.Messages.Add(new Message
            {
                Text = text,
                AuthorID = usr.UserID,
                SessionID = sessionID,
                User = usr,
                ServerSession = ss                
            });
            _context.SaveChanges();
        }
        //check if user with userName has password as password
        public bool CheckUserPassword(string userName, string password)
        {
            //get all users
            var users = (from user in _context.Users
                         where (user.UserName == userName) && (user.Password == password)
                         select user).ToList();
            //if no such users
            if (users.Count == 0)
                return false;
            else
                return true;                        
        }
        //check if user with such name exists
        //true - if exists
        public bool CheckUser(string userName)
        {
            //get all users with such userName
            var users = (from user in _context.Users
                         where (user.UserName == userName)
                         select user).ToList();
            if (users.Count == 0)
                return false;
            else
                return true;
        }
        //add authentication of user userName to table
        public void AddAuthentication(string userName)
        {
            //get current session id
            int sessionID = GetCurrentSessionID();
            //get session with such session id
            ServerSession session = (from ss in _context.ServerSessions
                                     where ss.SessionID == sessionID
                                     select ss).ToList()[0];
            //get user with such userName
            User user = (from usr in _context.Users
                         where usr.UserName == userName
                         select usr).ToList()[0];
            //add and save
            _context.Authentications.Add(new Authentication
            {
                SessionID = sessionID,
                UserID = user.UserID,
                User = user,
                ServerSession = session
            });
            _context.SaveChanges();
        }
        //add user with this parametres to database
        public void AddUser(string userName, string password, string userSurname,
                            string userPasportName, string userEmail, string userCity)
        {
            //add and save
            _context.Users.Add(new User
            {
                UserName = userName,
                Password = password,
                UserSurname = userSurname,
                UserPassportName = userPasportName,
                UserEmail = userEmail, 
                UserCity = userCity
            });
            _context.SaveChanges();
        }

        //get sessions from database
        public List<ServerSession> GetSessions()
        {
            return _context.ServerSessions.ToList();
        }
        //get messages from database
        public List<Message> GetMessages()
        {
            return _context.Messages.ToList();
        }
        //get users from database
        public List<User> GetUsers()
        {
            //_context.InitializeUsers(); 
            return _context.Users.ToList();
        }
        //get authentications ffrom database
        public List<Authentication> GetAuthentications()
        {
            return _context.Authentications.ToList();
        }
    }
}
