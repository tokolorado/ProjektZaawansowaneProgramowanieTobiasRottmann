using Microsoft.EntityFrameworkCore;
using QuizSystem.Infrastructure.Data.Entities;

namespace QuizSystem.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // ❗ Migracje robimy w AppBootstrapper, nie tutaj.
            // Tu tylko seed.

            // Jeśli już są quizy -> nie seedujemy drugi raz
            if (await db.Quizzes.AnyAsync())
                return;

            // ====== QUIZ 1 (łatwy) ======
            var quiz1 = new QuizEntity
            {
                Id = Guid.NewGuid(),
                Title = "Quiz łatwy",
                Description = "Proste pytania na rozgrzewkę",
                Questions = new List<QuestionEntity>()
            };

            quiz1.Questions.Add(MakeQuestion(
                "Ile to jest 2 + 2?",
                ("3", false), ("4", true), ("5", false), ("22", false)));

            quiz1.Questions.Add(MakeQuestion(
                "Który język jest używany w .NET?",
                ("C#", true), ("Python", false), ("HTML", false), ("CSS", false)));

            quiz1.Questions.Add(MakeQuestion(
                "Co oznacza skrót UI?",
                ("User Interface", true), ("Unique Index", false), ("Unit Input", false), ("User Internal", false)));

            quiz1.Questions.Add(MakeQuestion(
                "Która z tych technologii jest do GUI na Windows?",
                ("WPF", true), ("Django", false), ("Laravel", false), ("Flask", false)));

            quiz1.Questions.Add(MakeQuestion(
                "Zaznacz poprawne rozszerzenie pliku projektu C#:",
                (".csproj", true), (".java", false), (".py", false), (".html", false)));

            // ====== QUIZ 2 (średni) ======
            var quiz2 = new QuizEntity
            {
                Id = Guid.NewGuid(),
                Title = "Quiz średni",
                Description = "Trochę teorii + praktyki",
                Questions = new List<QuestionEntity>()
            };

            quiz2.Questions.Add(MakeQuestion(
                "Co robi LINQ?",
                ("Ułatwia zapytania do kolekcji", true),
                ("Zamienia WPF na HTML", false),
                ("To baza danych", false),
                ("To system operacyjny", false)));

            quiz2.Questions.Add(MakeQuestion(
                "Zaznacz elementy MVVM:",
                ("Model", true), ("View", true), ("ViewModel", true), ("Controller", false)));

            quiz2.Questions.Add(MakeQuestion(
                "Co robi migration w EF Core?",
                ("Tworzy/aktualizuje schemat bazy", true),
                ("Usuwa projekt", false),
                ("Zamienia C# na C++", false),
                ("Wyłącza debug", false)));

            quiz2.Questions.Add(MakeQuestion(
                "Co oznacza DbContext?",
                ("Główny kontekst EF do pracy z DB", true),
                ("Plik XAML", false),
                ("Silnik UI", false),
                ("Klasa do animacji", false)));

            quiz2.Questions.Add(MakeQuestion(
                "Zaznacz poprawne: Self-contained publish oznacza...",
                ("Aplikacja ma runtime w środku", true),
                ("Wymaga Visual Studio", false),
                ("Nie ma exe", false),
                ("Działa tylko w debug", false)));

            // ====== QUIZ 3 (trudny) ======
            var quiz3 = new QuizEntity
            {
                Id = Guid.NewGuid(),
                Title = "Quiz trudny",
                Description = "Dla ambitnych: EF + architektura + detale",
                Questions = new List<QuestionEntity>()
            };

            quiz3.Questions.Add(MakeQuestion(
                "Co robi Include() w EF Core?",
                ("Ładuje nawigacje (relacje) eager loading", true),
                ("Zapisuje dane do pliku", false),
                ("Tworzy migrację", false),
                ("Kompiluje projekt", false)));

            quiz3.Questions.Add(MakeQuestion(
                "Po co jest Repository pattern?",
                ("Oddziela dostęp do danych od logiki", true),
                ("Zastępuje WPF", false),
                ("Służy do rysowania UI", false),
                ("To framework testów", false)));

            quiz3.Questions.Add(MakeQuestion(
                "Co oznacza, że encja ma private set?",
                ("Dane kontroluje logika domenowa, nie UI", true),
                ("Nie da się jej zapisać w DB", false),
                ("Nie da się jej odczytać", false),
                ("To to samo co public set", false)));

            quiz3.Questions.Add(MakeQuestion(
                "Co jest lepsze dla stabilności MVVM?",
                ("Binding + ICommand zamiast click handler", true),
                ("Pisanie logiki w XAML", false),
                ("Wszystko w code-behind", false),
                ("Brak ViewModeli", false)));

            quiz3.Questions.Add(MakeQuestion(
                "MigrateAsync() robi:",
                ("Zakłada bazę + wykonuje migracje", true),
                ("Kasuje bazę", false),
                ("Uruchamia UI", false),
                ("Kompiluje XAML", false)));

            db.Quizzes.AddRange(quiz1, quiz2, quiz3);
            await db.SaveChangesAsync();
        }

        private static QuestionEntity MakeQuestion(string content, params (string text, bool isCorrect)[] answers)
        {
            var q = new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Content = content,
                Answers = new List<AnswerEntity>()
            };

            foreach (var (text, isCorrect) in answers)
            {
                q.Answers.Add(new AnswerEntity
                {
                    Id = Guid.NewGuid(),
                    Text = text,
                    IsCorrect = isCorrect
                });
            }

            return q;
        }
    }
}
