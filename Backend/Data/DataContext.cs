using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class DataContext: DbContext
    {

        public DataContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Comment> Comments { get; set; }
    }
}
