using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataStorage
{
    //class for storing users and there passwords
    public class User
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
        //user's surname
        [Required]
        public string UserSurname { get; set; }
        //user's passport name
        [Required]
        public string UserPassportName { get; set; }
        //user's email
        [Required]
        public string UserEmail { get; set; }
        //user's city
        public string UserCity { get; set; }
    }
}
