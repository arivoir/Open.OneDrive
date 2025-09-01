namespace Open.OneDrive;

public class AsyncJobStatus
{
    [JsonPropertyName("operation")]
    public string Operation { get; set; }

    [JsonPropertyName("percentageComplete")]
    [JsonRequired]
    public double PercentageComplete { get; set; }

    [JsonPropertyName("status")]
    [JsonRequired]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; }

    [JsonPropertyName("statusDescription")]
    public string StatusDescription { get; set; }

    [JsonPropertyName("resourceId")]
    public string ResourceId { get; set; }
}

public enum Status
{
    NotStarted,
    InProgress,
    Completed,
    Failed,
    Cancelled,
    Waiting,
    CancelPending,
}
