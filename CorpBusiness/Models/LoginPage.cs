using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CorpBusiness.Models;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CorpBusiness.Models
{
    public class LoginPage
    {
        //public int Id { get; set; }
        [Required]
        [DisplayName("User Id")]
        public string UserID { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
    public class AdminLoginPage
    {
        //public int Id { get; set; }
        [Required]
        [DisplayName("User Id")]
        public string UserID { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}