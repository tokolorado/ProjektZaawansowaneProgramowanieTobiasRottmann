using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Domain.Entities
{
    /// <summary>
    /// Konkretna implementacja odpowiedzi.
    /// </summary>
    /// 

    // Utrzymuję walidację tu (w domenie), bo UI ani baza nie powinny decydować o jakości danych.
    // Dzięki temu niezależnie od źródła (WPF/Web/DB) reguły są te same.


   

    public class Answer : IAnswer
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public bool IsCorrect { get; private set; }

        // EF / serializer
        private Answer()
        {
            Id = Guid.Empty;
            Text = string.Empty;
            IsCorrect = false;
        }

        // Normalne tworzenie w kodzie (generuje nowe Id)
        public Answer(string text, bool isCorrect)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Answer text cannot be empty.", nameof(text));

            Id = Guid.NewGuid();
            Text = text.Trim();
            IsCorrect = isCorrect;
        }

        // ✅ Rehydrate: odtworzenie z bazy (z zachowaniem Id)

        // Rehydrate istnieje, bo domena normalnie generuje Id (kontrola spójności),
        // a przy odczycie z bazy musimy odtworzyć obiekt z konkretnym Id.
        private Answer(Guid id, string text, bool isCorrect)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Answer id cannot be empty.", nameof(id));

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Answer text cannot be empty.", nameof(text));

            Id = id;
            Text = text.Trim();
            IsCorrect = isCorrect;
        }

        public static Answer Rehydrate(Guid id, string text, bool isCorrect)
            => new Answer(id, text, isCorrect);
    }
}
