using Up4It.ViewModels;

namespace Up4It.Pages;

public partial class CreateEventPage : ContentPage
{
    public CreateEventPage(CreateEventViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
