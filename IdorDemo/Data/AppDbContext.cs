using Microsoft.EntityFrameworkCore;
using RazorIdorDemo.Models;

namespace RazorIdorDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<UserTask> UserTasks => Set<UserTask>();
}