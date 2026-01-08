using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Element listy quizów w menu wyboru
    /// </summary>
    public class QuizListItemViewModel
    {
        public IQuiz Quiz { get; }

        public Guid Id => Quiz.Id;
        public string Title => Quiz.Title;
        public string? Description => Quiz.Description;
        public int QuestionCount => Quiz.Questions.Count;

        public QuizListItemViewModel(IQuiz quiz)
        {
            Quiz = quiz;
        }
    }
}
