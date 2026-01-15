using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuizSystem.Infrastructure.Data
{
    // EF Core używa tego w czasie migracji, żeby umieć stworzyć AppDbContext
    // bez uruchamiania całej aplikacji WPF/DI.
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Prosta lokalna baza SQLite do testów/migracji
            optionsBuilder.UseSqlite("Data Source=quizsystem.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
