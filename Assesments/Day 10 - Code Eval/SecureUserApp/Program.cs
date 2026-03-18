using System;
using Serilog;
using SecureUserApp.Services;
using SecureUserApp.Utils;

namespace SecureUserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("=== Secure User Management Application Started ===");

            try
            {
                var userService = new UserService();

                Console.WriteLine("\n--- Registering Users ---");

                // Register Alice
                userService.Register("alice", "Alice@123", "alice@example.com");
                Console.WriteLine("Alice registered successfully.");

                // Register Bob
                userService.Register("bob", "Bob@456", "bob@example.com");
                Console.WriteLine("Bob registered successfully.");

                // Try duplicate registration (will throw exception)
                try
                {
                    userService.Register("alice", "AnotherPass", "duplicate@test.com");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Duplicate registration blocked (expected).");
                    Log.Warning(ex, "Duplicate registration attempt.");
                }

                Console.WriteLine("\n--- Login Attempts ---");

                bool loginAlice = userService.Authenticate("alice", "Alice@123");
                Console.WriteLine(loginAlice
                    ? "Alice logged in successfully."
                    : "Alice login failed.");

                bool loginWrong = userService.Authenticate("alice", "wrongPass");
                Console.WriteLine(loginWrong
                    ? "Login succeeded (unexpected)."
                    : "Login blocked with wrong password (expected).");

                bool loginBob = userService.Authenticate("bob", "Bob@456");
                Console.WriteLine(loginBob
                    ? "Bob logged in successfully."
                    : "Bob login failed.");

                Console.WriteLine("\n--- Standalone Encryption Demo ---");

                string originalText = "SensitiveData_42";
                string encrypted = EncryptionHelper.EncryptData(originalText);
                string decrypted = EncryptionHelper.DecryptData(encrypted);

                Console.WriteLine($"Original  : {originalText}");
                Console.WriteLine($"Encrypted : {encrypted}");
                Console.WriteLine($"Decrypted : {decrypted}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred in the application.");
                Console.WriteLine("Something went wrong. Please check the logs.");
            }
            finally
            {
                Log.Information("=== Application Shutting Down ===");
                Log.CloseAndFlush();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}