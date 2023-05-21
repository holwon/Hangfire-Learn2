namespace IntroductHangfire.Controllers;

using IntroductHangfire.Data;
using IntroductHangfire.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PeopleController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(string personName)
    {
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
