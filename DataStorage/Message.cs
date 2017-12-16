using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage
{
    //class for storing messages
    public class Message
    {
        //message id
        [Key]
        public int MessageID { get; set; }
        //message text
        [Required]
        public string Text { get; set; }
        //session id
        //reference to ServerSessions table
        [ForeignKey("ServerSession"), Required]
        public int SessionID { get; set; }
        //user id of author
        [ForeignKey("User"), Required]
        public int AuthorID { get; set;}

        public virtual ServerSession ServerSession { get; set; }
        public virtual User User { get; set; }
    }
}
