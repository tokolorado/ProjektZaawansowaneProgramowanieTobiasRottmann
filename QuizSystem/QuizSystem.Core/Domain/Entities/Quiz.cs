using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Quiz jest agregatem (Aggregate Root) - trzyma kolekcję pytań.
    /// </summary>
    public class Quiz : IQuiz
    {
        private readonly List<IQuestion> _questions;

        public Guid Id { get; private set; }

        public string Title { get; private set; }

        public string? Description { get; private set; }

        public IReadOnlyList<IQuestion> Questions => _questions.AsReadOnly();

        private Quiz()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Description = null;
            _questions = new List<IQuestion>();
        }

        public Quiz(string title, string? description, IEnumerable<IQuestion> questions)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Quiz title cannot be empty.", nameof(title));

            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            var list = questions.ToList();

            if (list.Count == 0)
                throw new ArgumentException("Quiz must contain at least one question.", nameof(questions));

            Id = Guid.NewGuid();
            Title = title.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            _questions = list;
        }

        public int CalculateScore(Dictionary<Guid, IEnumerable<Guid>> userAnswers)
        {
            if (userAnswers is null)
                return 0;

            int score = 0;

            foreach (var question in _questions)
            {
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
