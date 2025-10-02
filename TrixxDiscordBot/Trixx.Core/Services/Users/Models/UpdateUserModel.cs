namespace Trixx.Core.Services.Users.Models
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public List<int> Roles { get; set; } = [];
        public string Password { get; set; } = string.Empty;
    }
}
