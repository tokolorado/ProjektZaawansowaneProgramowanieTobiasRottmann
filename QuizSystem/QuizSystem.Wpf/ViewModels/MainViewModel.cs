using QuizSystem.Core.Application;
using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.Wpf.Infrastructure;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Główny VM aplikacji: ładuje quiz i pozwala rozwiązać go w WPF.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private readonly IQuiz _quiz;
        private readonly List<QuestionViewModel> _questions;

        private int _currentIndex;
        private QuestionViewModel _currentQuestion;

        private bool _isFinished;
        private int _score;

        public string Title => _quiz.Title;
        public string? Description => _quiz.Description;

        public QuestionViewModel CurrentQuestion
        {
            get => _currentQuestion;
            private set => SetProperty(ref _currentQuestion, value);
        }

        public int CurrentNumber => _currentIndex + 1;
        public int TotalQuestions => _questions.Count;

        public bool IsFinished
        {
            get => _isFinished;
            private set => SetProperty(ref _isFinished, value);
        }

        public int Score
        {
            get => _score;
            private set => SetProperty(ref _score, value);
        }
        public string FinishedMessage
        {
            get
            {
                if (!IsFinished)
                    return string.Empty;

                return $"Koniec! Twój wynik to {Score} / {TotalQuestions}.";
            }
        }

        public RelayCommand NextCommand { get; }
        public RelayCommand PrevCommand { get; }
        public RelayCommand FinishCommand { get; }
        public RelayCommand RestartCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CloseAppCommand { get; }

        public MainViewModel()
        {
            // Na start bierzemy quiz demo.
            _quiz = QuizFactory.CreateSampleQuiz();

            _questions = _quiz.Questions.Select(q => new QuestionViewModel(q)).ToList();

            _currentIndex = 0;
            _currentQuestion = _questions[_currentIndex];

            _isFinished = false;
            _score = 0;

            NextCommand = new RelayCommand(Next, CanGoNext);
            PrevCommand = new RelayCommand(Prev, CanGoPrev);
            FinishCommand = new RelayCommand(Finish, CanFinish);
            RestartCommand = new RelayCommand(Restart);
            CancelCommand = new RelayCommand(Cancel);
            CloseAppCommand = new RelayCommand(CloseApp);

        }

        private bool CanGoNext() => !IsFinished && _currentIndex < _questions.Count - 1;
        private bool CanGoPrev() => !IsFinished && _currentIndex > 0;
        private bool CanFinish() => !IsFinished && _questions.Count > 0;

        private void Next()
        {
            if (!CanGoNext())
                return;

            _currentIndex++;
            CurrentQuestion = _questions[_currentIndex];

            OnPropertyChanged(nameof(CurrentNumber));
            RaiseButtons();
        }

        private void Prev()
        {
            if (!CanGoPrev())
                return;

            _currentIndex--;
            CurrentQuestion = _questions[_currentIndex];

            OnPropertyChanged(nameof(CurrentNumber));
            RaiseButtons();
        }

        private void Finish()
        {
            // Zbieramy odpowiedzi użytkownika do formatu wymaganego przez domenę:
            // QuestionId -> lista AnswerId.
            var userAnswers = new Dictionary<Guid, IEnumerable<Guid>>();

            foreach (var q in _questions)
            {
                userAnswers[q.Id] = q.GetChosenAnswerIds();
            }

            Score = _quiz.CalculateScore(userAnswers);
            IsFinished = true;
            OnPropertyChanged(nameof(FinishedMessage));

            RaiseButtons();
        }

        private void Restart()
        {
            // Reset zaznaczeń
            foreach (var q in _questions)
            {
                foreach (var option in q.Options)
                {
                    option.IsSelected = false;
                }
            }

            Score = 0;
            IsFinished = false;
            OnPropertyChanged(nameof(FinishedMessage));


            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex];
            OnPropertyChanged(nameof(CurrentNumber));

            RaiseButtons();
        }

        private void Cancel()
        {

        }


        private void CloseApp()
        {
            // Zamknięcie całej aplikacji WPF
            System.Windows.Application.Current.Shutdown();
        }


        private void RaiseButtons()
        {
            NextCommand.RaiseCanExecuteChanged();
            PrevCommand.RaiseCanExecuteChanged();
            FinishCommand.RaiseCanExecuteChanged();
        }
    }
}
