namespace CW2.Services;

public class OperationResult
{
    public bool Success { get; }
    public string Message { get; }

    private OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResult Ok(string message)
        => new OperationResult(true, message);

    public static OperationResult Fail(string message)
        => new OperationResult(false, message);

    public override string ToString()
    {
        return $"{(Success ? "OK" : "ERROR")}: {Message}";
    }
}