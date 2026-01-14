using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Pytanie zawiera treść i kolekcję odpowiedzi.
    /// </summary>
    public class Question : IQuestion
    {
        private readonly List<IAnswer> _answers;

        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public IReadOnlyList<IAnswer> Answers => _answers.AsReadOnly();

        // EF / serializer
        private Question()
        {
            Id = Guid.Empty;
            Content = string.Empty;
            _answers = new List<IAnswer>();
        }

        // Normalne tworzenie w kodzie (generuje nowe Id)
        public Question(string content, IEnumerable<IAnswer> answers)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Question content cannot be empty.", nameof(content));

            if (answers is null)
                throw new ArgumentNullException(nameof(answers));

            var list = answers.ToList();

            if (list.Count < 2)
                throw new ArgumentException("Question must have at least 2 answers.", nameof(answers));

            if (!list.Any(a => a.IsCorrect))
                throw new ArgumentException("Question must have at least one correct answer.", nameof(answers));

            Id = Guid.NewGuid();
            Content = content.Trim();
            _answers = list;
        }

        // ✅ Rehydrate: odtworzenie z bazy (z zachowaniem Id)
        private Question(Guid id, string content, IEnumerable<IAnswer> answers)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Question id cannot be empty.", nameof(id));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Question content cannot be empty.", nameof(content));

            if (answers is null)
                throw new ArgumentNullException(nameof(answers));

            var list = answers.ToList();

            if (list.Count < 2)
                throw new ArgumentException("Question must have at least 2 answers.", nameof(answers));

            if (!list.Any(a => a.IsCorrect))
                throw new ArgumentException("Question must have at least one correct answer.", nameof(answers));

            Id = id;
            Content = content.Trim();
            _answers = list;
        }

        public static Question Rehydrate(Guid id, string content, IEnumerable<IAnswer> answers)
            => new Question(id, content, answers);

        public bool CheckAnswer(IEnumerable<Guid> chosenAnswerIds)
        {
            if (chosenAnswerIds is null)
                return false;

            var chosen = new HashSet<Guid>(chosenAnswerIds);

            var correctIds = _answers
                .Where(a => a.IsCorrect)
                .Select(a => a.Id)
                .ToHashSet();

            return chosen.SetEquals(correctIds);
        }
    }
}
