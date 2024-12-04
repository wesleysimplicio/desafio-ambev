using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Domain.Entities
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumberSale { get; set; } = string.Empty;

        [Required]
        public DateTime DateSale { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValueTotal { get; set; }

        [Required]
        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string StatusSale { get; set; } = "Não Cancelado";

        public List<ItemSale> Itens { get; set; } = new();
    }
}
