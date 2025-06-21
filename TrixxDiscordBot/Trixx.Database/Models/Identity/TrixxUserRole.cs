using Microsoft.AspNetCore.Identity;

namespace Trixx.Database.Models.Identity
{
    public sealed class TrixxUserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
#pragma warning disable CS8618
        public TrixxUser User { get; set; }
        public TrixxRole Role { get; set; }
#pragma warning restore CS8618
    }
}
