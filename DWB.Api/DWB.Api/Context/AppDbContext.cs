using DWB.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DWB.Api.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
