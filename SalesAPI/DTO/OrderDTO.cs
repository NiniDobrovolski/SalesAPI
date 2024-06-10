namespace SalesAPI.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int AccountID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
