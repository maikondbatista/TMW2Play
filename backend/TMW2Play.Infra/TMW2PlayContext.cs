using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TMW2Play.Infra
{
    public class TMW2PlayContext : DbContext
    {
        public TMW2PlayContext(DbContextOptions<TMW2PlayContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
