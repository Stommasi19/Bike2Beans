using Bike2Beans.Infrastructure.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Bike2Beans.Infrastructure.Authentication;

public static class FirebaseAdminBootstrapper
{
    public static void Initialize(IConfiguration configuration)
    {
        // Determine environment
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                     ?? configuration["Environment:Name"]
                     ?? "Production";
        var isDevelopment = string.Equals(envName, "Development", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine($"[FIREBASE INIT] Environment: {envName}, IsDevelopment: {isDevelopment}");

        var authDomainUrl = configuration.GetRequiredSetting("FIREBASE_AUTH_DOMAIN", "Auth:Firebase:AuthDomain");
        var projectId = configuration.GetRequiredSetting("FIREBASE_PROJECT_ID", "Auth:Firebase:ProjectId");
        var credsPath = configuration.GetRequiredSetting("FIREBASE_CREDENTIALS_FILE", "Auth:Firebase:CredentialsFile");

        Console.WriteLine($"[FIREBASE INIT] Project ID: {projectId}");
        Console.WriteLine($"[FIREBASE INIT] Credentials Path: {credsPath}");

        // If in Development, point admin SDK to the emulator host
        if (isDevelopment && !string.IsNullOrWhiteSpace(authDomainUrl))
        {
            Console.WriteLine($"[FIREBASE INIT] Setting emulator host: {authDomainUrl}");
            Environment.SetEnvironmentVariable("FIREBASE_AUTH_EMULATOR_HOST", authDomainUrl);
        }

        if (FirebaseApp.DefaultInstance == null)
        {
            Console.WriteLine("[FIREBASE INIT] Initializing Firebase Admin SDK...");
            GoogleCredential cred;
            if (isDevelopment)
            {
                Console.WriteLine("[FIREBASE INIT] Using development mode (mock token)");
                cred = GoogleCredential.FromAccessToken("owner");
            }
            else if (!string.IsNullOrWhiteSpace(credsPath) && File.Exists(credsPath))
            {
                Console.WriteLine($"[FIREBASE INIT] Loading credentials from file: {credsPath}");
                Console.WriteLine($"[FIREBASE INIT] File exists: {File.Exists(credsPath)}");
                try
                {
                    cred = GoogleCredential.FromFile(credsPath);
                    Console.WriteLine("[FIREBASE INIT] Successfully loaded credentials from file");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FIREBASE INIT] ERROR loading credentials from file: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine($"[FIREBASE INIT] File not found at: {credsPath} (exists: {File.Exists(credsPath)})");
                Console.WriteLine("[FIREBASE INIT] Attempting Application Default Credentials...");
                try
                {
                    cred = GoogleCredential.GetApplicationDefault();
                    Console.WriteLine("[FIREBASE INIT] Successfully loaded Application Default Credentials");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FIREBASE INIT] ERROR loading Application Default Credentials: {ex.Message}");
                    throw;
                }
            }

            FirebaseApp.Create(new AppOptions
            {
                Credential = cred,
                ProjectId = string.IsNullOrWhiteSpace(projectId) ? null : projectId,
            });

            Console.WriteLine($"[FIREBASE INIT] Firebase Admin SDK initialized for project: {projectId}");
        }
        else
        {
            Console.WriteLine("[FIREBASE INIT] Firebase Admin SDK already initialized");
        }
    }
}

