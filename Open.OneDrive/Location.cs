using System.Text.Json.Serialization;

namespace Open.OneDrive;

public class Location
{
    [JsonPropertyName("latitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Longitude { get; set; }

    [JsonPropertyName("altitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Altitude { get; set; }
}
