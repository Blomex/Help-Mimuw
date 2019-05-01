using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using archive.Data;
using archive.Data.Entities;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using EntityFrameworkCoreMock;
using Remotion.Linq.Clauses;
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
        
        public static string ToPostString<T>(T instance)
        {
            var sb = new StringBuilder();
            foreach (FieldInfo fi in instance.GetType().GetFields())
            {
                sb.Append(fi.Name + "=" + fi.GetValue(instance) + "&");
            }
            foreach(PropertyInfo p in instance.GetType().GetProperties())
            {
                sb.Append(p.Name + "=" + p.GetValue(instance) + "&");
            }

            sb.Append("__dummy__=1");
            return sb.ToString();
        }
    }
    
    // Source: https://github.com/aspnet/AspNetCore.Docs/tree/master/aspnetcore/test/integration-tests/samples
    public class HtmlHelpers
    {
        public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New()
                .OpenAsync(ResponseFactory, CancellationToken.None);
            return (IHtmlDocument)document;

            void ResponseFactory(VirtualResponse htmlResponse)
            {
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

                MapHeaders(response.Headers);
                MapHeaders(response.Content.Headers);

                htmlResponse.Content(content);

                void MapHeaders(HttpHeaders headers)
                {
                    foreach (var header in headers)
                    {
                        foreach (var value in header.Value)
                        {
                            htmlResponse.Header(header.Key, value);
                        }
                    }
                }
            }
        }
    }
}