using QuizSystem.Core.Application;
using QuizSystem.Core.Domain.Interfaces;
using QuizSystem.Wpf.Infrastructure;

namespace QuizSystem.Wpf.ViewModels
{
    /// <summary>
    /// Główny VM aplikacji: menu wyboru quizu + rozwiązywanie quizu w WPF.
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

        // IsInMenu steruje widokiem: na początku pokazuję menu wyboru quizu,
        // a po starcie quizu przełączam UI na tryb rozwiązywania.
        public bool IsInMenu
        {
            get => _isInMenu;
            private set
            {
                if (SetProperty(ref _isInMenu, value))
                {
                    // UI ma dwa panele zależne od IsInMenu, więc odświeżamy też IsInQuiz.
                    OnPropertyChanged(nameof(IsInQuiz));
                }
            }
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
                    // Przycisk start ma działać dopiero po wybraniu quizu
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
                    // LINQ w filtrze: czytelnie opisuje intencję (szukaj po tytule/opisie),
                    // a jednocześnie jest łatwe do rozbudowy (np. sortowanie, poziom trudności).
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

        /// <summary>
        /// Publiczny dostęp do pytań (np. do wyświetlania podsumowania po Finish).
        /// </summary>
        public IReadOnlyList<QuestionViewModel> Questions => _questions;

        public bool IsFinished
        {
            get => _isFinished;
            private set
            {
                if (SetProperty(ref _isFinished, value))
                {
                    // W tekście końcowym zależymy od IsFinished
                    OnPropertyChanged(nameof(FinishedMessage));
                }
            }
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

            // Start w menu
            IsInMenu = true;

            // Domyślny quiz (awaryjnie) – przydaje się, gdy UI zostałby uruchomiony bez menu
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

            // Lista quizów do menu (z fabryki; potem można to podmienić na DB)
            _allQuizzes = QuizFactory.CreateSampleQuizzes()
                .Select(q => new QuizListItemViewModel(q))
                .ToList();

            _filteredQuizzes = _allQuizzes.ToList();
            Quizzes = _filteredQuizzes;

            // Przy starcie też odświeżamy komendy
            RaiseButtons();
            StartQuizCommand.RaiseCanExecuteChanged();
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
            // 1) Zbieramy odpowiedzi użytkownika do formatu domeny
            var userAnswers = new Dictionary<Guid, IEnumerable<Guid>>();

            foreach (var q in _questions)
            {
                userAnswers[q.Id] = q.GetChosenAnswerIds();
            }

            // 2) Liczymy wynik domenowo (logika w Quiz.CalculateScore)
            Score = _quiz.CalculateScore(userAnswers);

            // 3) Oceniamy każde pytanie i przygotowujemy UI do pokolorowania odpowiedzi
            foreach (var question in _questions)
            {
                question.Evaluate();

                foreach (var option in question.Options)
                {
                    option.IsQuizFinished = true; // dopiero po Finish włączamy kolory
                }
            }

            // 4) Flaga końca – panele w XAML reagują na IsFinished
            IsFinished = true;

            // 5) Odświeżamy UI
            OnPropertyChanged(nameof(Questions));
            RaiseButtons();
        }

        private void Restart()
        {
            // Reset zaznaczeń i wyników
            foreach (var q in _questions)
            {
                foreach (var option in q.Options)
                {
                    option.IsSelected = false;
                    option.IsQuizFinished = false; // ważne: wyłączamy kolory po restarcie
                }

                q.ClearEvaluation();
            }

            Score = 0;
            IsFinished = false;

            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex];

            OnPropertyChanged(nameof(CurrentNumber));
            OnPropertyChanged(nameof(Questions));

            // Po każdej zmianie stanu odświeżam CanExecute komend,
            // żeby UI automatycznie blokował/przywracał przyciski.
            RaiseButtons();
        }

        private void Cancel()
        {
            bool confirm = _dialogService.Confirm(
                "Wyjście z quizu",
                "Czy na pewno chcesz wyjść z quizu?\nPostęp zostanie utracony.");

            if (!confirm)
                return;

            System.Windows.Application.Current.Shutdown();
        }

        private bool CanCancel() => !IsFinished;

        private void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RaiseButtons()
        {
            NextCommand.RaiseCanExecuteChanged();
            PrevCommand.RaiseCanExecuteChanged();
            FinishCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            StartQuizCommand.RaiseCanExecuteChanged();
        }

        private bool CanStartSelectedQuiz() => SelectedQuiz != null;

        private void StartSelectedQuiz()
        {
            if (SelectedQuiz == null)
                return;

            LoadQuiz(SelectedQuiz.Quiz);
            IsInMenu = false;

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

            // reset ocen i kolorów (na wypadek powrotu do quizu)
            foreach (var q in _questions)
            {
                q.ClearEvaluation();
                foreach (var opt in q.Options)
                    opt.IsQuizFinished = false;
            }

            _currentIndex = 0;
            CurrentQuestion = _questions[_currentIndex];

            Score = 0;
            IsFinished = false;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(TotalQuestions));
            OnPropertyChanged(nameof(CurrentNumber));
            OnPropertyChanged(nameof(FinishedMessage));
            OnPropertyChanged(nameof(Questions));

            RaiseButtons();
        }
    }
}
