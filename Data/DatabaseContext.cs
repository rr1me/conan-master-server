using conan_master_server.Entities;
using conan_master_server.Models;
using Microsoft.EntityFrameworkCore;

namespace conan_master_server.Data;

public class DatabaseContext : DbContext
{
    public DbSet<ConanUser> Users { get; set; }
    public DbSet<ServerEntity> Servers { get; set; }
    
    public Guid InstanceId { get; } = Guid.NewGuid();

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}