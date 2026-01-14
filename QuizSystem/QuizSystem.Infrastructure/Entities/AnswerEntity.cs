using System;

namespace QuizSystem.Infrastructure.Entities
{
    /// <summary>
    /// Encja bazy danych: Odpowiedź.
    /// </summary>
    public class AnswerEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        // FK do Question
        public Guid QuestionId { get; set; }
        public QuestionEntity? Question { get; set; }
    }
}
