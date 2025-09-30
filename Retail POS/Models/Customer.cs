using System.ComponentModel.DataAnnotations;

namespace Retail_POS.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
