using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
	public class Product
	{
		public int Price { get; set; }
        [Key]
        public int ProductID { get; set; }
		public int Qty { get; set; }
		public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}

