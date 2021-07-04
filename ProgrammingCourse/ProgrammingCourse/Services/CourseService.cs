using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Services
{
    public class CourseService
    {
        private readonly CourseRepository courseRepository;
        private readonly StudentCourseRepository studentCourseRepository;
        private readonly ViewRepository viewRepository;

        public CourseService(CourseRepository courseRepository, StudentCourseRepository studentCourseRepository, ViewRepository viewRepository)
        {
            this.courseRepository = courseRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.viewRepository = viewRepository;
        }


        public async Task<Course> GetById(int id)
        {
            return await courseRepository.GetById(id);
        }


        public async Task Add(Course course)
        {
            await courseRepository.Add(course);
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await courseRepository.GetAll();
        }

        public async Task<IEnumerable<Course>> GetAllInfo()
        {
            return await courseRepository.GetAllInfo();
        }

        public async Task Remove(Course course)
        {
            await courseRepository.Remove(course);
        }

        public async Task Update(Course course)
        {
            await courseRepository.Update(course);
        }


        public async Task<IList<Course>> Get10BestSellerCourses()
        {
            var courses = await courseRepository.GetAll();
            var bestSellerCourses = courses
                .OrderByDescending(c => studentCourseRepository.GetRegisteredStudentCoursesInMonthByCourseId(c.Id).Result.Count)
                .Take(10)
                .ToList();
            return bestSellerCourses;
        }

        public async Task<IList<Course>> Get10NewestCourses()
        {
            var courses = await courseRepository.GetAll();
            var newestCourses = courses.OrderByDescending(c => c.Id).Take(10).ToList();
            return newestCourses;
        }

        public async Task<IList<Course>> Get10MostViewedCourses()
        {
            var courses = await courseRepository.GetAll();
            var mostViewedCourses = courses
                .OrderByDescending(c => viewRepository.GetViewNumberInMonthByCourseId(c.Id).Result)
                .Take(10)
                .ToList();
            return mostViewedCourses;
        }


        public async Task<IList<Course>> GetByCategoryId(int categoryId)
        {
            return await courseRepository.GetByCategoryId(categoryId);
        }

        public async Task<IList<Course>> Get10CoursesByCategoryId(int categoryId)
        {
            var courses = await courseRepository.GetByCategoryId(categoryId);
            return courses.Take(10).ToList();
        }
    }
}
