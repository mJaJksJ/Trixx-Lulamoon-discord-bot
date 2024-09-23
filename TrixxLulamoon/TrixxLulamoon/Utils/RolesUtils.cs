using TrixxLulamoon.Buttons;
using TrixxLulamoon.Config;

namespace TrixxLulamoon.Utils
{
    public static class RolesUtils
    {
        public static string GetRoleId(this RolesConfig config, ButtonIdType type)
        {
            return type switch
            {
                ButtonIdType.Fluttershy or ButtonIdType.NotFluttershy => config.FluttershyId,
                ButtonIdType.DellArte or ButtonIdType.NotDellArte => config.DellArteId,
                ButtonIdType.Zetsu or ButtonIdType.NotZetsu => config.ZetsuId,
                ButtonIdType.Mythical or ButtonIdType.NotMythical => config.MythicalId,
                ButtonIdType.Musical or ButtonIdType.NotMusical => config.MusicalId,
                _ => throw new NotImplementedException()
            };
        }
    }
}
