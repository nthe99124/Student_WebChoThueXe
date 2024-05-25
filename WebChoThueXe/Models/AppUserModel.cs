using Microsoft.AspNetCore.Identity;

namespace WebChoThueXe.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Occuapation { get; set; }
    }
}
