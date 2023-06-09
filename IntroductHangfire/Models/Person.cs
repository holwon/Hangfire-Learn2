using System.ComponentModel.DataAnnotations;

namespace IntroductHangfire.Models;

public class Person
{
    public int Id { get; set; }

    [MaxLength(30)]
    public string? Name { get; set; }
}
