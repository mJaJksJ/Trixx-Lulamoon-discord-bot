using System.Runtime.Serialization;

namespace Trixx.Database.Enums
{
    public enum Workscreen
    {
        [EnumMember(Value = "Главная")]
        MainPage = 1,

        [EnumMember(Value = "Мультфильмы")]
        DictionaryCartoons = 2,

        [EnumMember(Value = "Мульт. Студии")]
        DictionaryStudios = 3,

        [EnumMember(Value = "Пользователи")]
        TrixxUsers = 4,

        [EnumMember(Value = "Роли")]
        TrixxRoles = 5,

        [EnumMember(Value = "Паки")]
        CartoonsPack = 6,
    }   
}
