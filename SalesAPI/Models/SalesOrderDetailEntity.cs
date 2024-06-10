using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesAPI.Models
{
	public class SalesOrderDetailEntity
	{
        [Key]
        public int SalesOrderDetailEntityID { get; set; }
        public int OrderQty { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public decimal LineTotal { get; set; }
        public DateTime Date { get; set; }
        public Order Order { get; set; }
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int? ProductID { get; set; }

    }
}

