namespace Open.OneDrive;

public class SearchQuery
{
    [JsonPropertyName("queryString")]
    public string QueryString { get; set; }
}