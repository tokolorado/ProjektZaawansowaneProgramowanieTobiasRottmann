using System.Windows;

namespace QuizSystem.Wpf.Infrastructure
{
    /// <summary>
    /// Implementacja dialogów dla WPF.
    /// </summary>
    public class WpfDialogService : IDialogService
    {
        public bool Confirm(string title, string message)
        {
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
    }
}
