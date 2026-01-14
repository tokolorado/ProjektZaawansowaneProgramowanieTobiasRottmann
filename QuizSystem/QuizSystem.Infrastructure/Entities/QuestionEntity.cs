using System;
using System.Collections.Generic;

namespace QuizSystem.Infrastructure.Entities
{
    /// <summary>
    /// Encja bazy danych: Pytanie.
    /// </summary>
    public class QuestionEntity
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        // FK do Quiz
        public Guid QuizId { get; set; }
        public QuizEntity? Quiz { get; set; }

        // Relacja 1..N: Pytanie ma wiele odpowiedzi
        public List<AnswerEntity> Answers { get; set; } = new();
    }
}
