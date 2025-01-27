using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ProPresenter7WEB.DesktopApplication.ViewModels.Controls;

namespace ProPresenter7WEB.DesktopApplication.Views.Controls;

public partial class ProPresenterControlView : UserControl
{
    public ProPresenterControlView()
    {
        InitializeComponent();
        DataContext = App.Services?.GetRequiredService<ProPresenterControlViewModel>();
    }
}