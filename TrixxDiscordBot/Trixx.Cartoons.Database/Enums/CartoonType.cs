using System.Runtime.Serialization;

namespace Trixx.Cartoons.Database.Enums
{
    public enum CartoonType
    {
        [EnumMember(Value = "п/м")]
        FeatureLengthFilm = 0,
        [EnumMember(Value = "к/м")]
        ShortFilm = 1,
        [EnumMember(Value = "м/с")]
        SerialFilm = 2,
    }

    public static class CartoonTypeEnumHelper
    {
        public static string CartoonTypesShortLabels(this CartoonType type) {
            return type switch
            {
                CartoonType.FeatureLengthFilm => "п/м",
                CartoonType.ShortFilm => "к/м",
                CartoonType.SerialFilm => "м/с",
                _ => throw new NotImplementedException(),
            };
        }

    }
}
