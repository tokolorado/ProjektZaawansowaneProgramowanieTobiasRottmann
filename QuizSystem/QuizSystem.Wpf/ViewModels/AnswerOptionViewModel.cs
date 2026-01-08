using QuizSystem.Wpf.Infrastructure;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Pojedyncza opcja odpowiedzi w UI (checkbox).
    /// Trzymamy IsSelected w UI, a nie w domenie.
    /// </summary>
    public class AnswerOptionViewModel : ObservableObject
    {
        public Guid Id { get; }

        public string Text { get; }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public AnswerOptionViewModel(Guid id, string text)
        {
            Id = id;
            Text = text;
            _isSelected = false;
        }
    }
}
