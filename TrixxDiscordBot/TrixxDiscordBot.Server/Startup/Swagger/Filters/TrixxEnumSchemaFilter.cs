using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;

namespace TrixxDiscordBot.Server.Startup.Swagger.Filters
{
    public class TrixxEnumSchemaFilter : ISchemaFilter
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public TrixxEnumSchemaFilter(IOptions<JsonOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum && schema.Enum.Count > 0)
            {
                var enumNames = new OpenApiArray();
                var enumValues = new OpenApiArray();

                foreach (var field in context.Type.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
                    var value = enumMemberAttr?.Value ?? field.Name;
                    var name = field.Name;

                    enumValues.Add(new OpenApiString(value));
                    enumNames.Add(new OpenApiString(name));
                }

                schema.Enum = enumValues;
                schema.Extensions["x-enumNames"] = enumNames;
            }
        }
    }
}
