using System.ComponentModel.DataAnnotations;

namespace conan_master_server.Entities;

public class ServerEntity
{
    [Key]
    public string Id { get; set; }
    public DateTime LastPing { get; set; }

    public string s9 { get; set; }
    public decimal s8 { get; set; }
    public decimal s24 { get; set; }
    public bool s119 { get; set; }
    public bool s117 { get; set; }
    public string ip { get; set; }
    public bool s0 { get; set; }
    public bool s7 { get; set; }
    public decimal s6 { get; set; }
    public bool Private { get; set; }
    public decimal s4 { get; set; }
    public decimal s15 { get; set; }
    public bool s18 { get; set; }
    public decimal sl { get; set; }
    public int buildId { get; set; }
    public string? s17 { get; set; }
    public decimal s30 { get; set; }
    public int maxplayers { get; set; }
    public string kdsUri { get; set; }
    public decimal sz { get; set; }
    public decimal sy { get; set; }
    public string serverUID { get; set; }
    public string name { get; set; }
    public bool s122 { get; set; }
    public string s120 { get; set; }
    public bool csf { get; set; }
    public string? sw { get; set; }
    public string su { get; set; }
    public decimal s22 { get; set; }
    public decimal s23 { get; set; }
    public decimal s21 { get; set; }
    public int so { get; set; }
    public decimal sm { get; set; }
    public bool s25 { get; set; }
    public bool sa { get; set; }
    public decimal sg { get; set; }
    public decimal sf { get; set; }
    public string mapName { get; set; }
    public string displayedmaxplayers { get; set; }
    public decimal sj { get; set; }
    public decimal ss { get; set; }
    public int queryPort { get; set; }
    public bool s05 { get; set; }
    public decimal sk { get; set; }
    public string externaL_SERVER_UID { get; set; }
    public string guid { get; set; }
    public int port { get; set; }
    public string se { get; set; }
    public string? mods { get; set; }
    public string? online { get; set; }
}