using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Up4It.Models;
using Up4It.Services;

namespace Up4It.ViewModels;

public partial class EventFeedViewModel : ObservableObject
{
    private readonly SupabaseService _supabase;

    [ObservableProperty]
    private ObservableCollection<Event> todaysEvents = new();

    [ObservableProperty]
    private ObservableCollection<Event> myEvents = new();

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private bool showTodaysEvents = true;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public EventFeedViewModel(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    [RelayCommand]
    async Task LoadEvents()
    {
        if (!_supabase.IsAuthenticated) return;

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            // Load today's events
            var todaysList = await _supabase.GetTodaysEvents();
            TodaysEvents.Clear();
            foreach (var evt in todaysList)
            {
                TodaysEvents.Add(evt);
            }

            // Load my events
            var myList = await _supabase.GetMyEvents();
            MyEvents.Clear();
            foreach (var evt in myList)
            {
                MyEvents.Add(evt);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading events: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task CreateEvent()
    {
        await Shell.Current.GoToAsync("createevent");
    }

    [RelayCommand]
    void ToggleView()
    {
        ShowTodaysEvents = !ShowTodaysEvents;
    }

    [RelayCommand]
    async Task RefreshEvents()
    {
        await LoadEvents();
    }

    public async Task InitializeAsync()
    {
        await _supabase.InitializeAsync();
        await LoadEvents();
    }
}
