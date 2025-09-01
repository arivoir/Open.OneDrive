namespace Open.OneDrive;

internal class SearchRequest
{
    [JsonPropertyName("entityTypes")]
    public IReadOnlyList<string> EntityTypes { get; internal set; }

    [JsonPropertyName("query")]
    public SearchQuery Query { get; internal set; }
}