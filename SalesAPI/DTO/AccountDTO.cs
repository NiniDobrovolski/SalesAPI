using System.ComponentModel.DataAnnotations;
namespace SalesAPI.Models
{
	public class AccountDTO
	{
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

