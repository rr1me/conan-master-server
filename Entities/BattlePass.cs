using System.ComponentModel.DataAnnotations;

namespace conan_master_server.Entities;

public class BattlePass
{
    [Key]
    public int Id { get; set; }
    public int Level { get; set; }
    public string Items { get; set; }
}