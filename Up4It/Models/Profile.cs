using System.Text.Json.Serialization;

namespace Up4It.Models;

public class Profile
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = "America/Chicago";

    [JsonPropertyName("notification_preferences")]
    public string NotificationPreferences { get; set; } = "{\"push\": true, \"sms\": false}";

    [JsonPropertyName("reliability_score")]
    public int ReliabilityScore { get; set; } = 100;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("last_active_at")]
    public DateTime LastActiveAt { get; set; }
}
