using System.Runtime.Serialization;

namespace Trixx.Database.Enums
{
    public enum CommonPermission
    {
        [EnumMember(Value = "Чтение")]
        Read = 1,

        [EnumMember(Value = "Редактирование")]
        Edit = 2,

        [EnumMember(Value = "Удаление")]
        Delete = 3,
    }
}
