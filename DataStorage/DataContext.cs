using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace DataStorage
{
    public class DataContext : DbContext
    {
        public DataContext():base("DBConnection")
        {
            Database.SetInitializer(new Initializer());
        }
        //declare all tables of our database
        public DbSet<User> Users { get; set; }
        public DbSet<ServerSession> ServerSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public void InitializeUsers()
        {
            Users.AddOrUpdate(new User {
                UserName = "user1",
                UserSurname = "Ivanov",
                UserPassportName = "Ivan",
                UserCity = "Moscow",
                UserEmail = "ivanov@mail.ru",
                Password = "user1"
            });
            Users.AddOrUpdate(new User
            {
                UserName = "user2",
                UserSurname = "Petrov",
                UserPassportName = "Artem",
                UserCity = "Kiev",
                UserEmail = "petrov@mail.ru",
                Password = "12345"
            });
            Users.AddOrUpdate(new User
            {
                UserName = "user3",
                UserSurname = "Sidorov",
                UserPassportName = "Egor",
                UserCity = "Moscow",
                UserEmail = "sidorov@mail.ru",
                Password = "12345"
            });
            SaveChanges();
        }
    }
}
