using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Up4It.Models;

namespace Up4It.Services;

public class SupabaseService
{
    private readonly HttpClient _httpClient;
    private readonly string _supabaseUrl;
    private readonly string _supabaseKey;
    private bool _initialized = false;

    public SupabaseService()
    {
        _supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? 
                       throw new Exception("SUPABASE_URL not configured");
        _supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? 
                       throw new Exception("SUPABASE_KEY not configured");

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri($"{_supabaseUrl}/rest/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseKey);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public Task InitializeAsync()
    {
        _initialized = true;
        return Task.CompletedTask;
    }

    // Event methods
    public async Task<Event> CreateEvent(Event newEvent)
    {
        newEvent.CreatedAt = DateTime.UtcNow;
        newEvent.UpdatedAt = DateTime.UtcNow;

        var response = await _httpClient.PostAsJsonAsync("events", newEvent);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<Event>>();
        return result?.FirstOrDefault() ?? throw new Exception("Failed to create event");
    }

    public async Task<List<Event>> GetTodaysEvents()
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var tomorrow = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

        var response = await _httpClient.GetAsync(
            $"events?start_time=gte.{today}T00:00:00Z&start_time=lt.{tomorrow}T00:00:00Z&status=eq.open&order=start_time.asc");
        
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<Event>>();
        return events ?? new List<Event>();
    }

    public async Task<List<Event>> GetMyEvents()
    {
        // For now, return all events
        // TODO: Add auth and filter by user
        var response = await _httpClient.GetAsync("events?status=eq.open&order=start_time.asc");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<Event>>();
        return events ?? new List<Event>();
    }

    // Invite methods
    public async Task<EventInvite> CreateInvite(string eventId, string userId)
    {
        var invite = new EventInvite
        {
            EventId = eventId,
            UserId = userId,
            Status = "pending",
            InvitedAt = DateTime.UtcNow
        };

        var response = await _httpClient.PostAsJsonAsync("event_invites", invite);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<EventInvite>>();
        return result?.FirstOrDefault() ?? throw new Exception("Failed to create invite");
    }

    public async Task<EventInvite> UpdateInviteStatus(string inviteId, string status)
    {
        var update = new { status, responded_at = DateTime.UtcNow };

        var response = await _httpClient.PatchAsync(
            $"event_invites?id=eq.{inviteId}",
            JsonContent.Create(update));
        
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<EventInvite>>();
        return result?.FirstOrDefault() ?? throw new Exception("Failed to update invite");
    }

    public async Task<List<EventInvite>> GetEventInvites(string eventId)
    {
        var response = await _httpClient.GetAsync($"event_invites?event_id=eq.{eventId}");
        response.EnsureSuccessStatusCode();

        var invites = await response.Content.ReadFromJsonAsync<List<EventInvite>>();
        return invites ?? new List<EventInvite>();
    }

    public async Task<int> GetAttendeeCount(string eventId)
    {
        var invites = await GetEventInvites(eventId);
        return invites.Count(i => i.Status == "accepted");
    }

    // Profile methods
    public async Task<Profile?> GetCurrentProfile()
    {
        // TODO: Implement auth
        // For now, return null
        return null;
    }

    // Auth methods (placeholder for now)
    public bool IsAuthenticated => false;

    public Task<bool> SignInWithPhone(string phone)
    {
        // TODO: Implement Supabase Auth
        return Task.FromResult(false);
    }

    public Task<bool> VerifyOtp(string phone, string token)
    {
        // TODO: Implement Supabase Auth
        return Task.FromResult(false);
    }

    public Task SignOut()
    {
        // TODO: Implement Supabase Auth
        return Task.CompletedTask;
    }
}
