namespace QuizSystem.Core.Domain.Interfaces
{
    /// <summary>
    /// Reprezentuje pytanie w quizie.
    /// Pytanie ma treść oraz listę możliwych odpowiedzi.
    /// </summary>
    public interface IQuestion
    {
        Guid Id { get; }

        /// <summary>
        /// Treść pytania (np. "Ile to jest 2+2?").
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Lista odpowiedzi możliwych do wyboru.
        /// </summary>
        IReadOnlyList<IAnswer> Answers { get; }

        /// <summary>
        /// Zwraca true, jeśli podany zestaw odpowiedzi jest poprawny.
        /// Pozwala obsłużyć pytania jednokrotnego i wielokrotnego wyboru.
        /// </summary>
        bool CheckAnswer(IEnumerable<Guid> chosenAnswerIds);
    }
}
