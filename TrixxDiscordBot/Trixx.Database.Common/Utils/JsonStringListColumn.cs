using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Trixx.Database.Common.Utils
{
    public static class JsonStringListColumn
    {
        public static PropertyBuilder<T> Json<T>(this PropertyBuilder<T> propertyBuilder)
        {
            return propertyBuilder
                .HasColumnType("jsonb")
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<T>(x)!
                );
        }
    }
}
