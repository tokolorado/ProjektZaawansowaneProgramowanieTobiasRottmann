using Microsoft.EntityFrameworkCore;
using QuizSystem.Infrastructure.Data.Entities;

namespace QuizSystem.Infrastructure.Data
{
    /// <summary>
    /// Główny kontekst bazy danych EF Core.
    /// To tutaj EF Core trzyma mapowanie tabel i relacji.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<QuizEntity> Quizzes => Set<QuizEntity>();
        public DbSet<QuestionEntity> Questions => Set<QuestionEntity>();
        public DbSet<AnswerEntity> Answers => Set<AnswerEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Konfiguracja relacji i ograniczeń w modelu.
        /// Dzięki temu dostajemy: Quiz -> Questions -> Answers.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quiz
            modelBuilder.Entity<QuizEntity>(entity =>
            {
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(q => q.Description)
                      .HasMaxLength(500);

                entity.HasMany(q => q.Questions)
                      .WithOne(qq => qq.Quiz)
                      .HasForeignKey(qq => qq.QuizId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Question
            modelBuilder.Entity<QuestionEntity>(entity =>
            {
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Content)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.HasMany(q => q.Answers)
                      .WithOne(a => a.Question)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Answer
            modelBuilder.Entity<AnswerEntity>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Text)
                      .IsRequired()
                      .HasMaxLength(200);
            });
        }
    }
}
