using System.Text.Json.Serialization;

namespace Up4It.Models;

public class Event
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("creator_id")]
    public string CreatorId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_time")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public DateTime? EndTime { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("max_attendees")]
    public int? MaxAttendees { get; set; }

    [JsonPropertyName("min_attendees")]
    public int? MinAttendees { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; } = "friends";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "open";

    [JsonPropertyName("rsvp_deadline")]
    public DateTime? RsvpDeadline { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties (not stored in DB)
    [JsonIgnore]
    public string? CreatorName { get; set; }
    
    [JsonIgnore]
    public int AttendeeCount { get; set; }
}
