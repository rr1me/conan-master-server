﻿using conan_master_server.Entities;
using Microsoft.EntityFrameworkCore;

namespace conan_master_server.Data;

public class DatabaseContext : DbContext
{
    public DbSet<ConanUser> Users { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}