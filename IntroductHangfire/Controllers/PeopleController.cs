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
    public async Task<IActionResult> PostAsync(string personName)
    {
        // 这样加入的任务是给了hangfire执行(作为后台任务执行)的, 可能这样子不会阻塞?
        backgroundJobClient.Enqueue(() => Console.WriteLine(personName));

        System.Console.WriteLine("Adding person: " + personName);
        var person = new Person();
        _context.Add(person);
        // 假设的耗时任务
        await Task.Delay(5000);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Add the person: {person}");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
