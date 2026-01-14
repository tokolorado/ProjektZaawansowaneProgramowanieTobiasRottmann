using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Quiz jest agregatem - trzyma kolekcję pytań.
    /// </summary>
    public class Quiz : IQuiz
    {

        // Trzymam pytania w prywatnej liście i wystawiam ReadOnly,
        // bo Quiz jest agregatem (Aggregate Root) i powinien kontrolować swój stan.

        private readonly List<IQuestion> _questions;

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public IReadOnlyList<IQuestion> Questions => _questions.AsReadOnly();

        // EF / serializer
        private Quiz()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Description = null;
            _questions = new List<IQuestion>();
        }

        // Normalne tworzenie w kodzie (generuje nowe Id)
        public Quiz(string title, string? description, IEnumerable<IQuestion> questions)
        {
            // Wartościowe UX i spójność danych: quiz bez tytułu jest bezużyteczny.
            // Waliduję to w domenie, żeby niezależnie od UI (WPF/Web) reguła była ta sama.

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Quiz title cannot be empty.", nameof(title));

            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            var list = questions.ToList();


            // Quiz bez pytań to martwy obiekt — lepiej przerwać wcześniej
            // niż pozwolić na zapis błędnych danych i naprawiać je później.

            if (list.Count == 0)
                throw new ArgumentException("Quiz must contain at least one question.", nameof(questions));

            Id = Guid.NewGuid();
            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            _questions = list;
        }

        // ✅ Rehydrate: odtworzenie z bazy (z zachowaniem Id)
        private Quiz(Guid id, string title, string? description, IEnumerable<IQuestion> questions)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Quiz id cannot be empty.", nameof(id));

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Quiz title cannot be empty.", nameof(title));

            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            var list = questions.ToList();

            if (list.Count == 0)
                throw new ArgumentException("Quiz must contain at least one question.", nameof(questions));

            Id = id;
            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            _questions = list;
        }

        public static Quiz Rehydrate(Guid id, string title, string? description, IEnumerable<IQuestion> questions)
            => new Quiz(id, title, description, questions);

        public int CalculateScore(Dictionary<Guid, IEnumerable<Guid>> userAnswers)
        {
            if (userAnswers is null)
                return 0;

            int score = 0;

            // Używam Dictionary<QuestionId, AnswerIds>, bo to szybkie i czytelne:
            // lookup po Id jest O(1) i nie muszę szukać odpowiedzi w listach.

            foreach (var question in _questions)
            {
                // TryGetValue jest bezpieczne: jeśli użytkownik pominął pytanie, nie ma wyjątku.
                // To bardziej "odporne" zachowanie aplikacji.

                if (userAnswers.TryGetValue(question.Id, out var chosenIds))
                {
                    if (question.CheckAnswer(chosenIds))
                        score++;
                }
            }

            return score;
        }
    }
}
