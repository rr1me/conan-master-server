using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace conan_master_server.Entities;

[Table("User")]
public class ConanUser
{
    [Key]
    public long Id { get; set; }
    public long SteamId { get; set; }
    public string EntityId { get; set; }
    public string Ip { get; set; }
    public string PlayfabId { get; set; }
    public string PublisherId { get; set; }
    public string SpecId { get; set; }
    public string Ticket { get; set; }
    public string Token { get; set; }
    public string Username { get; set; }
    
    public DateTime CreationDate { get; set; }
    public int Identifier { get; set; }
    
    public int BpLevel { get; set; }
}