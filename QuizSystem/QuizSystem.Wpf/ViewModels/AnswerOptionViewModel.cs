using QuizSystem.Wpf.Infrastructure;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Pojedyncza odpowiedź w UI.
    /// Przechowuje zaznaczenie + informację, czy jest poprawna.
    /// </summary>
    public class AnswerOptionViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string Text { get; }

        // Czy odpowiedź jest poprawna (z domeny)
        public bool IsCorrect { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        // Flaga ustawiana po zakończeniu quizu
        private bool _isQuizFinished;
        public bool IsQuizFinished
        {
            get => _isQuizFinished;
            set => SetProperty(ref _isQuizFinished, value);
        }

        public AnswerOptionViewModel(Guid id, string text, bool isCorrect)
        {
            Id = id;
            Text = text;
            IsCorrect = isCorrect;
        }
    }
}
