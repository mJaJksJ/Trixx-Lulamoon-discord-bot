using Trixx.Database.Enums;
using Trixx.Database.Models.FormDefaults.Types;

namespace Trixx.Database.Models.FormDefaults
{
    public class FormDeafault
    {
        public FormDefaultType Type { get; set; }
#pragma warning disable CS8618
        public FormDeafaultCartoon Cartoon { get; set; }
#pragma warning restore CS8618
    }
}
