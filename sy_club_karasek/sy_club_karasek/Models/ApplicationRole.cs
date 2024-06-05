using Microsoft.AspNetCore.Identity;

namespace club.soundyard.web.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name, string agreement) : base(name)
        {
            this.Agreement = agreement;
        }
       

        public string? Agreement { get; set; }
    }
}
