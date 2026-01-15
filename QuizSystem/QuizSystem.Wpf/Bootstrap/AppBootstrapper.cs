using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizSystem.Infrastructure.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuizSystem.Wpf.Bootstrap
{
    public static class AppBootstrapper
    {
        // Tworzy DbContext z connection string (z appsettings.json albo domyślnie obok exe)
        public static AppDbContext CreateDbContext()
        {
            var connStr = TryReadConnectionStringFromAppSettings();

            if (string.IsNullOrWhiteSpace(connStr))
            {
                // domyślnie baza obok exe (w folderze aplikacji)
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quizsystem.db");
                connStr = $"Data Source={dbPath}";
            }

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connStr)
                .Options;

            return new AppDbContext(options);
        }

        // ✅ To odpalasz na starcie aplikacji:
        // - tworzy/aktualizuje bazę (migracje)
        // - seeduje dane (jeśli baza jest pusta)
        public static async Task EnsureDatabaseCreatedAndSeededAsync()
        {
            using var db = CreateDbContext();

            // 1) migracje -> tworzy DB jeśli nie ma, aktualizuje schemat jeśli jest
            await db.Database.MigrateAsync();

            // 2) seed -> tylko jeśli w bazie nie ma quizów
            await DbSeeder.SeedAsync(db);
        }

        private static string? TryReadConnectionStringFromAppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                return config.GetConnectionString("Default");
            }
            catch
            {
                return null;
            }
        }
    }
}
