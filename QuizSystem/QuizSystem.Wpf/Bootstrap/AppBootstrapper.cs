using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizSystem.Infrastructure.Data;
using System;

namespace QuizSystem.Wpf.Bootstrap
{
    /// <summary>
    /// Prosty bootstrapper: ładuje konfigurację i buduje DbContextOptions.
    /// Bez pełnego DI kontenera (na razie).
    /// </summary>
    public static class AppBootstrapper
    {
        public static AppDbContext CreateDbContext()
        {
            // 1) Wczytanie appsettings.json z katalogu uruchomieniowego aplikacji
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2) Pobranie connection string
            string? cs = config.GetConnectionString("QuizDb");

            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Brak ConnectionStrings:QuizDb w appsettings.json");

            // 3) Zbudowanie opcji DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(cs)
                .Options;

            return new AppDbContext(options);
        }
    }
}
