using System.ComponentModel.DataAnnotations;

namespace AzureCustomerOPeration.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set;  }
    }
}
