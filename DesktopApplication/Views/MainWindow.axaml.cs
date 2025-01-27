using Avalonia.Controls;
using ProPresenter7WEB.DesktopApplication.ViewModels;

namespace ProPresenter7WEB.DesktopApplication.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}