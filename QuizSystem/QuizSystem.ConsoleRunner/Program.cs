using QuizSystem.Core.Application;
using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.ConsoleRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1) Tworzymy przykładowy quiz z fabryki danych demo.
            IQuiz quiz = QuizFactory.CreateSampleQuiz();

            // 2) Wyświetlamy podstawowe informacje o quizie.
            Console.WriteLine("=== QUIZ DEMO ===");
            Console.WriteLine($"Tytuł: {quiz.Title}");
            Console.WriteLine($"Opis: {quiz.Description}");
            Console.WriteLine();

            // 3) Wypisujemy pytania oraz odpowiedzi.
            foreach (var question in quiz.Questions)
            {
                Console.WriteLine($"Pytanie: {question.Content}");

                foreach (var answer in question.Answers)
                {
                    Console.WriteLine($" - {answer.Text} (Id: {answer.Id})");
                }

                Console.WriteLine();
            }

            // 4) To tylko test domeny. UI zrobimy w WPF w następnym etapie.
            Console.WriteLine("Domena działa. Następny krok: rozwiązywanie quizu w konsoli.");
        }
    }
}
