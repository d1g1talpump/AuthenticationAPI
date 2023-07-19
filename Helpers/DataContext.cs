namespace AuthenticationAPI.Helpers;

using Microsoft.EntityFrameworkCore;
using AuthenticationAPI.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string s_AUTHENTICATION_DB_CONNECTION =  
            Environment.GetEnvironmentVariable("AUTHENTICATION_DB_CONNECTION") ?? 
            throw new Exception("Database Connection Error");
            
        options.UseNpgsql(s_AUTHENTICATION_DB_CONNECTION);
    }

    public DbSet<User> Users { get; set; }
    
}