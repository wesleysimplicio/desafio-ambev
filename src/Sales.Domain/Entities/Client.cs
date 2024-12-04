using System.ComponentModel.DataAnnotations;

namespace Sales.Domain.Entities
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(255)]
        public string NameClient { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public List<Sale> Sales { get; set; } = new();
    }
}