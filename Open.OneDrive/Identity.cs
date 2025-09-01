namespace Open.OneDrive;

public class IdentitySet
{
    [JsonPropertyName("user")]
    public Identity User { get; set; }

    [JsonPropertyName("application")]
    public Identity Application { get; set; }

    [JsonPropertyName("device")]
    public Identity Device { get; set; }
}

public class Identity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

}
