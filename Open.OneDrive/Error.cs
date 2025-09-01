namespace Open.OneDrive;

public class Error
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}
