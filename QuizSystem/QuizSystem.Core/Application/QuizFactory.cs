using QuizSystem.Core.Domain.Entities;
using QuizSystem.Core.Domain.Interfaces;

namespace QuizSystem.Core.Application
{
    /// <summary>
    /// Fabryka danych demonstracyjnych.
    /// Docelowo można ją zastąpić danymi z bazy (EF Core).
    /// </summary>
    public static class QuizFactory
    {
        /// <summary>
        /// Zachowane dla kompatybilności: zwraca quiz łatwy.
        /// </summary>
        public static IQuiz CreateSampleQuiz()
        {
            return CreateEasyQuiz();
        }

        /// <summary>
        /// Zwraca listę quizów do menu (łatwy/średni/trudny).
        /// </summary>
        public static List<IQuiz> CreateSampleQuizzes()
        {
            return new List<IQuiz>
            {
                CreateEasyQuiz(),
                CreateMediumQuiz(),
                CreateHardQuiz()
            };
        }

        // =========================
        // QUIZ 1 — ŁATWY (mix)
        // 3 single + 2 multi
        // =========================
        private static IQuiz CreateEasyQuiz()
        {
            // 1) single
            var q1 = new Question(
                "Ile to jest 2 + 2?",
                new List<IAnswer>
                {
                    new Answer("3", false),
                    new Answer("4", true),
                    new Answer("5", false),
                    new Answer("22", false),
                });

            // 2) multi (3 poprawne)
            var q2 = new Question(
                "Które z poniższych są związane z .NET? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("C#", true),
                    new Answer(".NET", true),
                    new Answer("WPF", true),
                    new Answer("Photoshop", false),
                });

            // 3) single
            var q3 = new Question(
                "Co oznacza skrót IDE?",
                new List<IAnswer>
                {
                    new Answer("Zintegrowane środowisko programistyczne", true),
                    new Answer("Internet Download Engine", false),
                    new Answer("Internal Debug Editor", false),
                    new Answer("Index Data Encoder", false),
                });

