using Trixx.Database.Attributes;

namespace Trixx.Database.Enums
{
    public enum Permission
    {
        [PermissionMapping(Workscreen.MainPage, CommonPermission.Read)]
        MainPage_Read = 0,

        [PermissionMapping(Workscreen.DictionaryCartoons, CommonPermission.Read)]
        DictionaryCartoons_Read = 1,
        [PermissionMapping(Workscreen.DictionaryCartoons, CommonPermission.Edit)]
        DictionaryCartoons_Edit = 2,
        [PermissionMapping(Workscreen.DictionaryCartoons, CommonPermission.Delete)]
        DictionaryCartoons_Delete = 3,

        [PermissionMapping(Workscreen.DictionaryStudios, CommonPermission.Read)]
        DictionaryStudios_Read = 4,
        [PermissionMapping(Workscreen.DictionaryStudios, CommonPermission.Edit)]
        DictionaryStudios_Edit = 5,
        [PermissionMapping(Workscreen.DictionaryCartoons, CommonPermission.Delete)]
        DictionaryStudios_Delete = 6,

        [PermissionMapping(Workscreen.TrixxUsers, CommonPermission.Read)]
        TrixxUsers_Read = 7,
        [PermissionMapping(Workscreen.TrixxUsers, CommonPermission.Edit)]
        TrixxUsers_Edit = 8,
    }
}
