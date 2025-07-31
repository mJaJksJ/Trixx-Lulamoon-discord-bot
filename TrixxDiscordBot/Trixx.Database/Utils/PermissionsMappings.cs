using Trixx.Database.Attributes;
using Trixx.Database.Enums;
using Trixx.Common.Utils;

namespace Trixx.Database.Utils
{
    public static class PermissionsMappings
    {
        public static IEnumerable<PermissionLine> Lines => Enum.GetValues(typeof(Permission))
            .Cast<Permission>()
            .Select(x => new PermissionLine(x)).ToList()
            .AsReadOnly();

        public sealed class PermissionLine
        {
            public PermissionLine(Permission x)
            {
                Permission = x;
                var mapping = x.TryGetCustomAttribute<PermissionMappingAttribute>();
                Workscreen = mapping.Workscreen;
            }

            public Permission Permission { get; }
            public Workscreen Workscreen { get; }
        }
    }
}
