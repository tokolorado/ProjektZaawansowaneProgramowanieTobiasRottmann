using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Pytanie zawiera treść i kolekcję odpowiedzi.
    /// Obsługujemy pytania single/multi-choice bez osobnego enum - logika wynika z ilości poprawnych odpowiedzi.
    /// </summary>
    public class Question : IQuestion
    {
        private readonly List<IAnswer> _answers;

        public Guid Id { get; private set; }

        public string Content { get; private set; }

        public IReadOnlyList<IAnswer> Answers => _answers.AsReadOnly();

        private Question()
        {
            Id = Guid.Empty;
            Content = string.Empty;
            _answers = new List<IAnswer>();
        }

        public Question(string content, IEnumerable<IAnswer> answers)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Question content cannot be empty.", nameof(content));

            if (answers is null)
                throw new ArgumentNullException(nameof(answers));

            var list = answers.ToList();

            // Minimalna liczba odpowiedzi (praktyczny sens)
            if (list.Count < 2)
                throw new ArgumentException("Question must have at least 2 answers.", nameof(answers));

            // Przynajmniej jedna poprawna odpowiedź
            if (!list.Any(a => a.IsCorrect))
                throw new ArgumentException("Question must have at least one correct answer.", nameof(answers));

            Id = Guid.NewGuid();
            Content = content.Trim();
            _answers = list;
        }

        public bool CheckAnswer(IEnumerable<Guid> chosenAnswerIds)
        {
            if (chosenAnswerIds is null)
                return false;

            var chosen = new HashSet<Guid>(chosenAnswerIds);

            // Zbiór poprawnych odpowiedzi w pytaniu
            var correctIds = _answers
                .Where(a => a.IsCorrect)
                .Select(a => a.Id)
                .ToHashSet();

            // Dokładne dopasowanie: użytkownik musi wybrać wszystkie poprawne i tylko poprawne
            return chosen.SetEquals(correctIds);
        }
    }
}
