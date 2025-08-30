namespace Trixx.Cartoons.Database.Enums
{
    public enum CartoonType
    {
        FeatureLengthFilm = 0,
        ShortFilm = 1,
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
