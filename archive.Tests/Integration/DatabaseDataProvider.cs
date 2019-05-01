using System;
using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;

namespace archive.Tests.Integration
{
    public class DatabaseDataProvider
    {
        // FIXME? Narazie nie używany, testy idą na żywej bazie
        public static void InitializeForTests(ApplicationDbContext db)
        {
            // Courses
            db.Courses.Add(new Course {Id = 1, Name = "Rachunek Prawdopodobieństwa"});
            db.Courses.Add(new Course {Id = 2, Name = "Inżynieria Oprogramowania"});
            db.Courses.Add(new Course {Id = 3, Name = "Funkcje Analityczne"});
            db.Courses.Add(new Course {Id = 4, Name = "Algebra"});
            db.Courses.Add(new Course {Id = 5, Name = "Systemy Operacyjne"});
            db.Courses.Add(new Course {Id = 6, Name = "Programowanie Współbieżne"});
            db.Courses.Add(new Course {Id = 7, Name = "Sieci Komputerowe"});
            db.Courses.Add(new Course {Id = 8, Name = "Języki, Automaty i Obliczenia"});

            // Tasksets
            db.Tasksets.Add(new Taskset
                {Id = 1, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 2, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 3, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 4, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 5, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 6, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 1});
            db.Tasksets.Add(new Taskset
                {Id = 7, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 8, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 9, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 10, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 11, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 12, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 2});
            db.Tasksets.Add(new Taskset
                {Id = 13, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 14, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 15, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 16, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 17, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 18, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 3});
            db.Tasksets.Add(new Taskset
                {Id = 19, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 20, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 21, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 22, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 23, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 24, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 4});
            db.Tasksets.Add(new Taskset
                {Id = 25, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 26, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 27, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 28, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 29, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 30, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 5});
            db.Tasksets.Add(new Taskset
                {Id = 31, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 32, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 33, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 34, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 35, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 36, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 6});
            db.Tasksets.Add(new Taskset
                {Id = 37, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 38, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 39, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 40, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 41, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 42, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 7});
            db.Tasksets.Add(new Taskset
                {Id = 43, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin             ", CourseId = 8});
            db.Tasksets.Add(new Taskset
                {Id = 44, Type = TasksetType.Exam, Year = 2018, Name = "Egzamin Poprawkowy  ", CourseId = 8});
            db.Tasksets.Add(new Taskset
                {Id = 45, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium           ", CourseId = 8});
            db.Tasksets.Add(new Taskset
                {Id = 46, Type = TasksetType.Test, Year = 2018, Name = "Kolokwium Poprawkowe", CourseId = 8});
            db.Tasksets.Add(new Taskset
                {Id = 47, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin             ", CourseId = 8});
            db.Tasksets.Add(new Taskset
                {Id = 48, Type = TasksetType.Exam, Year = 2017, Name = "Egzamin Poprawkowy  ", CourseId = 8});

            // Tasks
            db.Tasks.Add(new Task
            {
                Id = 1, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 1
            });
            db.Tasks.Add(new Task
            {
                Id = 2, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 1
            });
            db.Tasks.Add(new Task
            {
                Id = 3, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 1
            });
            db.Tasks.Add(new Task
            {
                Id = 4, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 1
            });
            db.Tasks.Add(new Task
            {
                Id = 5, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 6, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 7, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 8, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 9, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 10, Name = "Zadanie 6.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 2
            });
            db.Tasks.Add(new Task
            {
                Id = 11, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 3
            });
            db.Tasks.Add(new Task
            {
                Id = 12, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 3
            });
            db.Tasks.Add(new Task
            {
                Id = 13, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 3
            });
            db.Tasks.Add(new Task
            {
                Id = 14, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 3
            });
            db.Tasks.Add(new Task
            {
                Id = 15, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 4
            });
            db.Tasks.Add(new Task
            {
                Id = 16, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 4
            });
            db.Tasks.Add(new Task
            {
                Id = 17, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 4
            });
            db.Tasks.Add(new Task
            {
                Id = 18, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 4
            });
            db.Tasks.Add(new Task
            {
                Id = 19, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 4
            });
            db.Tasks.Add(new Task
            {
                Id = 20, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 21, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 22, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 23, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 24, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 25, Name = "Zadanie 6.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 5
            });
            db.Tasks.Add(new Task
            {
                Id = 26, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 6
            });
            db.Tasks.Add(new Task
            {
                Id = 27, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 6
            });
            db.Tasks.Add(new Task
            {
                Id = 28, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 6
            });
            db.Tasks.Add(new Task
            {
                Id = 29, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 6
            });
            db.Tasks.Add(new Task
            {
                Id = 30, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 7
            });
            db.Tasks.Add(new Task
            {
                Id = 31, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 7
            });
            db.Tasks.Add(new Task
            {
                Id = 32, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 7
            });
            db.Tasks.Add(new Task
            {
                Id = 33, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 7
            });
            db.Tasks.Add(new Task
            {
                Id = 34, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 8
            });
            db.Tasks.Add(new Task
            {
                Id = 35, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 8
            });
            db.Tasks.Add(new Task
            {
                Id = 36, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 8
            });
            db.Tasks.Add(new Task
            {
                Id = 37, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 9
            });
            db.Tasks.Add(new Task
            {
                Id = 38, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 9
            });
            db.Tasks.Add(new Task
            {
                Id = 39, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 9
            });
            db.Tasks.Add(new Task
            {
                Id = 40, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 9
            });
            db.Tasks.Add(new Task
            {
                Id = 41, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 10
            });
            db.Tasks.Add(new Task
            {
                Id = 42, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 10
            });
            db.Tasks.Add(new Task
            {
                Id = 43, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 10
            });
            db.Tasks.Add(new Task
            {
                Id = 44, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 10
            });
            db.Tasks.Add(new Task
            {
                Id = 45, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 11
            });
            db.Tasks.Add(new Task
            {
                Id = 46, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 11
            });
            db.Tasks.Add(new Task
            {
                Id = 47, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 11
            });
            db.Tasks.Add(new Task
            {
                Id = 48, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 11
            });
            db.Tasks.Add(new Task
            {
                Id = 49, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 11
            });
            db.Tasks.Add(new Task
            {
                Id = 50, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 12
            });
            db.Tasks.Add(new Task
            {
                Id = 51, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 12
            });
            db.Tasks.Add(new Task
            {
                Id = 52, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 12
            });
            db.Tasks.Add(new Task
            {
                Id = 53, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 12
            });
            db.Tasks.Add(new Task
            {
                Id = 54, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 13
            });
            db.Tasks.Add(new Task
            {
                Id = 55, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 13
            });
            db.Tasks.Add(new Task
            {
                Id = 56, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 13
            });
            db.Tasks.Add(new Task
            {
                Id = 57, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 13
            });
            db.Tasks.Add(new Task
            {
                Id = 58, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 14
            });
            db.Tasks.Add(new Task
            {
                Id = 59, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 14
            });
            db.Tasks.Add(new Task
            {
                Id = 60, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 14
            });
            db.Tasks.Add(new Task
            {
                Id = 61, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 14
            });
            db.Tasks.Add(new Task
            {
                Id = 62, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 14
            });
            db.Tasks.Add(new Task
            {
                Id = 63, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 15
            });
            db.Tasks.Add(new Task
            {
                Id = 64, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 15
            });
            db.Tasks.Add(new Task
            {
                Id = 65, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 15
            });
            db.Tasks.Add(new Task
            {
                Id = 66, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 16
            });
            db.Tasks.Add(new Task
            {
                Id = 67, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 16
            });
            db.Tasks.Add(new Task
            {
                Id = 68, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 16
            });
            db.Tasks.Add(new Task
            {
                Id = 69, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 16
            });
            db.Tasks.Add(new Task
            {
                Id = 70, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 71, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 72, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 73, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 74, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 75, Name = "Zadanie 6.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 17
            });
            db.Tasks.Add(new Task
            {
                Id = 76, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 18
            });
            db.Tasks.Add(new Task
            {
                Id = 77, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 18
            });
            db.Tasks.Add(new Task
            {
                Id = 78, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 18
            });
            db.Tasks.Add(new Task
            {
                Id = 79, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 18
            });
            db.Tasks.Add(new Task
            {
                Id = 80, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 19
            });
            db.Tasks.Add(new Task
            {
                Id = 81, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 19
            });
            db.Tasks.Add(new Task
            {
                Id = 82, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 19
            });
            db.Tasks.Add(new Task
            {
                Id = 83, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 19
            });
            db.Tasks.Add(new Task
            {
                Id = 84, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 20
            });
            db.Tasks.Add(new Task
            {
                Id = 85, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 20
            });
            db.Tasks.Add(new Task
            {
                Id = 86, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 20
            });
            db.Tasks.Add(new Task
            {
                Id = 87, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 20
            });
            db.Tasks.Add(new Task
            {
                Id = 88, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 20
            });
            db.Tasks.Add(new Task
            {
                Id = 89, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 21
            });
            db.Tasks.Add(new Task
            {
                Id = 90, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 21
            });
            db.Tasks.Add(new Task
            {
                Id = 91, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 21
            });
            db.Tasks.Add(new Task
            {
                Id = 92, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 21
            });
            db.Tasks.Add(new Task
            {
                Id = 93, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 21
            });
            db.Tasks.Add(new Task
            {
                Id = 94, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 22
            });
            db.Tasks.Add(new Task
            {
                Id = 95, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 22
            });
            db.Tasks.Add(new Task
            {
                Id = 96, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 22
            });
            db.Tasks.Add(new Task
            {
                Id = 97, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 22
            });
            db.Tasks.Add(new Task
            {
                Id = 98, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 23
            });
            db.Tasks.Add(new Task
            {
                Id = 99, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 23
            });
            db.Tasks.Add(new Task
            {
                Id = 100, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 23
            });
            db.Tasks.Add(new Task
            {
                Id = 101, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 23
            });
            db.Tasks.Add(new Task
            {
                Id = 102, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 23
            });
            db.Tasks.Add(new Task
            {
                Id = 103, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 24
            });
            db.Tasks.Add(new Task
            {
                Id = 104, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 24
            });
            db.Tasks.Add(new Task
            {
                Id = 105, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 24
            });
            db.Tasks.Add(new Task
            {
                Id = 106, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 24
            });
            db.Tasks.Add(new Task
            {
                Id = 107, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 25
            });
            db.Tasks.Add(new Task
            {
                Id = 108, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 25
            });
            db.Tasks.Add(new Task
            {
                Id = 109, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 25
            });
            db.Tasks.Add(new Task
            {
                Id = 110, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 25
            });
            db.Tasks.Add(new Task
            {
                Id = 111, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 25
            });
            db.Tasks.Add(new Task
            {
                Id = 112, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 26
            });
            db.Tasks.Add(new Task
            {
                Id = 113, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 26
            });
            db.Tasks.Add(new Task
            {
                Id = 114, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 26
            });
            db.Tasks.Add(new Task
            {
                Id = 115, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 27
            });
            db.Tasks.Add(new Task
            {
                Id = 116, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 27
            });
            db.Tasks.Add(new Task
            {
                Id = 117, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 27
            });
            db.Tasks.Add(new Task
            {
                Id = 118, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 28
            });
            db.Tasks.Add(new Task
            {
                Id = 119, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 28
            });
            db.Tasks.Add(new Task
            {
                Id = 120, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 28
            });
            db.Tasks.Add(new Task
            {
                Id = 121, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 28
            });
            db.Tasks.Add(new Task
            {
                Id = 122, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 29
            });
            db.Tasks.Add(new Task
            {
                Id = 123, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 29
            });
            db.Tasks.Add(new Task
            {
                Id = 124, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 29
            });
            db.Tasks.Add(new Task
            {
                Id = 125, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 29
            });
            db.Tasks.Add(new Task
            {
                Id = 126, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 29
            });
            db.Tasks.Add(new Task
            {
                Id = 127, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 30
            });
            db.Tasks.Add(new Task
            {
                Id = 128, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 30
            });
            db.Tasks.Add(new Task
            {
                Id = 129, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 30
            });
            db.Tasks.Add(new Task
            {
                Id = 130, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 30
            });
            db.Tasks.Add(new Task
            {
                Id = 131, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 30
            });
            db.Tasks.Add(new Task
            {
                Id = 132, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 31
            });
            db.Tasks.Add(new Task
            {
                Id = 133, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 31
            });
            db.Tasks.Add(new Task
            {
                Id = 134, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 31
            });
            db.Tasks.Add(new Task
            {
                Id = 135, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 31
            });
            db.Tasks.Add(new Task
            {
                Id = 136, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 32
            });
            db.Tasks.Add(new Task
            {
                Id = 137, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 32
            });
            db.Tasks.Add(new Task
            {
                Id = 138, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 32
            });
            db.Tasks.Add(new Task
            {
                Id = 139, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 32
            });
            db.Tasks.Add(new Task
            {
                Id = 140, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 33
            });
            db.Tasks.Add(new Task
            {
                Id = 141, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 33
            });
            db.Tasks.Add(new Task
            {
                Id = 142, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 33
            });
            db.Tasks.Add(new Task
            {
                Id = 143, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 34
            });
            db.Tasks.Add(new Task
            {
                Id = 144, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 34
            });
            db.Tasks.Add(new Task
            {
                Id = 145, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 34
            });
            db.Tasks.Add(new Task
            {
                Id = 146, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 34
            });
            db.Tasks.Add(new Task
            {
                Id = 147, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 35
            });
            db.Tasks.Add(new Task
            {
                Id = 148, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 35
            });
            db.Tasks.Add(new Task
            {
                Id = 149, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 35
            });
            db.Tasks.Add(new Task
            {
                Id = 150, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 35
            });
            db.Tasks.Add(new Task
            {
                Id = 151, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 152, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 153, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 154, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 155, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 156, Name = "Zadanie 6.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 36
            });
            db.Tasks.Add(new Task
            {
                Id = 157, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 37
            });
            db.Tasks.Add(new Task
            {
                Id = 158, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 37
            });
            db.Tasks.Add(new Task
            {
                Id = 159, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 37
            });
            db.Tasks.Add(new Task
            {
                Id = 160, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 37
            });
            db.Tasks.Add(new Task
            {
                Id = 161, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 38
            });
            db.Tasks.Add(new Task
            {
                Id = 162, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 38
            });
            db.Tasks.Add(new Task
            {
                Id = 163, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 38
            });
            db.Tasks.Add(new Task
            {
                Id = 164, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 38
            });
            db.Tasks.Add(new Task
            {
                Id = 165, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 39
            });
            db.Tasks.Add(new Task
            {
                Id = 166, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 39
            });
            db.Tasks.Add(new Task
            {
                Id = 167, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 39
            });
            db.Tasks.Add(new Task
            {
                Id = 168, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 40
            });
            db.Tasks.Add(new Task
            {
                Id = 169, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 40
            });
            db.Tasks.Add(new Task
            {
                Id = 170, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 40
            });
            db.Tasks.Add(new Task
            {
                Id = 171, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 40
            });
            db.Tasks.Add(new Task
            {
                Id = 172, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 41
            });
            db.Tasks.Add(new Task
            {
                Id = 173, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 41
            });
            db.Tasks.Add(new Task
            {
                Id = 174, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 41
            });
            db.Tasks.Add(new Task
            {
                Id = 175, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 41
            });
            db.Tasks.Add(new Task
            {
                Id = 176, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 41
            });
            db.Tasks.Add(new Task
            {
                Id = 177, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 178, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 179, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 180, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 181, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 182, Name = "Zadanie 6.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 42
            });
            db.Tasks.Add(new Task
            {
                Id = 183, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 43
            });
            db.Tasks.Add(new Task
            {
                Id = 184, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 43
            });
            db.Tasks.Add(new Task
            {
                Id = 185, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 43
            });
            db.Tasks.Add(new Task
            {
                Id = 186, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 44
            });
            db.Tasks.Add(new Task
            {
                Id = 187, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 44
            });
            db.Tasks.Add(new Task
            {
                Id = 188, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 44
            });
            db.Tasks.Add(new Task
            {
                Id = 189, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 44
            });
            db.Tasks.Add(new Task
            {
                Id = 190, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 45
            });
            db.Tasks.Add(new Task
            {
                Id = 191, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 45
            });
            db.Tasks.Add(new Task
            {
                Id = 192, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 45
            });
            db.Tasks.Add(new Task
            {
                Id = 193, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 45
            });
            db.Tasks.Add(new Task
            {
                Id = 194, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 45
            });
            db.Tasks.Add(new Task
            {
                Id = 195, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 46
            });
            db.Tasks.Add(new Task
            {
                Id = 196, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 46
            });
            db.Tasks.Add(new Task
            {
                Id = 197, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 46
            });
            db.Tasks.Add(new Task
            {
                Id = 198, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 46
            });
            db.Tasks.Add(new Task
            {
                Id = 199, Name = "Zadanie 5.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 46
            });
            db.Tasks.Add(new Task
            {
                Id = 200, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 47
            });
            db.Tasks.Add(new Task
            {
                Id = 201, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 47
            });
            db.Tasks.Add(new Task
            {
                Id = 202, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 47
            });
            db.Tasks.Add(new Task
            {
                Id = 203, Name = "Zadanie 1.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 48
            });
            db.Tasks.Add(new Task
            {
                Id = 204, Name = "Zadanie 2.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 48
            });
            db.Tasks.Add(new Task
            {
                Id = 205, Name = "Zadanie 3.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 48
            });
            db.Tasks.Add(new Task
            {
                Id = 206, Name = "Zadanie 4.",
                Content = "Długa i skomplikowana treść zadania o $n$ literach, której i tak nikt nigdy nie przeczyta",
                TasksetId = 48
            });

            // Solutions
            db.Solutions.Add(new Solution {Id = 1, CachedContent = "To jest... trywialne                ", TaskId = 1});
            db.Solutions.Add(new Solution {Id = 2, CachedContent = "To jest... trywialne                ", TaskId = 6});
            db.Solutions.Add(new Solution {Id = 3, CachedContent = "To jest... trywialne                ", TaskId = 7});
            db.Solutions.Add(new Solution {Id = 4, CachedContent = "To jest... trywialne                ", TaskId = 8});
            db.Solutions.Add(new Solution {Id = 5, CachedContent = "To jest... trywialne                ", TaskId = 10});
            db.Solutions.Add(new Solution {Id = 6, CachedContent = "To jest... trywialne                ", TaskId = 12});
            db.Solutions.Add(new Solution {Id = 7, CachedContent = "To jest... trywialne                ", TaskId = 14});
            db.Solutions.Add(new Solution {Id = 8, CachedContent = "To jest... trywialne                ", TaskId = 15});
            db.Solutions.Add(new Solution {Id = 9, CachedContent = "To jest... trywialne                ", TaskId = 18});
            db.Solutions.Add(new Solution {Id = 10, CachedContent = "To jest... trywialne                ", TaskId = 23});
            db.Solutions.Add(new Solution {Id = 11, CachedContent = "To jest... trywialne                ", TaskId = 24});
            db.Solutions.Add(new Solution {Id = 12, CachedContent = "To jest... trywialne                ", TaskId = 26});
            db.Solutions.Add(new Solution {Id = 13, CachedContent = "To jest... trywialne                ", TaskId = 35});
            db.Solutions.Add(new Solution {Id = 14, CachedContent = "To jest... trywialne                ", TaskId = 37});
            db.Solutions.Add(new Solution {Id = 15, CachedContent = "To jest... trywialne                ", TaskId = 38});
            db.Solutions.Add(new Solution {Id = 16, CachedContent = "To jest... trywialne                ", TaskId = 40});
            db.Solutions.Add(new Solution {Id = 17, CachedContent = "To jest... trywialne                ", TaskId = 41});
            db.Solutions.Add(new Solution {Id = 18, CachedContent = "To jest... trywialne                ", TaskId = 42});
            db.Solutions.Add(new Solution {Id = 19, CachedContent = "To jest... trywialne                ", TaskId = 44});
            db.Solutions.Add(new Solution {Id = 20, CachedContent = "To jest... trywialne                ", TaskId = 51});
            db.Solutions.Add(new Solution {Id = 21, CachedContent = "To jest... trywialne                ", TaskId = 53});
            db.Solutions.Add(new Solution {Id = 22, CachedContent = "To jest... trywialne                ", TaskId = 54});
            db.Solutions.Add(new Solution {Id = 23, CachedContent = "To jest... trywialne                ", TaskId = 56});
            db.Solutions.Add(new Solution {Id = 24, CachedContent = "To jest... trywialne                ", TaskId = 57});
            db.Solutions.Add(new Solution {Id = 25, CachedContent = "To jest... trywialne                ", TaskId = 58});
            db.Solutions.Add(new Solution {Id = 26, CachedContent = "To jest... trywialne                ", TaskId = 62});
            db.Solutions.Add(new Solution {Id = 27, CachedContent = "To jest... trywialne                ", TaskId = 63});
            db.Solutions.Add(new Solution {Id = 28, CachedContent = "To jest... trywialne                ", TaskId = 65});
            db.Solutions.Add(new Solution {Id = 29, CachedContent = "To jest... trywialne                ", TaskId = 66});
            db.Solutions.Add(new Solution {Id = 30, CachedContent = "To jest... trywialne                ", TaskId = 70});
            db.Solutions.Add(new Solution {Id = 31, CachedContent = "To jest... trywialne                ", TaskId = 71});
            db.Solutions.Add(new Solution {Id = 32, CachedContent = "To jest... trywialne                ", TaskId = 72});
            db.Solutions.Add(new Solution {Id = 33, CachedContent = "To jest... trywialne                ", TaskId = 73});
            db.Solutions.Add(new Solution {Id = 34, CachedContent = "To jest... trywialne                ", TaskId = 74});
            db.Solutions.Add(new Solution {Id = 35, CachedContent = "To jest... trywialne                ", TaskId = 76});
            db.Solutions.Add(new Solution {Id = 36, CachedContent = "To jest... trywialne                ", TaskId = 77});
            db.Solutions.Add(new Solution {Id = 37, CachedContent = "To jest... trywialne                ", TaskId = 80});
            db.Solutions.Add(new Solution {Id = 38, CachedContent = "To jest... trywialne                ", TaskId = 81});
            db.Solutions.Add(new Solution {Id = 39, CachedContent = "To jest... trywialne                ", TaskId = 82});
            db.Solutions.Add(new Solution {Id = 40, CachedContent = "To jest... trywialne                ", TaskId = 85});
            db.Solutions.Add(new Solution {Id = 41, CachedContent = "To jest... trywialne                ", TaskId = 86});
            db.Solutions.Add(new Solution {Id = 42, CachedContent = "To jest... trywialne                ", TaskId = 87});
            db.Solutions.Add(new Solution {Id = 43, CachedContent = "To jest... trywialne                ", TaskId = 88});
            db.Solutions.Add(new Solution {Id = 44, CachedContent = "To jest... trywialne                ", TaskId = 89});
            db.Solutions.Add(new Solution {Id = 45, CachedContent = "To jest... trywialne                ", TaskId = 90});
            db.Solutions.Add(new Solution {Id = 46, CachedContent = "To jest... trywialne                ", TaskId = 91});
            db.Solutions.Add(new Solution {Id = 47, CachedContent = "To jest... trywialne                ", TaskId = 93});
            db.Solutions.Add(new Solution {Id = 48, CachedContent = "To jest... trywialne                ", TaskId = 94});
            db.Solutions.Add(new Solution {Id = 49, CachedContent = "To jest... trywialne                ", TaskId = 95});
            db.Solutions.Add(new Solution {Id = 50, CachedContent = "To jest... trywialne                ", TaskId = 96});
            db.Solutions.Add(new Solution {Id = 51, CachedContent = "To jest... trywialne                ", TaskId = 97});
            db.Solutions.Add(new Solution {Id = 52, CachedContent = "To jest... trywialne                ", TaskId = 98});
            db.Solutions.Add(new Solution {Id = 53, CachedContent = "To jest... trywialne                ", TaskId = 100});
            db.Solutions.Add(new Solution {Id = 54, CachedContent = "To jest... trywialne                ", TaskId = 102});
            db.Solutions.Add(new Solution {Id = 55, CachedContent = "To jest... trywialne                ", TaskId = 104});
            db.Solutions.Add(new Solution {Id = 56, CachedContent = "To jest... trywialne                ", TaskId = 105});
            db.Solutions.Add(new Solution {Id = 57, CachedContent = "To jest... trywialne                ", TaskId = 107});
            db.Solutions.Add(new Solution {Id = 58, CachedContent = "To jest... trywialne                ", TaskId = 110});
            db.Solutions.Add(new Solution {Id = 59, CachedContent = "To jest... trywialne                ", TaskId = 113});
            db.Solutions.Add(new Solution {Id = 60, CachedContent = "To jest... trywialne                ", TaskId = 114});
            db.Solutions.Add(new Solution {Id = 61, CachedContent = "To jest... trywialne                ", TaskId = 115});
            db.Solutions.Add(new Solution {Id = 62, CachedContent = "To jest... trywialne                ", TaskId = 116});
            db.Solutions.Add(new Solution {Id = 63, CachedContent = "To jest... trywialne                ", TaskId = 118});
            db.Solutions.Add(new Solution {Id = 64, CachedContent = "To jest... trywialne                ", TaskId = 121});
            db.Solutions.Add(new Solution {Id = 65, CachedContent = "To jest... trywialne                ", TaskId = 122});
            db.Solutions.Add(new Solution {Id = 66, CachedContent = "To jest... trywialne                ", TaskId = 124});
            db.Solutions.Add(new Solution {Id = 67, CachedContent = "To jest... trywialne                ", TaskId = 125});
            db.Solutions.Add(new Solution {Id = 68, CachedContent = "To jest... trywialne                ", TaskId = 126});
            db.Solutions.Add(new Solution {Id = 69, CachedContent = "To jest... trywialne                ", TaskId = 128});
            db.Solutions.Add(new Solution {Id = 70, CachedContent = "To jest... trywialne                ", TaskId = 129});
            db.Solutions.Add(new Solution {Id = 71, CachedContent = "To jest... trywialne                ", TaskId = 130});
            db.Solutions.Add(new Solution {Id = 72, CachedContent = "To jest... trywialne                ", TaskId = 131});
            db.Solutions.Add(new Solution {Id = 73, CachedContent = "To jest... trywialne                ", TaskId = 134});
            db.Solutions.Add(new Solution {Id = 74, CachedContent = "To jest... trywialne                ", TaskId = 135});
            db.Solutions.Add(new Solution {Id = 75, CachedContent = "To jest... trywialne                ", TaskId = 136});
            db.Solutions.Add(new Solution {Id = 76, CachedContent = "To jest... trywialne                ", TaskId = 139});
            db.Solutions.Add(new Solution {Id = 77, CachedContent = "To jest... trywialne                ", TaskId = 140});
            db.Solutions.Add(new Solution {Id = 78, CachedContent = "To jest... trywialne                ", TaskId = 142});
            db.Solutions.Add(new Solution {Id = 79, CachedContent = "To jest... trywialne                ", TaskId = 144});
            db.Solutions.Add(new Solution {Id = 80, CachedContent = "To jest... trywialne                ", TaskId = 146});
            db.Solutions.Add(new Solution {Id = 81, CachedContent = "To jest... trywialne                ", TaskId = 153});
            db.Solutions.Add(new Solution {Id = 82, CachedContent = "To jest... trywialne                ", TaskId = 154});
            db.Solutions.Add(new Solution {Id = 83, CachedContent = "To jest... trywialne                ", TaskId = 155});
            db.Solutions.Add(new Solution {Id = 84, CachedContent = "To jest... trywialne                ", TaskId = 156});
            db.Solutions.Add(new Solution {Id = 85, CachedContent = "To jest... trywialne                ", TaskId = 157});
            db.Solutions.Add(new Solution {Id = 86, CachedContent = "To jest... trywialne                ", TaskId = 158});
            db.Solutions.Add(new Solution {Id = 87, CachedContent = "To jest... trywialne                ", TaskId = 160});
            db.Solutions.Add(new Solution {Id = 88, CachedContent = "To jest... trywialne                ", TaskId = 162});
            db.Solutions.Add(new Solution {Id = 89, CachedContent = "To jest... trywialne                ", TaskId = 164});
            db.Solutions.Add(new Solution {Id = 90, CachedContent = "To jest... trywialne                ", TaskId = 167});
            db.Solutions.Add(new Solution {Id = 91, CachedContent = "To jest... trywialne                ", TaskId = 170});
            db.Solutions.Add(new Solution {Id = 92, CachedContent = "To jest... trywialne                ", TaskId = 173});
            db.Solutions.Add(new Solution {Id = 93, CachedContent = "To jest... trywialne                ", TaskId = 174});
            db.Solutions.Add(new Solution {Id = 94, CachedContent = "To jest... trywialne                ", TaskId = 176});
            db.Solutions.Add(new Solution {Id = 95, CachedContent = "To jest... trywialne                ", TaskId = 178});
            db.Solutions.Add(new Solution {Id = 96, CachedContent = "To jest... trywialne                ", TaskId = 179});
            db.Solutions.Add(new Solution {Id = 97, CachedContent = "To jest... trywialne                ", TaskId = 182});
            db.Solutions.Add(new Solution {Id = 98, CachedContent = "To jest... trywialne                ", TaskId = 185});
            db.Solutions.Add(new Solution {Id = 99, CachedContent = "To jest... trywialne                ", TaskId = 187});
            db.Solutions.Add(new Solution {Id = 100, CachedContent = "To jest... trywialne                ", TaskId = 188});
            db.Solutions.Add(new Solution {Id = 101, CachedContent = "To jest... trywialne                ", TaskId = 189});
            db.Solutions.Add(new Solution {Id = 102, CachedContent = "To jest... trywialne                ", TaskId = 191});
            db.Solutions.Add(new Solution {Id = 103, CachedContent = "To jest... trywialne                ", TaskId = 193});
            db.Solutions.Add(new Solution {Id = 104, CachedContent = "To jest... trywialne                ", TaskId = 195});
            db.Solutions.Add(new Solution {Id = 105, CachedContent = "To jest... trywialne                ", TaskId = 196});
            db.Solutions.Add(new Solution {Id = 106, CachedContent = "To jest... trywialne                ", TaskId = 198});
            db.Solutions.Add(new Solution {Id = 107, CachedContent = "To jest... trywialne                ", TaskId = 200});
            db.Solutions.Add(new Solution {Id = 108, CachedContent = "To jest... trywialne                ", TaskId = 202});
            db.Solutions.Add(new Solution {Id = 109, CachedContent = "To jest... trywialne                ", TaskId = 203});
            db.Solutions.Add(new Solution {Id = 110, CachedContent = "To jest... trywialne                ", TaskId = 204});
            db.Solutions.Add(new Solution {Id = 111, CachedContent = "To jest... trywialne                ", TaskId = 206});
            db.Solutions.Add(new Solution {Id = 112, CachedContent = "Tu wpisz tresć rowziązania...asdfdfd", TaskId = 210});
            db.Solutions.Add(new Solution {Id = 113, CachedContent = "Tu wpisz tresć rowziązania...d      ", TaskId = 211});
            db.Solutions.Add(new Solution {Id = 114, CachedContent = "Tu wpisz tresć rowziązania...ddd    ", TaskId = 211});
            db.Solutions.Add(new Solution {Id = 115, CachedContent = "Tu wpisz tresć rowziązania...       ", TaskId = 80});
            db.Solutions.Add(new Solution {Id = 116, CachedContent = "Tu wpisz tresć rowziązania...dddddd ", TaskId = 212});
            db.Solutions.Add(new Solution {Id = 117, CachedContent = "Tu wpisz tresć rowziązania...d      ", TaskId = 213});
            db.Solutions.Add(new Solution {Id = 118, CachedContent = "Tu wpisz tresć rowziązania...ddd    ", TaskId = 80});
            db.Solutions.Add(new Solution {Id = 119, CachedContent = "Tu wpisz tresć rowziązania...fffdfd ", TaskId = 80});
            db.Solutions.Add(new Solution {Id = 120, CachedContent = "Tu wpisz tresć rowziązania...       ", TaskId = 80});
            db.Solutions.Add(new Solution {Id = 121, CachedContent = "Tu wpisz tresć rowziązania...fdfd   ", TaskId = 216});
            db.Solutions.Add(new Solution {Id = 122, CachedContent = "Tu wpisz tresć rowziązania...ddd    ", TaskId = 217});
            db.Solutions.Add(new Solution {Id = 123, CachedContent = "Tu wpisz tresć rowziązania...ff     ", TaskId = 218});

            // Comments
            db.Comments.Add(new Comment
            {
                Id = 1, ApplicationUserId = "5c633625-db0e-45f1-b0e8-2f49ae232070", Content = "Fajne",
                CommentDate = new DateTime(2010, 10, 10)
            });
            db.Comments.Add(new Comment
            {
                Id = 1, ApplicationUserId = "5c633625-db0e-45f1-b0e8-2f49ae232070", Content = "Łatwe",
                CommentDate = new DateTime(2010, 10, 11)
            });
            db.Comments.Add(new Comment
            {
                Id = 1, ApplicationUserId = "5c633625-db0e-45f1-b0e8-2f49ae232070", Content = "Trudne",
                CommentDate = new DateTime(2010, 10, 12)
            });

            db.SaveChanges();
        }
    }
}