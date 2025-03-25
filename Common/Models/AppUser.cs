using Microsoft.AspNetCore.Identity;

namespace Common.Models
{
    public class AppUser : IdentityUser
    {
        public Consumer Consumer { get; set; }
    }
}
