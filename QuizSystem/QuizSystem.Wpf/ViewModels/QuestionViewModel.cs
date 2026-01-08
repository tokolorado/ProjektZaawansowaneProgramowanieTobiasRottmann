using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Pytanie w UI: treść + lista opcji.
    /// </summary>
    public class QuestionViewModel
    {
        public Guid Id { get; }

        public string Content { get; }

        public List<AnswerOptionViewModel> Options { get; }

        public QuestionViewModel(IQuestion question)
        {
            Id = question.Id;
            Content = question.Content;

            Options = question.Answers
                .Select(a => new AnswerOptionViewModel(a.Id, a.Text))
                .ToList();
        }

        public IEnumerable<Guid> GetChosenAnswerIds()
        {
            return Options
                .Where(o => o.IsSelected)
                .Select(o => o.Id)
                .ToList();
        }
    }
}
