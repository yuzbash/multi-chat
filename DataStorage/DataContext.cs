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
        //declare all tables of our database
        public DbSet<Users> Users { get; set; }
        public DbSet<ServerSessions> ServerSessions { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
    }
}
