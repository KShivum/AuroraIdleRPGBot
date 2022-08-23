using System.ComponentModel.DataAnnotations.Schema;
namespace AuroraLibrary.DatabaseModels;

public class Player
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; set; }
    public string PlayerName { get; set; }
    public int Gold { get; set; } = 0;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int Speed { get; set; } = 1;
    public int Strength { get; set; } = 1;
    
}
