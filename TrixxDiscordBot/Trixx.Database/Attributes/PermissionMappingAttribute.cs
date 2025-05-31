using Trixx.Database.Enums;

namespace Trixx.Database.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class PermissionMappingAttribute(Workscreen workscreen, CommonPermission commonPermission) : Attribute
    {
        public Workscreen Workscreen { get; } = workscreen;
        public CommonPermission CommonPermission { get; } = commonPermission;
    }
}
