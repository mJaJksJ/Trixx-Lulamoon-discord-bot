namespace Trixx.Database.Common.Models
{
    public abstract class AbstractSystemObject
    {
        public int Id { get; set; }
        public string Creator { get; set; } = string.Empty;
        public DateTime CreateDateTime { get; set; }
    }
}
