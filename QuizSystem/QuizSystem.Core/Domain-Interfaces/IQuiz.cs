namespace QuizSystem.Core.Domain.Interfaces
{
    /// <summary>
    /// Reprezentuje quiz, czyli zbiór pytań.
    /// </summary>
    public interface IQuiz
    {
        Guid Id { get; }

        /// <summary>
        /// Nazwa quizu wyświetlana w UI.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Opis quizu (opcjonalny, ale przydatny na stronach Razor).
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Lista pytań w quizie.
        /// </summary>
        IReadOnlyList<IQuestion> Questions { get; }

        /// <summary>
        /// Liczy wynik na podstawie odpowiedzi użytkownika.
        /// </summary>
        int CalculateScore(Dictionary<Guid, IEnumerable<Guid>> userAnswers);
    }
}
