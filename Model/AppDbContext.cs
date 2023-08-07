using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;

namespace AlphaOneA.Model
{
    public class AppDbContext: DbContext
    {
        public DbSet<Model> Model { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
       
        {
        }
    }
}

