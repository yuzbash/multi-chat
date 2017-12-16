using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorage;

namespace TcpServer
{
    
    class DatabaseWorker
    {
        private DataContext _context;
        public DatabaseWorker()
        {
            _context = new DataContext();
            //_context.InitializeUsers(); 
        }
        public void SaveSession( )
        {
            _context.ServerSessions.Add(new ServerSession
            {
                BeginningDate = DateTime.Now
            });
            _context.SaveChanges();
        }
        public int GetCurrentSessionID()
        {
            List<ServerSession> sessions = _context.ServerSessions.ToList();
            int sessionsSize = sessions.Count;
            var currentSession = sessions[sessionsSize - 1];
            return currentSession.SessionID;
        }
        public void SaveSessionEnd()
        {
            List<ServerSession> sessions = _context.ServerSessions.ToList();
            int sessionsSize = sessions.Count;
            var currentSession = sessions[sessionsSize - 1];
            currentSession.EndingDate = DateTime.Now;
            _context.SaveChanges();
        }
        public void SaveMessage(string text, string userName, int sessionID)
        {
            User usr = (from user in _context.Users
                        where user.UserName == userName
                        select user).ToList()[0];
            ServerSession ss = (from session in _context.ServerSessions
                                where session.SessionID == sessionID
                                select session).ToList()[0];
            _context.Messages.Add(new Message
            {
                Text = text,
                AuthorID = usr.UserID,
                SessionID = sessionID,
                User = usr,
                ServerSession = ss                
            });
        }
        public bool CheckUserPassword(string userName, string password)
        {
            var users = (from user in _context.Users
                         where (user.UserName == userName) && (user.Password == password)
                         select user).ToList();
            if (users.Count == 0)
                return false;
            else
                return true;                        
        }
        public bool CheckUser(string userName)
        {
            var users = (from user in _context.Users
                         where (user.UserName == userName)
                         select user).ToList();
            if (users.Count == 0)
                return false;
            else
                return true;
        }
        public void AddAuthentication(string userName)
        {
            int sessionID = GetCurrentSessionID();
            ServerSession session = (from ss in _context.ServerSessions
                                     where ss.SessionID == sessionID
                                     select ss).ToList()[0];
            User user = (from usr in _context.Users
                         where usr.UserName == userName
                         select usr).ToList()[0];
            _context.Authentications.Add(new Authentication
            {
                SessionID = sessionID,
                UserID = user.UserID,
                User = user,
                ServerSession = session
            });
            _context.SaveChanges();
        }
        public void AddUser(string userName, string password, string userSurname,
                            string userPasportName, string userEmail, string userCity)
        {
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
    }
}
