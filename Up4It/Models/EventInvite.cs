using System.Text.Json.Serialization;

namespace Up4It.Models;

public class EventInvite
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("event_id")]
    public string EventId { get; set; } = string.Empty;

    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = "pending"; // pending, accepted, declined, ignored

    [JsonPropertyName("invited_at")]
    public DateTime InvitedAt { get; set; }

    [JsonPropertyName("responded_at")]
    public DateTime? RespondedAt { get; set; }

    [JsonPropertyName("notified_at")]
    public DateTime? NotifiedAt { get; set; }
}
