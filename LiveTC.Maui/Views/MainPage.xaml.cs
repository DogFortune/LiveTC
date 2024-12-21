using LiveTC.Maui.ViewModels;
using Microsoft.Maui.Controls;

namespace LiveTC.Maui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}