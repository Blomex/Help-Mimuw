using System.Collections.Generic;
using archive.Data.Entities;
using archive.Services;
using NUnit.Framework;
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Services
{
    [TestFixture]
    public class CourseServiceTest
    {
        [Test]
        public async Task FindAllCourses()
        {
            // given
            var content = new List<Course>
            {
                new Course {Id = 1, Name = "first"},
                new Course {Id = 2, Name = "second"}
            };

            var repo = TestUtils.MockRepository(courses: content);
            var tested = new CourseService(repo.Object);

            // when
            var result = await tested.FindAllAsync();

            // then
            Assert.AreEqual(content.Count, result.Count);
            Assert.AreEqual(content[0].Id, result[0].Id);
            Assert.AreEqual(content[0].Name, content[0].Name);
            Assert.AreEqual(content[1].Id, result[1].Id);
            Assert.AreEqual(content[1].Name, content[1].Name);
        }

        [Test]
        public async Task FindNoCourses()
        {
            // given
            var empty = new List<Course>();
            var repo = TestUtils.MockRepository(courses: empty);
            var tested = new CourseService(repo.Object);

            // when
            var result = await tested.FindAllAsync();

            // then
            CollectionAssert.AreEqual(empty, result);
        }
    }
}