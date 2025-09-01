namespace Open.OneDrive;

public class Drives
{
    [JsonPropertyName("value")]
    public List<Drive> Value { get; set; }

    [JsonPropertyName("@odata.nextLink")]
    public string NextLink { get; set; }
}

public class Drive
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("driveType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DriveType DriveType { get; set; }

    [JsonPropertyName("owner")]
    public IdentitySet Owner { get; set; }

    [JsonPropertyName("quota")]
    public Quota Quota { get; set; }
}

public enum DriveType
{
    Personal,
    Business,
}
