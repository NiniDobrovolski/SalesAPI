namespace SalesAPI.DTO
{
    public class OrderDetailDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int OrderQty { get; set; }
        public decimal LineTotal { get; set; }
    }
}
