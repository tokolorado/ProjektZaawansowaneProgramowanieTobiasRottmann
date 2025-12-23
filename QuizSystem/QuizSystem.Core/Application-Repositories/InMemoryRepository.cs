namespace QuizSystem.Core.Application.Repositories
{
    /// <summary>
    /// Prosta implementacja repozytorium trzymająca dane w pamięci.
    /// To jest świetne do testowania logiki zanim podepniesz EF Core.
    /// </summary>
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _items = new();

        public IReadOnlyList<T> GetAll()
        {
            // Zwracamy kopię, żeby nikt nie modyfikował listy "zza pleców"
            return _items.ToList().AsReadOnly();
        }

        public void Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public bool Remove(T item)
        {
            if (item is null)
                return false;

            return _items.Remove(item);
        }

        public bool Any()
        {
            return _items.Count > 0;
        }
    }
}
