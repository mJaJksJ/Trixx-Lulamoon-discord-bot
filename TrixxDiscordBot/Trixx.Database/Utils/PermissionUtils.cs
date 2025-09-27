using Trixx.Database.Attributes;
using Trixx.Database.Enums;
using Trixx.Common.Utils;

namespace Trixx.Database.Utils
{
    public static class PermissionUtils
    {
        public static Workscreen GetWorkscreen(this Permission permission)
        {
            var attr = permission.TryGetCustomAttribute<PermissionMappingAttribute>();
            return attr?.Workscreen ?? throw new NotImplementedException();
        }

        public static CommonPermission GetCommonPermission(this Permission permission)
        {
            var attr = permission.TryGetCustomAttribute<PermissionMappingAttribute>();
            return attr?.CommonPermission ?? throw new NotImplementedException();
        }
    }
}
