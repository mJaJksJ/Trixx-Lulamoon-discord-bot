namespace Trixx.Core.Services.Users.Models
{
    public class UserManuallyCreateModel
    {
        public string UserName { get; set; } = string.Empty;
        public List<int> Roles { get; set; } = [];
        public string Password { get; set; } = string.Empty;
    }
}
