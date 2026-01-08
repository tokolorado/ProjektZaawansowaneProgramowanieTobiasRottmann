namespace QuizSystem.Wpf.Infrastructure
{
    /// <summary>
    /// Abstrakcja okien dialogowych (MessageBox).
    /// Dzięki temu ViewModel nie zna WPF-owego MessageBox bezpośrednio.
    /// </summary>
    public interface IDialogService
    {
        bool Confirm(string title, string message);
    }
}
