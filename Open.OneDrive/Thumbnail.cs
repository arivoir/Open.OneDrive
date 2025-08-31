using System.Text.Json.Serialization;

namespace Open.OneDrive;

public class ThumbnailsSet
{
    [JsonPropertyName("id")]
    public int id { get; set; }
    [JsonPropertyName("small")]
    public Thumbnail small { get; set; }
    [JsonPropertyName("medium")]
    public Thumbnail Medium { get; set; }
    [JsonPropertyName("large")]
    public Thumbnail Large { get; set; }
    [JsonPropertyName("source")]
    public Thumbnail source { get; set; }
}

public class Thumbnail
{
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
}