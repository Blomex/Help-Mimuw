using System;
using System.Collections.Generic;
using System.Text;
using archive.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Job = System.Threading.Tasks.Task;

namespace archive.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IRepository
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Taskset> Tasksets { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        public DbSet<Rating> Ratings {get; set;}

        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rating>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                });

            builder.Entity<Course>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasAlternateKey(e => e.Name);

                    entity.HasMany(e => e.Tasksets).WithOne();
                });

            builder.Entity<Taskset>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasAlternateKey(e => new {e.CourseId, e.Name, e.Year});

                    entity.Property(e => e.Type).IsRequired();
                    entity.Property(e => e.Year).IsRequired();
                    entity.Property(e => e.Name).IsRequired();

                    entity.HasOne(e => e.Course)
                        .WithMany(e => e.Tasksets)
                        .HasForeignKey(e => e.CourseId);
                });

            builder.Entity<Comment>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.ApplicationUserId).IsRequired();
                    entity.Property(e => e.content).IsRequired();
                    entity.Property(e => e.CommentDate).IsRequired();

                    entity.HasOne(e => e.Solution)
                        .WithMany(e => e.Comments);
                    entity.HasOne(e => e.ApplicationUser).WithMany(u => u.Comments);

                });

            builder.Entity<Task>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Name).IsRequired();
                    entity.Property(e => e.Content);

                    entity.HasOne(e => e.Taskset)
                        .WithMany(e => e.Tasks)
                        .HasForeignKey(e => e.TasksetId);
                });

            builder.Entity<Solution>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Content).IsRequired();

                    entity.HasOne(e => e.Task)
                        .WithMany(e => e.Solutions)
                        .HasForeignKey(e => e.TaskId);
                });
        }

        public Job SaveChangesAsync() => base.SaveChangesAsync();
    }
}