            // 4) multi (2 poprawne)
            var q4 = new Question(
                "Które czynności są typowe przed zrobieniem commita? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Sprawdzenie zmian (np. Git status / Git Desktop)", true),
                    new Answer("Dodanie plików do stage (stage/unstage)", true),
                    new Answer("Natychmiastowy push bez commita", false),
                    new Answer("Usunięcie folderu .git", false),
                });

            // 5) single
            var q5 = new Question(
                "Do czego służy plik .gitignore?",
                new List<IAnswer>
                {
                    new Answer("Ignoruje wybrane pliki w repozytorium (nie trafiają do commita)", true),
                    new Answer("Ustawia język aplikacji", false),
                    new Answer("Służy do pisania testów jednostkowych", false),
                    new Answer("Szyfruje kod źródłowy", false),
                });

            return new Quiz(
                title: "Quiz Łatwy — Podstawy (mix)",
                description: "Poziom: ŁATWY • Czas: ~3–5 min • Proste pytania + kilka wielokrotnego wyboru.",
                questions: new List<IQuestion> { q1, q2, q3, q4, q5 }
            );
        }

        // =========================
        // QUIZ 2 — ŚREDNI (mix)
        // 2 single + 3 multi
        // =========================
        private static IQuiz CreateMediumQuiz()
        {
            // 1) single
            var q1 = new Question(
                "Która kolekcja najlepiej pasuje do unikalnych wartości (bez duplikatów)?",
                new List<IAnswer>
                {
                    new Answer("List<T>", false),
                    new Answer("HashSet<T>", true),
                    new Answer("Dictionary<TKey,TValue>", false),
                    new Answer("Queue<T>", false),
                });

            // 2) multi (2 poprawne)
            var q2 = new Question(
                "Które operacje są typowe w LINQ? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Where", true),
                    new Answer("Select", true),
                    new Answer("Paint", false),
                    new Answer("ResizeWindow", false),
                });

            // 3) multi (2 poprawne)
            var q3 = new Question(
                "W WPF, co jest typowe dla MVVM? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Binding w XAML do właściwości ViewModelu", true),
                    new Answer("Komendy (ICommand) do obsługi przycisków", true),
                    new Answer("Logika UI tylko w code-behind", false),
                    new Answer("Brak potrzeby DataContext", false),
                });

            // 4) single
            var q4 = new Question(
                "Co oznacza 'Build' w Visual Studio?",
                new List<IAnswer>
                {
                    new Answer("Kompiluje projekt", true),
                    new Answer("Usuwa folder .git", false),
                    new Answer("Tworzy bazę danych automatycznie", false),
                    new Answer("Publikuje aplikację na Azure", false),
                });

            // 5) multi (3 poprawne)
            var q5 = new Question(
                "Co w praktyce daje EF Core? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Mapowanie obiektów C# na tabele w bazie danych", true),
                    new Answer("CRUD bez pisania ręcznego SQL w wielu przypadkach", true),
                    new Answer("Relacje (Include/ThenInclude) w kodzie", true),
                    new Answer("Zastępuje WPF jako UI framework", false),
                });

            return new Quiz(
                title: "Quiz Średni — C# / WPF / LINQ (mix)",
                description: "Poziom: ŚREDNI • Czas: ~6–8 min • Kolekcje, LINQ, MVVM i EF Core.",
                questions: new List<IQuestion> { q1, q2, q3, q4, q5 }
            );
        }

        // =========================
        // QUIZ 3 — TRUDNY (mix)
        // 2 single + 3 multi (w tym 1 z 3 poprawnymi „podchwytliwe”)
        // =========================
        private static IQuiz CreateHardQuiz()
        {
            // 1) multi (2 poprawne)
            var q1 = new Question(
                "Co jest celem enkapsulacji w OOP? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Ograniczenie bezpośredniej modyfikacji stanu obiektu z zewnątrz", true),
                    new Answer("Ukrycie szczegółów implementacji", true),
                    new Answer("Wymuszenie dziedziczenia w każdym przypadku", false),
                    new Answer("Zamiana wszystkich klas na static", false),
                });

            // 2) single
            var q2 = new Question(
                "Co oznacza 'private set;' przy właściwości w C#?",
                new List<IAnswer>
                {
                    new Answer("Wartość można zmieniać tylko wewnątrz klasy", true),
                    new Answer("Wartość można zmieniać tylko w innym projekcie", false),
                    new Answer("Właściwość jest stała (const)", false),
                    new Answer("Getter jest prywatny, setter publiczny", false),
                });

            // 3) multi (3 poprawne) — trudniejsze, ale fair
            var q3 = new Question(
                "Które stwierdzenia o EF Core i wydajności odczytu są prawdziwe? (3 poprawne)",
                new List<IAnswer>
                {
                    new Answer("AsNoTracking() zmniejsza narzut, bo EF nie śledzi zmian encji", true),
                    new Answer("Include() może zwiększyć ilość pobranych danych, ale upraszcza ładowanie relacji", true),
                    new Answer("Filtrowanie (Where) po stronie bazy zwykle jest lepsze niż pobranie wszystkiego i filtrowanie w pamięci", true),
                    new Answer("AsNoTracking() automatycznie zapisuje zmiany do bazy", false),
                });

            // 4) multi (2 poprawne)
            var q4 = new Question(
                "Dlaczego w domenie używamy IReadOnlyList zamiast List? (może być kilka poprawnych)",
                new List<IAnswer>
                {
                    new Answer("Żeby ograniczyć możliwość modyfikacji kolekcji z zewnątrz", true),
                    new Answer("Żeby utrzymać kontrolę stanu agregatu (np. Quiz jako Aggregate Root)", true),
                    new Answer("Bo List nie działa w .NET", false),
                    new Answer("Bo IReadOnlyList zawsze jest szybsze od List", false),
                });

            // 5) single
            var q5 = new Question(
                "Co oznacza zasada SRP (Single Responsibility Principle)?",
                new List<IAnswer>
                {
                    new Answer("Klasa powinna mieć jeden powód do zmiany (jedną odpowiedzialność)", true),
                    new Answer("W projekcie może być tylko jedna klasa", false),
                    new Answer("Metoda musi mieć jedną linijkę kodu", false),
                    new Answer("UI i logika muszą być w jednym pliku", false),
                });

            return new Quiz(
                title: "Quiz Trudny — Architektura i EF Core (mix)",
                description: "Poziom: TRUDNY • Czas: ~8–12 min • Enkapsulacja, SRP, wydajność EF Core, podejście warstwowe.",
                questions: new List<IQuestion> { q1, q2, q3, q4, q5 }
            );
        }
    }
}
