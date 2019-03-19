using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using archive.Data;
using archive.Data.Entities;
using EntityFrameworkCoreMock;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace archive.Tests
{
    internal static class TestUtils
    {
        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options)
                : base(options)
            {
            }

            public virtual Microsoft.EntityFrameworkCore.DbSet<Course> Courses { get; set; }
            public virtual Microsoft.EntityFrameworkCore.DbSet<Taskset> Tasksets { get; set; }
            public virtual Microsoft.EntityFrameworkCore.DbSet<Data.Entities.Task> Tasks { get; set; }
            public virtual Microsoft.EntityFrameworkCore.DbSet<Solution> Solutions { get; set; }
        }

        public static DbContextOptions<TestDbContext> DummyOptions { get; } =
            new DbContextOptionsBuilder<TestDbContext>().Options;

        public static Mock<IRepository> MockRepository(
            IEnumerable<Course> courses = null,
            IEnumerable<Taskset> tasksets = null,
            IEnumerable<Data.Entities.Task> tasks = null,
            IEnumerable<Solution> solutions = null
        )
        {
            var dbContext = new DbContextMock<TestDbContext>(TestUtils.DummyOptions);
            var coursesDbSet = dbContext.CreateDbSetMock(x => x.Courses, courses);
            var tasksetsDbSet = dbContext.CreateDbSetMock(x => x.Tasksets, tasksets);
            var tasksDbSet = dbContext.CreateDbSetMock(x => x.Tasks, tasks);
            var solutionsDbSet = dbContext.CreateDbSetMock(x => x.Solutions, solutions);

            var repo = new Mock<IRepository>();
            repo.Setup(t => t.Courses).Returns(coursesDbSet.Object);
            repo.Setup(t => t.Tasksets).Returns(tasksetsDbSet.Object);
            repo.Setup(t => t.Tasks).Returns(tasksDbSet.Object);
            repo.Setup(t => t.Solutions).Returns(solutionsDbSet.Object);

            return repo;
        }
    }
}