using System.ComponentModel.DataAnnotations;

namespace AuroraLibrary.Models;

public class User
{
    [Key] public int Id { get; set; }
    public ulong? DiscordSnowflake { get; set; }
    [Required, StringLength(32)] public string Name { get; set; }
    [Required] public int XP { get; set; } = 0;
}