using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.LoginSystem
{
    public partial class UserLogin
    {
        [Key]
        public int UserId { get; set; }
        public string UserAccount { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Salt { get; set; }
        public string UserRole { get; set; }
        public int IsEmailAuthenticated { get; set; }
        public string AuthCode { get; set; }
    }
}
