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
    }
}
