using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Application.Repositories;
using QuizSystem.Core.Domain.Entities;
using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.Infrastructure.Data;
using QuizSystem.Infrastructure.Data.Entities;

namespace QuizSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repozytorium EF Core:
    /// - czyta i zapisuje Quizy do bazy
    /// - mapuje Entity <-> Domain, żeby domena nie zależała od EF
    /// </summary>
    public class EfQuizRepository : IQuizRepository
    {
        private readonly AppDbContext _db;

        public EfQuizRepository(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // READ
        // =========================

        public async Task<List<Quiz>> GetAllAsync()
        {
            // AsNoTracking = odczyt bez śledzenia zmian (wydajniej, bo nic nie edytujemy w tym miejscu)
            var entities = await _db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .ToListAsync();

            return entities.Select(MapToDomain).ToList();
        }

        public async Task<Quiz?> GetByIdAsync(Guid id)
        {
            var entity = await _db.Quizzes
                .AsNoTracking()
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            return entity == null ? null : MapToDomain(entity);
        }

        // =========================
        // CREATE
        // =========================

        public async Task AddAsync(Quiz quiz)
        {
            var entity = MapToEntity(quiz);

            _db.Quizzes.Add(entity);
            await _db.SaveChangesAsync();
        }

        // =========================
        // UPDATE
        // =========================

        public async Task UpdateAsync(Quiz quiz)
        {
            // Najprościej: usuwamy stary graf i wstawiamy nowy (w projekcie demo to wystarczy).
            // Przy większych systemach robi się "diff" i aktualizuje selektywnie,
            // ale tu ważniejsze jest spełnienie CRUD i czytelność.
            var existing = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quiz.Id);

            if (existing == null)
                throw new InvalidOperationException($"Quiz with id '{quiz.Id}' not found.");

            // 1) Czyścimy dzieci (pytania i odpowiedzi)
            _db.Answers.RemoveRange(existing.Questions.SelectMany(x => x.Answers));
            _db.Questions.RemoveRange(existing.Questions);

            // 2) Aktualizujemy podstawowe pola
            existing.Title = quiz.Title;
            existing.Description = quiz.Description;

            // 3) Dodajemy nowy graf pytań/odpowiedzi
            var newQuestions = CreateQuestionEntities(quiz, existing.Id);
            existing.Questions = newQuestions;

            await _db.SaveChangesAsync();
        }

        // =========================
        // DELETE
        // =========================

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (entity == null)
                return; // Delete "idempotent" - jak nie ma, to nic nie robimy

            _db.Quizzes.Remove(entity);
            await _db.SaveChangesAsync();
        }

        // =========================
        // MAPOWANIE: Entity -> Domain
        // =========================

        private static Quiz MapToDomain(QuizEntity quizEntity)
        {
            // Tworzymy pytania domenowe
            var domainQuestions = quizEntity.Questions
                .OrderBy(q => q.Order)
                .Select(MapQuestionToDomain)
                .ToList();

            // UWAGA:
            // Jeśli Twoja domena ma Rehydrate(...) to użyj jej.
            // Jeśli nie ma, to konstruktor Quiz tworzy nowe Id i to będzie konflikt z DB.
            // Zakładam, że masz Rehydrate, bo wcześniej tak to naprawialiśmy.
            return Quiz.Rehydrate(
                id: quizEntity.Id,
                title: quizEntity.Title,
                description: quizEntity.Description,
                questions: domainQuestions
            );
        }

        private static Question MapQuestionToDomain(QuestionEntity questionEntity)
        {
            var domainAnswers = questionEntity.Answers
                .OrderBy(a => a.Order)
                .Select(MapAnswerToDomain)
                .ToList();

            return Question.Rehydrate(
                id: questionEntity.Id,
                content: questionEntity.Content,
                answers: domainAnswers
            );
        }

        private static Answer MapAnswerToDomain(AnswerEntity answerEntity)
        {
            return Answer.Rehydrate(
                id: answerEntity.Id,
                text: answerEntity.Text,
                isCorrect: answerEntity.IsCorrect
            );
        }

        // =========================
        // MAPOWANIE: Domain -> Entity
        // =========================

        private static QuizEntity MapToEntity(Quiz quiz)
        {
            var quizEntity = new QuizEntity
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Questions = CreateQuestionEntities(quiz, quiz.Id)
            };

            return quizEntity;
        }

        private static List<QuestionEntity> CreateQuestionEntities(Quiz quiz, Guid quizId)
        {
            // quiz.Questions jest IReadOnlyList<IQuestion> - mapujemy do encji EF
            var list = new List<QuestionEntity>();

            int qOrder = 1;
            foreach (var q in quiz.Questions)
            {
                var questionEntity = new QuestionEntity
                {
                    Id = q.Id,
                    QuizId = quizId,
                    Content = q.Content,
                    Order = qOrder++,
                    Answers = CreateAnswerEntities(q, q.Id)
                };

                list.Add(questionEntity);
            }

            return list;
        }

        private static List<AnswerEntity> CreateAnswerEntities(IQuestion question, Guid questionId)
        {
            var list = new List<AnswerEntity>();

            int aOrder = 1;
            foreach (var a in question.Answers)
            {
                list.Add(new AnswerEntity
                {
                    Id = a.Id,
                    QuestionId = questionId,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Order = aOrder++
                });
            }

            return list;
        }
    }
}
