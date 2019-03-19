using System.Collections.Generic;
using archive.Data.Entities;
using archive.Services;
using NUnit.Framework;

namespace archive.Tests.Services
{
    public class TaskServiceTest
    {
        public class TasksetServiceTest
        {
            [Test]
            public async System.Threading.Tasks.Task FindNoneWhenYearWithoutMatch()
            {
                // given
                var course = new Course {Id = 1, Name = "firstc"};
                var taskset1 = new Taskset {Id = 1, Name = "firstt", Year = 1, Course = course};
                var taskset1_ = new Taskset {Id = 2, Name = "firstt", Year = 2, Course = course};
                var taskset2 = new Taskset {Id = 3, Name = "secondt", Year = 1, Course = course};

                var repo = TestUtils.MockRepository(
                    tasks: new List<Task>
                    {
                        new Task {Id = 1, Taskset = taskset1},
                        new Task {Id = 2, Taskset = taskset1_},
                        new Task {Id = 3, Taskset = taskset2},
                        new Task {Id = 4, Taskset = taskset2},
                    }
                );
                var tested = new TaskService(repo.Object);

                // when
                var result = await tested.FindForTasksetAsync("firstc", "firstt", 3);

                // then
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public async System.Threading.Tasks.Task FindNoneWhenTasksetNameWithoutMatch()
            {
                // given
                var course = new Course {Id = 1, Name = "firstc"};
                var taskset1 = new Taskset {Id = 1, Name = "firstt", Year = 1, Course = course};
                var taskset1_ = new Taskset {Id = 2, Name = "firstt", Year = 2, Course = course};
                var taskset2 = new Taskset {Id = 3, Name = "secondt", Year = 1, Course = course};

                var repo = TestUtils.MockRepository(
                    tasks: new List<Task>
                    {
                        new Task {Id = 1, Taskset = taskset1},
                        new Task {Id = 2, Taskset = taskset1_},
                        new Task {Id = 3, Taskset = taskset2},
                        new Task {Id = 4, Taskset = taskset2},
                    }
                );
                var tested = new TaskService(repo.Object);

                // when
                var result = await tested.FindForTasksetAsync("firstc", "thirdt", 1);

                // then
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public async System.Threading.Tasks.Task FindNoneWhenCourseNameWithoutMatch()
            {
                // given
                var course = new Course {Id = 1, Name = "firstc"};
                var taskset1 = new Taskset {Id = 1, Name = "firstt", Year = 1, Course = course};
                var taskset1_ = new Taskset {Id = 2, Name = "firstt", Year = 2, Course = course};
                var taskset2 = new Taskset {Id = 3, Name = "secondt", Year = 1, Course = course};

                var repo = TestUtils.MockRepository(
                    tasks: new List<Task>
                    {
                        new Task {Id = 1, Taskset = taskset1},
                        new Task {Id = 2, Taskset = taskset1_},
                        new Task {Id = 3, Taskset = taskset2},
                        new Task {Id = 4, Taskset = taskset2},
                    }
                );
                var tested = new TaskService(repo.Object);

                // when
                var result = await tested.FindForTasksetAsync("secondc", "firstt", 1);

                // then
                Assert.AreEqual(0, result.Count);
            }

            [Test]
            public async System.Threading.Tasks.Task FindSome()
            {
                // given
                var course = new Course {Id = 1, Name = "firstc"};
                var taskset1 = new Taskset {Id = 1, Name = "firstt", Year = 1, Course = course};
                var taskset1_ = new Taskset {Id = 2, Name = "firstt", Year = 1, Course = course};
                var taskset2 = new Taskset {Id = 3, Name = "secondt", Year = 2, Course = course};

                var repo = TestUtils.MockRepository(
                    tasks: new List<Task>
                    {
                        new Task {Id = 1, Taskset = taskset1},
                        new Task {Id = 2, Taskset = taskset1_},
                        new Task {Id = 3, Taskset = taskset2},
                        new Task {Id = 4, Taskset = taskset2},
                    }
                );
                var tested = new TaskService(repo.Object);

                // when
                var result = await tested.FindForTasksetAsync("firstc", "secondt", 2);

                // then
                Assert.AreEqual(2, result.Count);
            }

            [Test]
            public async System.Threading.Tasks.Task FindAll()
            {
                // given
                var course = new Course {Id = 1, Name = "firstc"};
                var taskset1 = new Taskset {Id = 1, Name = "firstt", Year = 1, Course = course};
                var taskset1_ = new Taskset {Id = 2, Name = "firstt", Year = 2, Course = course};
                var taskset2 = new Taskset {Id = 3, Name = "firstt", Year = 3, Course = course};

                var repo = TestUtils.MockRepository(
                    tasks: new List<Task>
                    {
                        new Task {Id = 1, Taskset = taskset1},
                        new Task {Id = 2, Taskset = taskset1},
                        new Task {Id = 3, Taskset = taskset1},
                        new Task {Id = 4, Taskset = taskset1},
                    }
                );
                var tested = new TaskService(repo.Object);

                // when
                var result = await tested.FindForTasksetAsync("firstc", "firstt", 1);

                // then
                Assert.AreEqual(4, result.Count);
            }
        }
    }
}