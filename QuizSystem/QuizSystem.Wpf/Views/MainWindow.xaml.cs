using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizSystem.Wpf.Infrastructure;
using QuizSystem.Wpf.ViewModels;
using QuizSystem.Wpf.Bootstrap;


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

            var db = AppBootstrapper.CreateDbContext();


            // Proste "wstrzyknięcie" zależności bez DI kontenera
            var dialogService = new WpfDialogService();
            DataContext = new MainViewModel(dialogService);
        }
    }
}