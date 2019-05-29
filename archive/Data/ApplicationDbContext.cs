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
        public DbSet<SolutionVersion> SolutionsVersions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserAvatar> Avatars { get; set; }

        public DbSet<File> Files { get; set; }

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
                    entity.HasAlternateKey(e => new {e.IdSolution, e.NameUser});
                });

            builder.Entity<Course>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.ShortcutCode).IsUnique();

                    entity.Property(e => e.Name).IsRequired();

                    entity.HasMany(e => e.Tasksets).WithOne();
                });

            builder.Entity<Taskset>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => new {e.CourseId, e.ShortcutCode}).IsUnique().ForNpgsqlHasMethod("btree");

                    entity.Property(e => e.Type).IsRequired();
                    entity.Property(e => e.Year).IsRequired();
                    entity.Property(e => e.Name).IsRequired();

                    entity.HasOne(e => e.Course)
                        .WithMany(e => e.Tasksets)
                        .HasForeignKey(e => e.CourseId);
                });

            builder.Entity<Tag>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.TaskId).IsRequired();
                    entity.Property(e => e.Name).IsRequired();

                    entity.HasOne(e => e.Task)
                        .WithMany(e => e.Tags);
                    entity.HasIndex(e => e.Name);
                    entity.HasIndex(e => e.TaskId);
                });

            builder.Entity<Comment>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.ApplicationUserId).IsRequired();
                    entity.Property(e => e.Content).IsRequired();
                    entity.Property(e => e.CommentDate).IsRequired();

                    entity.HasOne(e => e.Solution)
                        .WithMany(e => e.Comments);
                    entity.HasOne(e => e.ApplicationUser).WithMany(u => u.Comments);
                });

            builder.Entity<Task>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.TasksetId);

                    entity.Property(e => e.Name);
                    entity.Property(e => e.Content);

                    entity.HasOne(e => e.Taskset)
                        .WithMany(e => e.Tasks)
                        .HasForeignKey(e => e.TasksetId);
                });

            builder.Entity<Solution>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.HasOne(e => e.Task)
                        .WithMany(e => e.Solutions)
                        .HasForeignKey(e => e.TaskId);

                    entity.HasOne(s => s.Author).WithMany(a => a.Solutions);
                    entity.HasOne(s => s.CurrentVersion);
                });

            builder.Entity<SolutionVersion>(solutionVersion =>
            {
                solutionVersion.HasKey(v => v.Id);

                solutionVersion.HasOne(v => v.Solution).WithMany(s => s.Versions);
            });

            builder.Entity<UserAvatar>(
                entity =>
                {
                    entity.HasKey(e => e.ApplicationUserId);

                    entity.Property(e => e.ApplicationUserId).IsRequired();
                    entity.Property(e => e.Image).IsRequired();

                    entity.HasOne(e => e.ApplicationUser)
                        .WithOne(e => e.Avatar);
                });

            builder.Entity<File>();

            builder.Entity<TasksetsFiles>(entity =>
            {
                entity.HasOne(tf => tf.File)
                    .WithMany(f => f.TasksetReferers)
                    .HasForeignKey(tf => tf.FileId);

                entity.HasOne(tf => tf.Taskset)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(tf => tf.TasksetId);

                entity.HasKey(t => new {t.TasksetId, t.FileId});
            });

            builder.Entity<TasksFiles>(entity =>
            {
                entity.HasOne(tf => tf.File)
                    .WithMany(f => f.TasksReferers)
                    .HasForeignKey(tf => tf.FileId);

                entity.HasOne(tf => tf.Task)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(tf => tf.TaskId);

                entity.HasKey(t => new {t.TaskId, t.FileId});
            });

            builder.Entity<SolutionsFiles>(entity =>
            {
                entity.HasOne(tf => tf.File)
                    .WithMany(f => f.SolutionsReferers)
                    .HasForeignKey(tf => tf.FileId);

                entity.HasOne(tf => tf.Solution)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(tf => tf.SolutionId);

                entity.HasKey(t => new {t.SolutionId, t.FileId});
            });
        }

        public Job SaveChangesAsync() => base.SaveChangesAsync();
    }
}
