namespace IntroductHangfire.Controllers;

using Hangfire;
using IntroductHangfire.Data;
using IntroductHangfire.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IBackgroundJobClient backgroundJobClient;

    public PeopleController(
        ApplicationDbContext dbContext,
        IBackgroundJobClient backgroundJobClient
    )
    {
        _context = dbContext;
        this.backgroundJobClient = backgroundJobClient;
    }

    [HttpPost("create")]
    public IActionResult PostAsync(string personName)
    {
        // 只有 public 的方法才能给后台调用, 加入后台调用的任务不会阻塞进程
        backgroundJobClient.Enqueue(() => CreatePersonAsync(personName));
        return Ok();
    }

    public async Task CreatePersonAsync(string personName)
    {
        Console.WriteLine("Adding person: " + personName);
        var person = new Person();
        _context.Add(person);
        // 假设的耗时任务
        await Task.Delay(5000);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Add the person: {person}");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
