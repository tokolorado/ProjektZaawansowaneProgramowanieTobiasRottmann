using QuizSystem.Core.Application;
using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.ConsoleRunner.ConsoleUI;

namespace QuizSystem.ConsoleRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Tworzymy quiz demo.
            IQuiz quiz = QuizFactory.CreateSampleQuiz();

            // Uruchamiamy quiz w konsoli.
            var runner = new ConsoleQuizRunner();
            runner.Run(quiz);

            Console.WriteLine();
            Console.WriteLine("Następny krok: WPF (desktop UI).");
        }
    }
}
