using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage
{
    public class Authentication
    {
        //primary ley
        [Key]
        public int AuthenticationID { get; set; }
        //session id
        //reference to ServerSessions table
        [ForeignKey("ServerSessions"), Required]
        public int SessionID { get; set; }
        //user id of author
        [ForeignKey("Users"), Required]
        public int UserID { get; set; }

        public virtual ServerSessions Session { get; set; }
        public virtual Users Author { get; set; }
    }
}
