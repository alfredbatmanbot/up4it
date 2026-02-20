using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Up4It.Models;

[Table("events")]
public class Event : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("creator_id")]
    public string CreatorId { get; set; } = string.Empty;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("location")]
    public string? Location { get; set; }

    [Column("max_attendees")]
    public int? MaxAttendees { get; set; }

    [Column("min_attendees")]
    public int? MinAttendees { get; set; }

    [Column("visibility")]
    public string Visibility { get; set; } = "friends";

    [Column("status")]
    public string Status { get; set; } = "open";

    [Column("rsvp_deadline")]
    public DateTime? RsvpDeadline { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties (not stored in DB)
    public string? CreatorName { get; set; }
    public int AttendeeCount { get; set; }
}
