namespace IntroductHangfire.Services;

public interface ITimeService
{
    public void PrintNow();
}

public class TimeService : ITimeService
{
    private readonly ILogger<TimeService> _logger;

    public TimeService(ILogger<TimeService> logger)
    {
        _logger = logger;
    }

    public void PrintNow()
    {
        _logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
    }
}
