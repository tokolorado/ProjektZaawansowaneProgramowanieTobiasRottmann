using QuizSystem.Core.Domain.Entities;

namespace QuizSystem.Core.Application.Repositories
{
    public interface IQuizRepository
    {
        List<Quiz> GetAll();
        Quiz? GetById(Guid id);
        void Add(Quiz quiz);
    }
}
