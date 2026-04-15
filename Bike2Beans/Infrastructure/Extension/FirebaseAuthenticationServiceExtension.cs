using Bike2Beans.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Bike2Beans.Infrastructure.Extension;

public sealed class FirebaseAuthenticationOptions
{
    public const string SectionName = "Auth:Firebase";

    public string ProjectId { get; init; } = "";
    public bool RequireHttpsMetadata { get; init; } = true;
}

public static class FirebaseAuthenticationServiceExtension
{
    private const string ProjectIdEnvVarName = "FIREBASE_PROJECT_ID";
    private static readonly string ProjectIdConfigKey = $"{FirebaseAuthenticationOptions.SectionName}:ProjectId";
    private const string ServiceAccountJsonEnvVarName = "FIREBASE_ADMIN_SERVICE_ACCOUNT_JSON";
    private static readonly string ServiceAccountJsonConfigKey = $"{FirebaseAdminOptions.SectionName}:ServiceAccountJson";

    private const string RequireHttpsMetadataEnvVarName = "FIREBASE_REQUIRE_HTTPS_METADATA";
    private static readonly string RequireHttpsMetadataConfigKey = $"{FirebaseAuthenticationOptions.SectionName}:RequireHttpsMetadata";

    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var projectId =
            configuration.GetSetting(ProjectIdEnvVarName, ProjectIdConfigKey);
        if (string.IsNullOrWhiteSpace(projectId))
        {
            var serviceAccountJson = configuration.GetSetting(ServiceAccountJsonEnvVarName, ServiceAccountJsonConfigKey);
            if (!string.IsNullOrWhiteSpace(serviceAccountJson))
            {
                try
                {
                    using var document = JsonDocument.Parse(serviceAccountJson);
                    if (document.RootElement.TryGetProperty("project_id", out var projectIdElement))
                    {
                        projectId = projectIdElement.GetString();
                    }
                }
                catch (JsonException)
                {
                    // Keep existing error behavior below when project id cannot be resolved.
                }
            }
        }

        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new InvalidOperationException(
                $"Missing required configuration value. Set environment variable '{ProjectIdEnvVarName}', appsettings key '{ProjectIdConfigKey}', or provide '{ServiceAccountJsonEnvVarName}'/'{ServiceAccountJsonConfigKey}' containing project_id.");
        }

        var requireHttpsMetadataRaw =
            configuration.GetSetting(RequireHttpsMetadataEnvVarName, RequireHttpsMetadataConfigKey);
        var requireHttpsMetadata = bool.TryParse(requireHttpsMetadataRaw, out var parsedRequireHttps)
        ? parsedRequireHttps
        : true;

        var authority = $"https://securetoken.google.com/{projectId}";

        services.AddSingleton<IOptions<FirebaseAuthenticationOptions>>(
            Options.Create(new FirebaseAuthenticationOptions
            {
                ProjectId = projectId,
                RequireHttpsMetadata = requireHttpsMetadata
            }));

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authority;
                options.Audience = projectId;
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = projectId,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = "user_id"
                };
            });

        return services;
    }
}
