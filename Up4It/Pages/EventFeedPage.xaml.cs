using Up4It.ViewModels;

namespace Up4It.Pages;

public partial class EventFeedPage : ContentPage
{
    private readonly EventFeedViewModel _viewModel;

    public EventFeedPage(EventFeedViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
