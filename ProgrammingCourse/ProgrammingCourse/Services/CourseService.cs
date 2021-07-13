using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Services
{
    public class CourseService
    {
        private readonly CourseRepository courseRepository;
        private readonly FeedbackRepository feedbackRepository;
        private readonly CategoryRepository categoryRepository;
        private readonly UserRepository userRepository;
        private readonly ViewRepository viewRepository;
        private readonly StudentCourseRepository studentCourseRepository;
        private readonly LectureRepository lectureRepository;

        public CourseService(CourseRepository courseRepository, FeedbackRepository feedbackRepository, 
            CategoryRepository categoryRepository, UserRepository userRepository,
             ViewRepository viewRepository, StudentCourseRepository studentCourseRepository,
             LectureRepository lectureRepository)
        {
            this.courseRepository = courseRepository;
            this.feedbackRepository = feedbackRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.viewRepository = viewRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.lectureRepository = lectureRepository;
        }


        public async Task<Course> GetById(int id)
        {
            return await courseRepository.GetById(id);
        }

        public async Task<List<int>> GetCourseIdList()
        {
            return await courseRepository.GetCourseIdList();
        }

        public async Task<dynamic> GeWithAllInfoById(int id)
        {
            var course = await courseRepository.GetById(id);

            if (course == null)
            {
                return null;
            }


            dynamic dynamicCourse = new ExpandoObject();
            dynamicCourse.id = course.Id;
            dynamicCourse.price = course.Price;
            dynamicCourse.name = course.Name;
            dynamicCourse.imageUrl = course.ImageUrl;
            dynamicCourse.lastUpdated = course.LastUpdated;
            dynamicCourse.statusId = course.StatusId;
            //dynamicCourse.status = course.Status.Name;
            dynamicCourse.discount = course.Discount;
            dynamicCourse.shortDiscription = course.ShortDiscription;
            dynamicCourse.detailDiscription = course.DetailDiscription;
            dynamicCourse.lecturerId = course.LecturerId;
            dynamicCourse.lecturer = await userRepository.GetById(course.LecturerId);
            dynamicCourse.rating = await feedbackRepository.GetRatingByCourseId(course.Id);
            dynamicCourse.reviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
            dynamicCourse.registeredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);
            dynamicCourse.viewNumber = await viewRepository.GetViewNumberByCourseId(course.Id);
            dynamicCourse.lectures = await lectureRepository.GetLectureListByCourseId(course.Id);

            return dynamicCourse;
        }


        public async Task Add(Course course)
        {
            await courseRepository.Add(course);
        }

        public async Task AddRange(List<Course> courses)
        {
            await courseRepository.AddRange(courses);
        }

        public async Task<List<Course>> GetAll()
        {
            return await courseRepository.GetAll();
        }


        public async Task Remove(Course course)
        {
            await courseRepository.Remove(course);
        }

        public async Task Update(Course course)
        {
            await courseRepository.Update(course);
        }

        public async Task<IList<dynamic>> Get10BestSellerCoursesInMonthByCategoryTypeId(int categoryTypeId)
        {
            var courseIds = await studentCourseRepository.Get10BestSellerCoursesInMonthByCategoryTypeId(categoryTypeId);

            IList<dynamic> bestSellerCourses = new List<dynamic>();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var course = await courseRepository.GetById(courseIds[i].CourseId);

                dynamic dynamicCourse = new ExpandoObject();
                dynamicCourse.Id = course.Id;
                dynamicCourse.Price = course.Price;
                dynamicCourse.Name = course.Name;
                dynamicCourse.ImageUrl = course.ImageUrl;
                dynamicCourse.LastUpdated = course.LastUpdated;
                dynamicCourse.StatusId = course.StatusId;
                dynamicCourse.Status = course.Status;
                dynamicCourse.Discount = course.Discount;
                dynamicCourse.LecturerId = course.LecturerId;
                dynamicCourse.Lecturer = await userRepository.GetById(course.LecturerId);
                dynamicCourse.Rating = await feedbackRepository.GetRatingByCourseId(course.Id);
                dynamicCourse.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
                dynamicCourse.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);

                bestSellerCourses.Add(dynamicCourse);
            }

            return bestSellerCourses;
        }


        public async Task<List<Course>> GetByCategoryId(int categoryId)
        {
            return await courseRepository.GetByCategoryId(categoryId);
        }

        public async Task<dynamic> GetOutstandingCourses()
        {
            var courseIds = await courseRepository.GetOutStandingCourseIds();

            IList<dynamic> outStandingCourses = new List<dynamic>();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var course = await courseRepository.GetById(courseIds[i].CourseId);

                dynamic dynamicCourse = new ExpandoObject();
                dynamicCourse.Id = course.Id;
                dynamicCourse.Price = course.Price;
                dynamicCourse.Name = course.Name;
                dynamicCourse.ImageUrl = course.ImageUrl;
                dynamicCourse.LastUpdated = course.LastUpdated;
                dynamicCourse.StatusId = course.StatusId;
                dynamicCourse.Status = course.Status;
                dynamicCourse.Discount = course.Discount;
                dynamicCourse.LecturerId = course.LecturerId;
                dynamicCourse.Lecturer = await userRepository.GetById(course.LecturerId);
                dynamicCourse.Rating = await feedbackRepository.GetRatingByCourseId(course.Id);
                dynamicCourse.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
                dynamicCourse.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);

                outStandingCourses.Add(dynamicCourse);
            }

            return outStandingCourses;
        }

        public async Task<dynamic> GetMostViewedCourses()
        {
            var courseIds = await viewRepository.Get10MostViewedCourseIdsInMonth();

            IList<dynamic> mostViewedCourses = new List<dynamic>();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var course = await courseRepository.GetById(courseIds[i].CourseId);

                dynamic dynamicCourse = new ExpandoObject();
                dynamicCourse.Id = course.Id;
                dynamicCourse.Price = course.Price;
                dynamicCourse.Name = course.Name;
                dynamicCourse.ImageUrl = course.ImageUrl;
                dynamicCourse.LastUpdated = course.LastUpdated;
                dynamicCourse.StatusId = course.StatusId;
                dynamicCourse.Status = course.Status;
                dynamicCourse.Discount = course.Discount;
                dynamicCourse.LecturerId = course.LecturerId;
                dynamicCourse.Lecturer = await userRepository.GetById(course.LecturerId);
                dynamicCourse.Rating = await feedbackRepository.GetRatingByCourseId(course.Id);
                dynamicCourse.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
                dynamicCourse.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);

                mostViewedCourses.Add(dynamicCourse);
            }

            return mostViewedCourses;
        }


        public async Task<dynamic> GetNewestCourses()
        {
            var courseIds = await courseRepository.Get10NewestCourseIds();

            IList<dynamic> newestCourses = new List<dynamic>();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var course = await courseRepository.GetById(courseIds[i].CourseId);

                dynamic dynamicCourse = new ExpandoObject();
                dynamicCourse.Id = course.Id;
                dynamicCourse.Price = course.Price;
                dynamicCourse.Name = course.Name;
                dynamicCourse.ImageUrl = course.ImageUrl;
                dynamicCourse.LastUpdated = course.LastUpdated;
                dynamicCourse.StatusId = course.StatusId;
                dynamicCourse.Status = course.Status;
                dynamicCourse.Discount = course.Discount;
                dynamicCourse.LecturerId = course.LecturerId;
                dynamicCourse.Lecturer = await userRepository.GetById(course.LecturerId);
                dynamicCourse.Rating = await feedbackRepository.GetRatingByCourseId(course.Id);
                dynamicCourse.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
                dynamicCourse.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);

                newestCourses.Add(dynamicCourse);
            }

            return newestCourses;
        }


        public async Task<dynamic> GetBestSellerCoursesByCategoryId(int courseId, int categoryId)
        {
            var courseIds = await courseRepository.GetBestSellerCoursesByCategoryId(courseId, categoryId);

            IList<dynamic> bestSellerCourses = new List<dynamic>();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var course = await courseRepository.GetById(courseIds[i].CourseId);

                dynamic dynamicCourse = new ExpandoObject();
                dynamicCourse.Id = course.Id;
                dynamicCourse.Price = course.Price;
                dynamicCourse.Name = course.Name;
                dynamicCourse.ImageUrl = course.ImageUrl;
                dynamicCourse.LastUpdated = course.LastUpdated;
                dynamicCourse.StatusId = course.StatusId;
                dynamicCourse.Status = course.Status;
                dynamicCourse.Discount = course.Discount;
                dynamicCourse.LecturerId = course.LecturerId;
                dynamicCourse.Lecturer = await userRepository.GetById(course.LecturerId);
                dynamicCourse.Rating = await feedbackRepository.GetRatingByCourseId(course.Id);
                dynamicCourse.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(course.Id);
                dynamicCourse.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(course.Id);

                bestSellerCourses.Add(dynamicCourse);
            }

            return bestSellerCourses;
        }

        public async Task<IList<dynamic>> GetCourseListByLecturerId(string lecturerId)
        {
            return await courseRepository.GetCourseListByLecturerId(lecturerId);
        }


        //public async Task<IList<Course>> Get10CoursesByCategoryId(int categoryId)
        //{
        //    var courses = await courseRepository.GetByCategoryId(categoryId);
        //    return courses.Take(10).ToList();
        //}
    }
}
