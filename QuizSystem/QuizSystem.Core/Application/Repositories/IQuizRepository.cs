using QuizSystem.Core.Domain.Entities;

namespace QuizSystem.Core.Application.Repositories
{
    /// <summary>
    /// Repozytorium quizów – abstrakcja dostępu do danych.
    /// Definiuje pełny zestaw operacji CRUD.
    /// </summary>
    public interface IQuizRepository
    {
        /// <summary>
        /// Pobiera wszystkie quizy (Read).
        /// </summary>
        Task<List<Quiz>> GetAllAsync();

        /// <summary>
        /// Pobiera pojedynczy quiz po ID (Read).
        /// </summary>
        Task<Quiz?> GetByIdAsync(Guid id);

        /// <summary>
        /// Dodaje nowy quiz do bazy (Create).
        /// </summary>
        Task AddAsync(Quiz quiz);

        /// <summary>
        /// Aktualizuje istniejący quiz (Update).
        /// </summary>
        Task UpdateAsync(Quiz quiz);

        /// <summary>
        /// Usuwa quiz z bazy po ID (Delete).
        /// </summary>
        Task DeleteAsync(Guid id);
    }
}
