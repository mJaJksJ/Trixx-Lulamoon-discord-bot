using Trixx.Common.Models;

namespace TrixxCore.Services.Users.Models
{
    public class UserListItem: SelectItem
    {
        public bool IsLocked { get; set; }
        public bool IsLockable { get; set; }
    }
}
