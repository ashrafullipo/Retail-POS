using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Retail_POS.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    }
}
