using Trixx.Database.Attributes;

namespace Trixx.Database.Enums
{
    public enum Permission
    {
        [PermissionMapping(Workscreen.MainPage, CommonPermission.Read)]
        MainPage_Read,
    }
}
