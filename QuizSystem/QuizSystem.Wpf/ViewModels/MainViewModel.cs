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
        private IQuiz _quiz;
        private List<QuestionViewModel> _questions;
        private List<QuizListItemViewModel> _allQuizzes;
        private List<QuizListItemViewModel> _filteredQuizzes;
        private QuizListItemViewModel? _selectedQuiz;
        private string _searchText = string.Empty;
        private bool _isInMenu = true;


        private int _currentIndex;
        private QuestionViewModel _currentQuestion;

        private bool _isFinished;
        private int _score;

        private readonly IDialogService _dialogService;


        public string Title => _quiz.Title;
        public string? Description => _quiz.Description;

        public bool IsInMenu
        {
            get => _isInMenu;
            private set => SetProperty(ref _isInMenu, value);
        }

        public bool IsInQuiz => !IsInMenu;

        public List<QuizListItemViewModel> Quizzes
        {
            get => _filteredQuizzes;
            private set => SetProperty(ref _filteredQuizzes, value);
        }

        public QuizListItemViewModel? SelectedQuiz
        {
            get => _selectedQuiz;
            set
            {
                if (SetProperty(ref _selectedQuiz, value))
                {
                    StartQuizCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyQuizFilter();
                }
            }
        }


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
        public RelayCommand StartQuizCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand PrevCommand { get; }
        public RelayCommand FinishCommand { get; }
        public RelayCommand RestartCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CloseAppCommand { get; }

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            IsInMenu = true;
            OnPropertyChanged(nameof(IsInQuiz));


            // Na start bierzemy quiz demo.
            _quiz = QuizFactory.CreateSampleQuiz();

            _questions = _quiz.Questions.Select(q => new QuestionViewModel(q)).ToList();

            _currentIndex = 0;
            _currentQuestion = _questions[_currentIndex];

            _isFinished = false;
            _score = 0;

            StartQuizCommand = new RelayCommand(StartSelectedQuiz, CanStartSelectedQuiz);
            NextCommand = new RelayCommand(Next, CanGoNext);
            PrevCommand = new RelayCommand(Prev, CanGoPrev);
            FinishCommand = new RelayCommand(Finish, CanFinish);
            RestartCommand = new RelayCommand(Restart);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            CloseAppCommand = new RelayCommand(CloseApp);

            _allQuizzes = QuizFactory.CreateSampleQuizzes()
                .Select(q => new QuizListItemViewModel(q))
                .ToList();

            _filteredQuizzes = _allQuizzes.ToList();
            Quizzes = _filteredQuizzes;


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
            // 1) Pytamy użytkownika czy na pewno chce wyjść
            bool confirm = _dialogService.Confirm(
                "Wyjście z quizu",
                "Czy na pewno chcesz wyjść z quizu?\nPostęp zostanie utracony.");

            if (!confirm)
                return;

            // 2) Zamykamy aplikację (lub później wrócimy do ekranu wyboru quizu)
            System.Windows.Application.Current.Shutdown();
        }


        private bool CanCancel() => !IsFinished;



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
            CancelCommand.RaiseCanExecuteChanged();
        }

        private bool CanStartSelectedQuiz() => SelectedQuiz != null;

        private void StartSelectedQuiz()
        {
            if (SelectedQuiz == null)
                return;

            LoadQuiz(SelectedQuiz.Quiz);
            IsInMenu = false;

            OnPropertyChanged(nameof(IsInQuiz));
            RaiseButtons();
        }
        private void ApplyQuizFilter()
        {
            var text = (SearchText ?? string.Empty).Trim();

            var query = _allQuizzes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(text))
            {
                query = query.Where(q =>
                    q.Title.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    (q.Description != null &&
                     q.Description.Contains(text, StringComparison.OrdinalIgnoreCase)));
            }

            query = query.OrderBy(q => q.Title);
            Quizzes = query.ToList();
        }
        private void LoadQuiz(IQuiz quiz)
        {
            _quiz = quiz;

            _questions = _quiz.Questions
                .Select(q => new QuestionViewModel(q))
                .ToList();

            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex];

            Score = 0;
            IsFinished = false;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(TotalQuestions));
            OnPropertyChanged(nameof(CurrentNumber));
            OnPropertyChanged(nameof(FinishedMessage));

            RaiseButtons();
        }

    }

}
