using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuroraLibrary.DatabaseModels;

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)] [Column(TypeName = "VARCHAR(255)")]
    public ulong Id { get; set; }
    [Required]
    public string PlayerName { get; set; }
    [Required]
    public int Gold { get; set; } = 0;
    [Required]
    public int Level { get; set; } = 1;
    [Required]
    public int Experience { get; set; } = 0;
    [Required]
    public int Speed { get; set; } = 1;
    [Required]
    public int Strength { get; set; } = 1;
    public ICollection<Item> Items { get; set; }

}
