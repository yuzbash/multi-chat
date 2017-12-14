using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataStorage
{
    //class for storing users and there passwords
    public class Users
    {
        //user id
        [Key]
        public int UserID { get; set; }
        //user login name
        [Required]
        public string UserName { get; set; }
        //user password
        [Required]
        public string Password { get; set; }
        //true if user is admibn
        [Required]
        public bool IsAdmin { get; set; }
    }
}
