using System;
using System.Collections.Generic;
using System.Text;
using archive.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Job = System.Threading.Tasks.Task;

namespace archive.Data
{
    public class ApplicationDbContext : IdentityDbContext, IRepository
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Taskset> Tasksets { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Name).IsRequired();

                    entity.HasMany(e => e.Tasksets).WithOne();
                });

            builder.Entity<Taskset>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Type).IsRequired();
                    entity.Property(e => e.Year).IsRequired();
                    entity.Property(e => e.Name).IsRequired();

                    entity.HasOne(e => e.Course)
                        .WithMany()
                        .HasForeignKey(e => e.CourseId);

                    entity.HasMany(e => e.Tasks).WithOne();
                });

            builder.Entity<Task>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Name).IsRequired();
                    entity.Property(e => e.Content);

                    entity.HasOne(e => e.Taskset)
                        .WithMany()
                        .HasForeignKey(e => e.TasksetId);

                    entity.HasMany(e => e.Solutions)
                        .WithOne();
                });

            builder.Entity<Solution>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Content).IsRequired();
             
                    entity.HasOne(e => e.Task)
                        .WithMany()
                        .HasForeignKey(e => e.TaskId);
                });
        }

        public Job SaveChangesAsync() => base.SaveChangesAsync();
    }
}