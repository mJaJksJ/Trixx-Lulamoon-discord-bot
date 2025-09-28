using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using TrixxDiscordBot.Server.Startup.Swagger.Filters;

namespace TrixxDiscordBot.Server.Startup.Swagger
{
    public static class TrixxSwaggerExtensions
    {
        private const string PRIMARY_API = "v1";
        internal static IServiceCollection AddTrixxSwaggerGen(this IServiceCollection services)
        {
            services.AddTransient<ISerializerDataContractResolver, TrixxJsonSerializerDataContractResolver>();
            services.Configure<SwaggerGenOptions>(AddSwaggerOptions);
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, api) =>
                    (docName == PRIMARY_API && string.IsNullOrEmpty(api.GroupName))
                            || (docName == api.GroupName));
                c.SwaggerDoc(PRIMARY_API, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Trixx API",
                    Version = "V1",
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                });
            });

            services.Configure<SwaggerUIOptions>(opts =>
            {
                opts.SwaggerEndpoint($"/swagger/{PRIMARY_API}/swagger.json", "Trixx API V1");
            });
            return services;
        }
        internal static void UseTrixxSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        private static void AddSwaggerOptions(SwaggerGenOptions options)
        {
            var c = options;
            c.SchemaFilter<TrixxEnumSchemaFilter>();
        }
    }
}
