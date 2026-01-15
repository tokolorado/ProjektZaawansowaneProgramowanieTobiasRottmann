using System.Windows;
using QuizSystem.Wpf.Bootstrap;
using QuizSystem.Wpf.Infrastructure;
using QuizSystem.Wpf.ViewModels;

namespace QuizSystem.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (_, __) =>
            {
                // 1) Migracje + seed
                await AppBootstrapper.EnsureDatabaseCreatedAndSeededAsync();

                // 2) VM
                var dialogService = new WpfDialogService();
                DataContext = new MainViewModel(dialogService);
            };
        }
    }
}
