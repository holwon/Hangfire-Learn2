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
        // 实际上 Hangfire 添加任务后会返回 Job 的 id
        var jobId = backgroundJobClient.Schedule(
            () => Console.WriteLine("This name is " + personName),
            TimeSpan.FromSeconds(5)
        );
        // 我们可以利用这个 id 做一些后续任务
        backgroundJobClient.ContinueJobWith(
            jobId,
            () => Console.WriteLine($"The job {jobId} has finished")
        );
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
