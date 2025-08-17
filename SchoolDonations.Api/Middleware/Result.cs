namespace SchoolDonations.API.Middleware;

public class Result
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string StackTrace { get; set; }

    public Result(string message, string source, string stackTrace = "")
    {
        Message = message;
        Source = source;
        StackTrace = stackTrace;
    }
}