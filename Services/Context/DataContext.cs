using BackendRedo.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendRedo.Services.Context;

    public class DataContext : DbContext
    {
        public DbSet<UserModel> UserInfo { get; set; }
        public DbSet<FormModel> FormInfo { get; set; }

        public DataContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
