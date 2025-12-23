namespace QuizSystem.Core.Application.Repositories
{
    /// <summary>
    /// Generyczne repozytorium - abstrakcja przechowywania danych.
    /// Dziś będzie to InMemory, później EF Core bez zmiany logiki aplikacji.
    /// </summary>
    /// <typeparam name="T">Typ encji (np. Quiz, Question)</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Zwraca wszystkie elementy.
        /// </summary>
        IReadOnlyList<T> GetAll();

        /// <summary>
        /// Dodaje nowy element do repozytorium.
        /// </summary>
        void Add(T item);

        /// <summary>
        /// Usuwa element (jeśli istnieje).
        /// </summary>
        bool Remove(T item);

        /// <summary>
        /// Czy repozytorium ma jakiekolwiek elementy.
        /// </summary>
        bool Any();
    }
}
