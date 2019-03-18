using System.Collections.Generic;
using archive.Data.Entities;
using archive.Data.Enums;
using archive.Services;
using NUnit.Framework;
using Task = System.Threading.Tasks.Task;

namespace archive.Tests.Services
{
    public class TasksetServiceTest
    {
        [Test]
        public async Task FindAllForCourse()
        {
            // given
            var repo = TestUtils.MockRepository(
                tasksets: new List<Taskset>
                {
                    new Taskset
                    {
                        Id = 1, Type = TasksetType.Exam, Year = 1, Name = "first", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                    new Taskset
                    {
                        Id = 2, Type = TasksetType.Test, Year = 2, Name = "second", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                    new Taskset
                    {
                        Id = 3, Type = TasksetType.Test, Year = 2, Name = "third", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                }
            );
            var tested = new TasksetService(repo.Object);

            // when
            var result = await tested.FindForCourseAsync("firstc");

            // then
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task FindSomeForCourse()
        {
            // given
            var repo = TestUtils.MockRepository(
                tasksets: new List<Taskset>
                {
                    new Taskset
                    {
                        Id = 1, Type = TasksetType.Exam, Year = 1, Name = "first", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                    new Taskset
                    {
                        Id = 2, Type = TasksetType.Test, Year = 2, Name = "second", CourseId = 2,
                        Course = new Course {Id = 2, Name = "secondc"},
                    },
                    new Taskset
                    {
                        Id = 3, Type = TasksetType.Test, Year = 2, Name = "third", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                }
            );

            var tested = new TasksetService(repo.Object);

            // when
            var result = await tested.FindForCourseAsync("firstc");

            // then
            Assert.AreEqual(2, result.Count);
        }
        
        [Test]
        public async Task FindNoneForCourse()
        {
            // given
            var repo = TestUtils.MockRepository(
                tasksets: new List<Taskset>
                {
                    new Taskset
                    {
                        Id = 1, Type = TasksetType.Exam, Year = 1, Name = "first", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                    new Taskset
                    {
                        Id = 2, Type = TasksetType.Test, Year = 2, Name = "second", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                    new Taskset
                    {
                        Id = 3, Type = TasksetType.Test, Year = 2, Name = "third", CourseId = 1,
                        Course = new Course {Id = 1, Name = "firstc"},
                    },
                }
            );
            var tested = new TasksetService(repo.Object);

            // when
            var result = await tested.FindForCourseAsync("secondc");

            // then
            Assert.AreEqual(0, result.Count);
        }
    }
}