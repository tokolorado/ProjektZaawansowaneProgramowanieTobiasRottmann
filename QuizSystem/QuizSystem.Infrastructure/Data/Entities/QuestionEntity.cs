namespace QuizSystem.Infrastructure.Data.Entities
{
    public class QuestionEntity
    {
        public Guid Id { get; set; }


        public string Content { get; set; } = string.Empty;

        public int Order { get; set; }


        public Guid QuizId { get; set; }
        public QuizEntity? Quiz { get; set; }

        public List<AnswerEntity> Answers { get; set; } = new();
    }
}
