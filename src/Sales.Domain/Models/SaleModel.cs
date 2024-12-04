namespace Sales.Domain.Models
{
    public class SaleModel
    {
        public int IdSale { get; set; }
        public string NumberSale { get; set; } = string.Empty;
        public DateTime DateSale { get; set; }
        public string Client { get; set; } = string.Empty;
        public int IdClient { get; set; }
        public decimal ValueTotal { get; set; }
        public string Branch { get; set; } = string.Empty;
        public int IdBranch { get; set; }
        public string StatusSale { get; set; } = "Não Cancelado";
        public List<ItemSaleModel> Itens { get; set; } = new();
    }
}
