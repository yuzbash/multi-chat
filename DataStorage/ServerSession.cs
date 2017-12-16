using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataStorage
{
    //class for storing server sesseions and there dates
    public class ServerSession
    {
        //session id
        [Key]
        public int SessionID { get; set; }
        //date of the session beggining
        [Required]
        public DateTime BeginningDate { get; set; }
        //date of the session beggining
        public DateTime? EndingDate { get; set; }
    }
}
