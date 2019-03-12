using System;
using System.Collections.Generic;
using System.Text;
using archive.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace archive.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        
        public DbSet<Course> Courses { get; set; }
        public DbSet<Taskset> Tasksets { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Solution> Solutions { get; set; }
          
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}