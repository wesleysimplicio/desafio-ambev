using System.ComponentModel.DataAnnotations;

namespace Sales.Domain.Entities
{
    public class Branch
    {

        [Key]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(255)]
        public string NameBranch { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        public List<Sale> Sales { get; set; } = new();
    }
}