namespace Open.OneDrive;

public class Quota
{
    [JsonPropertyName("total")]
    public long Total { get; set; }
    
    [JsonPropertyName("used")]
    public long Used { get; set; }
    
    [JsonPropertyName("remaining")]
    public long Remaining { get; set; }
    
    [JsonPropertyName("deleted")]
    public long Deleted { get; set; }
    
    [JsonPropertyName("state")]
    public string State { get; set; } //"normal | nearing | critical | exceeded"
}
