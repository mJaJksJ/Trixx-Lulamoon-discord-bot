using Microsoft.AspNetCore.Identity;
using Trixx.Database.Enums;

namespace Trixx.Database.Models.Identity
{
    public sealed class TrixxRole : IdentityRole<int>
    {
        public static int ID_ADMIN => 1;

        public bool IsReadOnly { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
