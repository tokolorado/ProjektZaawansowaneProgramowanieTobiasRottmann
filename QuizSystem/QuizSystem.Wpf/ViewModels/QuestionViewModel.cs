using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.Wpf.Infrastructure;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Pytanie w UI: treść + lista opcji + wynik (po zakończeniu quizu).
    /// </summary>
    public class QuestionViewModel : ObservableObject
    {
        private readonly IQuestion _question;

        public Guid Id { get; }

        public string Content { get; }

        public List<AnswerOptionViewModel> Options { get; }

        // null = jeszcze nie sprawdzano, true/false = po Finish()
        private bool? _isCorrect;

        public bool? IsCorrect
        {
            get => _isCorrect;
            private set => SetProperty(ref _isCorrect, value);
        }

        public string ResultLabel
        {
            get
            {
                if (IsCorrect == null) return string.Empty;
                return IsCorrect == true ? "✅ Poprawnie" : "❌ Błędnie";
            }
        }

        public QuestionViewModel(IQuestion question)
        {
            _question = question;

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

        /// <summary>
        /// Wywoływane po kliknięciu "Zakończ" - sprawdza czy odpowiedź jest poprawna.
        /// </summary>
        public void Evaluate()
        {
            var chosenIds = GetChosenAnswerIds();

            // Logika poprawności jest w domenie (IQuestion.CheckAnswer)
            IsCorrect = _question.CheckAnswer(chosenIds);

            // Odświeżamy etykietę
            OnPropertyChanged(nameof(ResultLabel));
        }

        /// <summary>
        /// Reset stanu wyniku (np. przy Restart).
        /// </summary>
        public void ClearEvaluation()
        {
            IsCorrect = null;
            OnPropertyChanged(nameof(ResultLabel));
        }
    }
}
