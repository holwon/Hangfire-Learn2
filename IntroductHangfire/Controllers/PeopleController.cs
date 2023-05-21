namespace IntroductHangfire.Controllers;

using Hangfire;
using IntroductHangfire.Data;
using IntroductHangfire.Models;
using IntroductHangfire.Services;
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
        backgroundJobClient.Enqueue<IPeopleRepository>(
            repository => repository.CreatePersonAsync(personName)
        );
        return Ok();
    }

    [HttpPost("schedule")]
    public IActionResult Schedule(string personName)
    {
        backgroundJobClient.Schedule(
            () => Console.WriteLine("This name is " + personName),
            TimeSpan.FromSeconds(5)
        );
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
