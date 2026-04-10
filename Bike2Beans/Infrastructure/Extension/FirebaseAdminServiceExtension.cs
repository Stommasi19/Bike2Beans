using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Bike2Beans.Infrastructure.Extensions;


namespace Bike2Beans.Infrastructure.Extension;


public sealed class FirebaseAdminOptions
{
    public const string SectionName = "FirebaseAdmin";

    public string ServiceAccountJson { get; init; } = "";
}
public static class FirebaseAdminServiceExtension
{
    private const string ServiceAccountJsonEnvVarName = "FIREBASE_ADMIN_SERVICE_ACCOUNT_JSON";
    private static readonly string ServiceAccountJsonConfigKey = $"{FirebaseAdminOptions.SectionName}:ServiceAccountJson";

    public static IServiceCollection AddFirebaseAdmin(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceAccountJson = configuration.GetRequiredSetting(ServiceAccountJsonEnvVarName, ServiceAccountJsonConfigKey);
        if (string.IsNullOrWhiteSpace(serviceAccountJson) || !serviceAccountJson.TrimStart().StartsWith("{", StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Invalid Firebase Admin service account JSON. Value from '{ServiceAccountJsonEnvVarName}'/'{ServiceAccountJsonConfigKey}' must be a JSON object.");
        }

        services.AddSingleton<IOptions<FirebaseAdminOptions>>(
            Microsoft.Extensions.Options.Options.Create(new FirebaseAdminOptions
            {
                ServiceAccountJson = serviceAccountJson
            }));

        services.AddSingleton<FirebaseApp>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<FirebaseAdminOptions>>().Value;

            try
            {
                return FirebaseApp.DefaultInstance;
            }
            catch (InvalidOperationException)
            {
                return FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(options.ServiceAccountJson)
                });
            }
        });

        services.AddSingleton(sp => FirebaseAuth.GetAuth(sp.GetRequiredService<FirebaseApp>()));


        return services;
    }
}
