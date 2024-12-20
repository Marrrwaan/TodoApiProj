using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.Entities;

namespace Infrastructure.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems { get; set; }
}
