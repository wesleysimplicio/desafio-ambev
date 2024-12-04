namespace Sales.Domain.Models
{
    public class ItemSaleModel
    {
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal Discount { get; set; }
        public decimal ValueTotalItem { get; set; }
        public int ItemSaleId { get; set; }
        public int ProductId { get; set; }
    }
}