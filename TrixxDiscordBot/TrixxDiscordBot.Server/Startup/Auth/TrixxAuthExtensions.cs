using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Trixx.Database.Enums;
using Trixx.Database;
using Trixx.Database.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrixxDiscordBot.Server.Startup.Auth
{
    public static class TrixxAuthExtensions
    {
        public const string RefreshTokenProviderName = "RefreshTokenProvider";

        public static IServiceCollection AddTrixxIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = configuration.ReadRequiredSection<JwtConfiguration>();
            services
                .Configure<DataProtectionTokenProviderOptions>(o =>
                {
                    o.TokenLifespan = TimeSpan.FromDays(jwtConfiguration.RefreshTokenLifetimeDays);
                });

            services.
                AddIdentity<TrixxUser, TrixxRole>(c =>
                {
                    c.Password.RequireDigit = true;
                    c.Password.RequiredLength = 8;
                    c.Password.RequireLowercase = true;
                    c.Password.RequireUppercase = true;
                    c.Password.RequireNonAlphanumeric = true;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                 .AddDefaultTokenProviders()
                 .AddTokenProvider(RefreshTokenProviderName, typeof(DataProtectorTokenProvider<TrixxUser>));

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(c =>
                {
                    c.RequireHttpsMetadata = false;
                    c.SaveToken = true;
                    c.TokenValidationParameters.ValidateIssuer = true;
                    c.TokenValidationParameters.ValidIssuer = jwtConfiguration.IssuerFullName;

                    c.TokenValidationParameters.ValidateAudience = true;
                    c.TokenValidationParameters.ValidAudience = jwtConfiguration.Audience;

                    c.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    c.TokenValidationParameters.IssuerSigningKey = jwtConfiguration.SigningKey;

                    c.TokenValidationParameters.ValidateLifetime = true;
                }) ;

            services.AddAuthorization();
            services
                .AddSingleton<JwtBuilder>()
                .AddSingleton(jwtConfiguration);

            return services;
        }

        internal static Claim ToClaim(this Permission permission) =>
            new(TrixxClaimTypes.CLAIMTYPE_PERMISSION, permission.ToString());

        internal static void UseTrixxJwt(this IApplicationBuilder app) =>
            app
            .UseAuthentication()
            .UseAuthorization();

        internal static int GetId(this ClaimsPrincipal user)
        {
            var claim = user.Claims.SingleOrDefault(x => x.Type == TrixxClaimTypes.CLAIMTYPE_USER_ID);
            return claim == null ? throw new InvalidOperationException() : int.Parse(claim.Value);
        }

        internal static bool HasPermission(this ClaimsPrincipal user, Permission permission) =>
            user.HasClaim(TrixxClaimTypes.CLAIMTYPE_PERMISSION, permission.ToString());
    }
}
