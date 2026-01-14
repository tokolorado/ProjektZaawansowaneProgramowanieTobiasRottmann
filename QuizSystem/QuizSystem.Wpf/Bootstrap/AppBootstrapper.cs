using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizSystem.Infrastructure.Data;
using System;
using System.IO;

namespace QuizSystem.Wpf.Bootstrap
{
    public static class AppBootstrapper
    {
        // ✅ To jest metoda, której brakuje wg błędu:
        // "AppBootstrapper does not contain a definition for CreateDbContext"
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
