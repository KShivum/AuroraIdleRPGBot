using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models;

public class User
{
    [Key]
    public long Id { get; set; }
    public string PlayerName { get; set; }
}