using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using ProPresenter7WEB.WebServerApplication;

namespace ProPresenter7WEB.DesktopApplication.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ButtonClicked(object source, RoutedEventArgs args)
        {
            Task.Run(() => new WebServer().StartServer());
        }
    }
}