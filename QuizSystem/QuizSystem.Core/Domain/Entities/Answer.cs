using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Konkretna implementacja odpowiedzi.
    /// </summary>
    public class Answer : IAnswer
    {
        public Guid Id { get; private set; }

        public string Text { get; private set; }

        public bool IsCorrect { get; private set; }

        // EF Core lubi konstruktor bezparametrowy (później się przyda).
        private Answer() 
        {
            Id = Guid.Empty;
            Text = string.Empty;
            IsCorrect = false;
        }

        public Answer(string text, bool isCorrect)
        {
            // Walidacja - to jest element "myślenia krytycznego"
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Answer text cannot be empty.", nameof(text));

            Id = Guid.NewGuid();
            Text = text.Trim();
            IsCorrect = isCorrect;
        }
    }
}
