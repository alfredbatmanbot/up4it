using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Up4It.Models;

[Table("profiles")]
public class Profile : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; } = string.Empty;

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [Column("email")]
    public string? Email { get; set; }

    [Column("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [Column("timezone")]
    public string Timezone { get; set; } = "America/Chicago";

    [Column("notification_preferences")]
    public string NotificationPreferences { get; set; } = "{\"push\": true, \"sms\": false}";

    [Column("reliability_score")]
    public int ReliabilityScore { get; set; } = 100;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("last_active_at")]
    public DateTime LastActiveAt { get; set; }
}
