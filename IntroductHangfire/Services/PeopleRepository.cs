using IntroductHangfire.Data;
using IntroductHangfire.Models;
using Microsoft.Extensions.Logging;

namespace IntroductHangfire.Services;

public interface IPeopleRepository
{
    public Task CreatePersonAsync(string personName);
}

public class PeopleRepository : IPeopleRepository
{
    private ApplicationDbContext _context;
    private ILogger<PeopleRepository> _logger;

    public PeopleRepository(ApplicationDbContext context, ILogger<PeopleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CreatePersonAsync(string personName)
    {
        _logger.LogInformation("Adding person: " + personName);
        var person = new Person() { Name = personName };
        _context.Add(person);
        // 假设的耗时任务
        await Task.Delay(5000);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Add the person: {person}");
    }
}
