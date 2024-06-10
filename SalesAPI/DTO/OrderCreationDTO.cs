namespace SalesAPI.DTO
{
    public class OrderCreationDTO
    {
        public int? OrderID { get; set; }
        public int AccountID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
