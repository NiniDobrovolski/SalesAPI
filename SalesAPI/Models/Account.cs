using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
	public class Account
	{
        [Key]
        public int AccountNumber { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }

    }
}

