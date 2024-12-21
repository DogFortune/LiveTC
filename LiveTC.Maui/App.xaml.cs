using LiveTC.Maui.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LiveTC.Maui;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        InitializeComponent();
        var mainPage = services.GetRequiredService<MainPage>();
        MainPage = new NavigationPage(mainPage);
    }
}