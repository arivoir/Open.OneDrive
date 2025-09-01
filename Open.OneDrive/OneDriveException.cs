namespace Open.OneDrive;

public class OneDriveException : Exception
{
    public OneDriveException(Exception exc)
        : base(exc.Message, exc)
    {
        Error = new Error() { Message = exc.Message };
    }

    public OneDriveException(string message) : base(message)
    {
        Error = new Error() { Message = message };
    }

    public OneDriveException(Error error)
        : base(error.Message)
    {
        Error = error;
    }

    public Error Error { get; private set; }
}
