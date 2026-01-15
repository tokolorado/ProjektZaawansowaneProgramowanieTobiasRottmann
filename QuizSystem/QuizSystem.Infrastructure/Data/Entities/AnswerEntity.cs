namespace QuizSystem.Infrastructure.Data.Entities
{
    public class AnswerEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int Order { get; set; }


        public Guid QuestionId { get; set; }
        public QuestionEntity? Question { get; set; }
    }
}
