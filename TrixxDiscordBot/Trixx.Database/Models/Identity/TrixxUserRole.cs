using Microsoft.AspNetCore.Identity;

namespace Trixx.Database.Models.Identity
{
    public sealed class TrixxUserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public TrixxUser User { get; set; } = new TrixxUser();
        public TrixxRole Role { get; set; } = new TrixxRole();
    }
}
