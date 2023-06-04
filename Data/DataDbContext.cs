using FinalProjectShortenURL.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectShortenURL.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<URLModel> Urls { get; set; }
    }
}
