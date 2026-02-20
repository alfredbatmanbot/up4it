using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Up4It.Models;

[Table("event_invites")]
public class EventInvite : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("event_id")]
    public string EventId { get; set; } = string.Empty;

    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "pending"; // pending, accepted, declined, ignored

    [Column("invited_at")]
    public DateTime InvitedAt { get; set; }

    [Column("responded_at")]
    public DateTime? RespondedAt { get; set; }

    [Column("notified_at")]
    public DateTime? NotifiedAt { get; set; }
}
