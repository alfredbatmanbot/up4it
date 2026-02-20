using Supabase;
using Supabase.Postgrest;
using Up4It.Models;

namespace Up4It.Services;

public class SupabaseService
{
    private readonly Client _client;
    private bool _initialized = false;

    public SupabaseService()
    {
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? 
                  throw new Exception("SUPABASE_URL not configured");
        var key = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? 
                  throw new Exception("SUPABASE_KEY not configured");

        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        _client = new Client(url, key, options);
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        
        await _client.InitializeAsync();
        _initialized = true;
    }

    public Client Client => _client;

    // User/Profile methods
    public async Task<Profile?> GetCurrentProfile()
    {
        var user = _client.Auth.CurrentUser;
        if (user == null) return null;

        var response = await _client
            .From<Profile>()
            .Where(p => p.Id == user.Id)
            .Single();

        return response;
    }

    // Event methods
    public async Task<Event> CreateEvent(Event newEvent)
    {
        var user = _client.Auth.CurrentUser;
        if (user == null) throw new Exception("User not authenticated");

        newEvent.CreatorId = user.Id;
        newEvent.CreatedAt = DateTime.UtcNow;
        newEvent.UpdatedAt = DateTime.UtcNow;

        var response = await _client
            .From<Event>()
            .Insert(newEvent);

        return response.Models.First();
    }

    public async Task<List<Event>> GetTodaysEvents()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var response = await _client
            .From<Event>()
            .Where(e => e.StartTime >= today && e.StartTime < tomorrow)
            .Where(e => e.Status == "open")
            .Order(x => x.StartTime, Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<List<Event>> GetMyEvents()
    {
        var user = _client.Auth.CurrentUser;
        if (user == null) return new List<Event>();

        // Get events created by me
        var myCreated = await _client
            .From<Event>()
            .Where(e => e.CreatorId == user.Id)
            .Where(e => e.Status == "open")
            .Order(x => x.StartTime, Constants.Ordering.Ascending)
            .Get();

        // Get events I've accepted
        var myInvites = await _client
            .From<EventInvite>()
            .Where(i => i.UserId == user.Id)
            .Where(i => i.Status == "accepted")
            .Get();

        var invitedEventIds = myInvites.Models.Select(i => i.EventId).ToList();
        
        if (invitedEventIds.Any())
        {
            var invitedEvents = await _client
                .From<Event>()
                .Filter("id", Constants.Operator.In, invitedEventIds)
                .Where(e => e.Status == "open")
                .Get();

            // Combine and dedupe
            var allEvents = myCreated.Models
                .Concat(invitedEvents.Models)
                .DistinctBy(e => e.Id)
                .OrderBy(e => e.StartTime)
                .ToList();

            return allEvents;
        }

        return myCreated.Models;
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

        var response = await _client
            .From<EventInvite>()
            .Insert(invite);

        return response.Models.First();
    }

    public async Task<EventInvite> UpdateInviteStatus(string inviteId, string status)
    {
        var invite = await _client
            .From<EventInvite>()
            .Where(i => i.Id == inviteId)
            .Single();

        if (invite == null) throw new Exception("Invite not found");

        invite.Status = status;
        invite.RespondedAt = DateTime.UtcNow;

        var updated = await _client
            .From<EventInvite>()
            .Update(invite);

        return updated.Models.First();
    }

    public async Task<List<EventInvite>> GetEventInvites(string eventId)
    {
        var response = await _client
            .From<EventInvite>()
            .Where(i => i.EventId == eventId)
            .Get();

        return response.Models;
    }

    public async Task<int> GetAttendeeCount(string eventId)
    {
        var invites = await GetEventInvites(eventId);
        return invites.Count(i => i.Status == "accepted");
    }

    // Authentication methods
    public async Task<bool> SignInWithPhone(string phone)
    {
        try
        {
            await _client.Auth.SignIn(Supabase.Gotrue.Constants.Provider.Phone, 
                new Supabase.Gotrue.SignInOptions { Phone = phone });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> VerifyOtp(string phone, string token)
    {
        try
        {
            await _client.Auth.VerifyOTP(phone, token, Supabase.Gotrue.Constants.EmailOtpType.SMS);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsAuthenticated => _client.Auth.CurrentUser != null;

    public async Task SignOut()
    {
        await _client.Auth.SignOut();
    }
}
