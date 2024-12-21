using LiveTC.Maui.Views;

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