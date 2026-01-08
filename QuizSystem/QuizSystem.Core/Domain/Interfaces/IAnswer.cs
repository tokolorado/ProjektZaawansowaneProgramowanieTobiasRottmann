namespace QuizSystem.Core.Domain.Interfaces
{
    /// <summary>
    /// Reprezentuje pojedynczą odpowiedź możliwą do wybrania przez użytkownika.
    /// </summary>
    public interface IAnswer
    {
        /// <summary>
        /// Unikalny identyfikator odpowiedzi.
        /// Przyda się w bazie danych (EF Core) i do mapowania UI.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Tekst odpowiedzi wyświetlany w UI.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Czy ta odpowiedź jest poprawna.
        /// </summary>
        bool IsCorrect { get; }
    }
}
