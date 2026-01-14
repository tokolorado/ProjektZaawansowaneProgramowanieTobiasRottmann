using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Application.Repositories;
using QuizSystem.Core.Domain.Entities;
using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.Infrastructure.Data;
using QuizSystem.Infrastructure.Data.Entities;

namespace QuizSystem.Infrastructure.Repositories
{
    public class EfQuizRepository : IQuizRepository
    {
        private readonly AppDbContext _db;

        public EfQuizRepository(AppDbContext db)
        {
            _db = db;
        }

        public List<Quiz> GetAll()
        {
            var entities = _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .ToList();

            return entities.Select(MapToDomain).ToList();
        }

        public Quiz? GetById(Guid id)
        {
            var entity = _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefault(q => q.Id == id);

            return entity == null ? null : MapToDomain(entity);
        }

        public void Add(Quiz quiz)
        {
            var entity = MapToEntity(quiz);
            _db.Quizzes.Add(entity);
            _db.SaveChanges();
        }

        // ----------------------------
        // EF -> Domain
        // ----------------------------
        private static Quiz MapToDomain(QuizEntity qe)
        {
            var domainQuestions = qe.Questions
                .Select(qEntity =>
                {
                    var answers = qEntity.Answers
                        .Select(ae => (IAnswer)Answer.Rehydrate(ae.Id, ae.Text, ae.IsCorrect))
                        .ToList();

                    return (IQuestion)Question.Rehydrate(qEntity.Id, qEntity.Content, answers);
                })
                .ToList();

            return Quiz.Rehydrate(qe.Id, qe.Title, qe.Description, domainQuestions);
        }

        // ----------------------------
        // Domain -> EF
        // ----------------------------
        private static QuizEntity MapToEntity(Quiz quiz)
        {
            var quizEntity = new QuizEntity
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = new List<QuestionEntity>()
            };

            foreach (var q in quiz.Questions)
            {
                var qEntity = new QuestionEntity
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuizId = quizEntity.Id,
                    Answers = new List<AnswerEntity>()
                };

                foreach (var a in q.Answers)
                {
                    qEntity.Answers.Add(new AnswerEntity
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                        QuestionId = qEntity.Id
                    });
                }

                quizEntity.Questions.Add(qEntity);
            }

            return quizEntity;
        }
    }
}
