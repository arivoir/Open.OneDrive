namespace Open.OneDrive;

public class UploadSession
{
    [JsonPropertyName("uploadUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string UploadUrl { get; set; }

    [JsonPropertyName("expirationDateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ExpirationDateTime { get; set; }

    [JsonPropertyName("nextExpectedRanges")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[] NextExpectedRanges { get; set; }
}