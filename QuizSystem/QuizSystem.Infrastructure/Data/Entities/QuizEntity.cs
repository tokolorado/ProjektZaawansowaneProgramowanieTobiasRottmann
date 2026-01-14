namespace QuizSystem.Infrastructure.Data.Entities
{
    public class QuizEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<QuestionEntity> Questions { get; set; } = new();
    }
}
