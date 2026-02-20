using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Up4It.Models;
using Up4It.Services;

namespace Up4It.ViewModels;

public partial class CreateEventViewModel : ObservableObject
{
    private readonly SupabaseService _supabase;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string location = string.Empty;

    [ObservableProperty]
    private DateTime startTime = DateTime.Now.AddHours(2);

    [ObservableProperty]
    private int? maxAttendees;

    [ObservableProperty]
    private bool isCreating = false;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public CreateEventViewModel(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    [RelayCommand]
    async Task CreateEvent()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Please enter a title";
            return;
        }

        if (StartTime < DateTime.Now)
        {
            ErrorMessage = "Start time must be in the future";
            return;
        }

        IsCreating = true;
        ErrorMessage = string.Empty;

        try
        {
            var newEvent = new Event
            {
                Title = Title,
                Description = Description,
                Location = Location,
                StartTime = StartTime,
                MaxAttendees = MaxAttendees,
                Visibility = "friends",
                Status = "open"
            };

            var created = await _supabase.CreateEvent(newEvent);

            // Navigate back or show success
            await Shell.Current.DisplayAlert("Success", "Event created!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating event: {ex.Message}";
        }
        finally
        {
            IsCreating = false;
        }
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
