using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.ConsoleRunner.ConsoleUI
{
    /// <summary>
    /// Odpowiada za uruchomienie quizu w konsoli.
    /// To jest prosta warstwa "UI", zanim zrobimy WPF.
    /// </summary>
    public class ConsoleQuizRunner
    {
        public void Run(IQuiz quiz)
        {
            // Słownik: QuestionId -> lista AnswerId, które użytkownik wybrał
            var userAnswers = new Dictionary<Guid, IEnumerable<Guid>>();

            Console.WriteLine("=== Rozpoczynamy quiz ===");
            Console.WriteLine();

            foreach (var question in quiz.Questions)
            {
                Console.WriteLine(question.Content);

                // Wypisujemy odpowiedzi numerowane 1..N dla wygody użytkownika
                var answers = question.Answers.ToList();
                for (int i = 0; i < answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {answers[i].Text}");
                }

                Console.WriteLine();
                Console.WriteLine("Wpisz numery odpowiedzi oddzielone przecinkami (np. 1,3):");

                // Czytamy wejście od użytkownika
                string input = Console.ReadLine() ?? string.Empty;

                // Parsujemy input na listę numerów
                var chosenIndexes = ParseIndexes(input, answers.Count);

                // Zamieniamy indeksy na Guid odpowiedzi
                var chosenAnswerIds = chosenIndexes.Select(i => answers[i].Id).ToList();

                // Zapisujemy odpowiedź użytkownika
                userAnswers[question.Id] = chosenAnswerIds;

                Console.WriteLine();
            }

            // Liczymy wynik
            int score = quiz.CalculateScore(userAnswers);

            Console.WriteLine("=== Koniec quizu ===");
            Console.WriteLine($"Twój wynik: {score} / {quiz.Questions.Count}");
        }

        private List<int> ParseIndexes(string input, int maxAnswers)
        {
            // 1) Rozbijamy po przecinkach.
            var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // 2) Zamieniamy na liczby i filtrujemy błędy.
            var indexes = new List<int>();

            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out int number))
                {
                    // Zamiana 1..N -> 0..N-1
                    int index = number - 1;

                    if (index >= 0 && index < maxAnswers)
                    {
                        indexes.Add(index);
                    }
                }
            }

            // 3) Usuwamy duplikaty (jak ktoś wpisze 1,1,2)
            return indexes.Distinct().ToList();
        }
    }
}
