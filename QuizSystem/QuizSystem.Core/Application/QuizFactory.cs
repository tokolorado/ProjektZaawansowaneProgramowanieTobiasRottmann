using QuizSystem.Core.Domain.Entities;
using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Application
{
    /// <summary>
    /// Fabryka danych demonstracyjnych.
    /// To jest celowo osobna klasa - potem łatwo ją wywalisz, albo zamienisz na dane z DB.
    /// </summary>
    public static class QuizFactory
    {
        public static IQuiz CreateSampleQuiz()
        {
            // Pytanie 1
            var q1Answers = new List<IAnswer>
            {
                new Answer("2", false),
                new Answer("4", true),
                new Answer("5", false),
                new Answer("22", false),
            };

            var q1 = new Question("Ile to jest 2 + 2?", q1Answers);

            // Pytanie 2 (wielokrotny wybór)
            var q2Answers = new List<IAnswer>
            {
                new Answer("C#", true),
                new Answer("WPF", true),
                new Answer("HTML", false),
                new Answer("Entity Framework Core", true),
            };

            var q2 = new Question("Które elementy pasują do aplikacji .NET quiz? (może być kilka poprawnych)", q2Answers);

            // Quiz
            return new Quiz(
                title: "Quiz startowy",
                description: "Prosty quiz do testowania logiki systemu.",
                questions: new List<IQuestion> { q1, q2 }
            );


        }
        public static List<IQuiz> CreateSampleQuizzes()
        {
            // 1) istniejący quiz
            var quiz1 = CreateSampleQuiz();

            // 2) na początek: druga kopia z innym tytułem/opisem
            var quiz2 = CreateSampleQuiz();

            // Jeśli Twoje IQuiz ma settery, ustaw tytuł/opis.
            // Jeżeli nie ma setterów, to za chwilę zrobimy drugi quiz poprawnie
            // przez utworzenie nowej instancji Quiz z innymi danymi.
            // Na razie ważne jest: więcej niż 1 element.

            return new List<IQuiz> { quiz1, quiz2 };
        }
    }
}
