using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int AccountID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CarrierTrackingNumber { get; set; }
        

        public ICollection<SalesOrderDetailEntity> SalesOrderDetails { get; set; }
    }

}

