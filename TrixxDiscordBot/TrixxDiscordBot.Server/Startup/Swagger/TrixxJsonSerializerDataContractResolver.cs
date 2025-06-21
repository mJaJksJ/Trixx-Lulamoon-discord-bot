using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TrixxDiscordBot.Server.Startup.Swagger
{
    public sealed class TrixxJsonSerializerDataContractResolver : ISerializerDataContractResolver
    {
        private readonly JsonSerializerDataContractResolver _resolver;

        public TrixxJsonSerializerDataContractResolver(IOptions<JsonOptions> options)
        {
            var serializerOptions = options.Value.JsonSerializerOptions;
            _resolver = new JsonSerializerDataContractResolver(serializerOptions);
        }

        public DataContract GetDataContractForType(Type type)
        {
            var result = _resolver.GetDataContractForType(type);
            if (type.IsEnum)
            {
                result = DataContract.ForPrimitive(
                    underlyingType: result.UnderlyingType,
                    dataType: DataType.String,
                    dataFormat: "string",
                    jsonConverter: x => $"\"{x}\"");
            }
            return result;
        }
    }
}
