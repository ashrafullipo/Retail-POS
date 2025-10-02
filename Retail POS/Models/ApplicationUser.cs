using Microsoft.AspNetCore.Identity;

namespace Retail_POS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
