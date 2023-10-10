using System.ComponentModel.DataAnnotations;

namespace AuroraLibrary.Models;

public class Item
{
    [Key] public int Id { get; set; }
    public User? Owner { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? Stat1 { get; set; }
    public int? Stat2 { get; set; }
    public int? Stat3 { get; set; }
    public int? Stat4 { get; set; }
}