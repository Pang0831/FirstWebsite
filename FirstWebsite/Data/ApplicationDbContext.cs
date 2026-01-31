using FirstWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FirstWebsite.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}