using Microsoft.AspNetCore.Identity;

namespace Trixx.Database.Models.Identity
{
    public sealed class TrixxUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public List<TrixxUserRole> Roles { get; set; }
    }
}
