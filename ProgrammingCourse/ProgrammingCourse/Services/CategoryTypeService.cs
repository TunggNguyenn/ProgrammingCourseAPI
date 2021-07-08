using ProgrammingCourse.Models;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Services
{
    public class CategoryTypeService
    {
        private readonly CourseService courseService;
        private readonly CategoryTypeRepository categoryTypeRepository;
        private readonly StudentCourseRepository studentCourseRepository;
        private readonly CategoryRepository categoryRepository;
        private readonly FeedbackRepository feedbackRepository;
        private readonly CourseRepository courseRepository;
        private readonly ViewRepository viewRepository;
        private readonly UserRepository userRepository;

        public CategoryTypeService(CourseService courseService, CategoryTypeRepository categoryTypeRepository,
            StudentCourseRepository studentCourseRepository, CategoryRepository categoryRepository,
            FeedbackRepository feedbackRepository, CourseRepository courseRepository,
            ViewRepository viewRepository, UserRepository userRepository)
        {
            this.courseService = courseService;

            this.categoryTypeRepository = categoryTypeRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.categoryRepository = categoryRepository;
            this.feedbackRepository = feedbackRepository;
            this.courseRepository = courseRepository;
            this.viewRepository = viewRepository;
            this.userRepository = userRepository;
        }

        public async Task<CategoryType> GetById(int id)
        {
            return await categoryTypeRepository.GetById(id);
        }

        public async Task<CategoryType> GetWithAllInfoById(int id)
        {
            return await categoryTypeRepository.GetWithAllInfoById(id);
        }

        public async Task Add(CategoryType categoryType)
        {
            await categoryTypeRepository.Add(categoryType);
        }


        public async Task<List<CategoryType>> GetAll()
        {
            return await categoryTypeRepository.GetAll();
        }

        public async Task Remove(CategoryType categoryType)
        {
            await categoryTypeRepository.Remove(categoryType);
        }

        public async Task Update(CategoryType categoryType)
        {
            await categoryTypeRepository.Update(categoryType);
        }


        public async Task<object> GetFormattedCategoryTypeById(int id)
        {
            var categoryType = await categoryTypeRepository.GetWithAllInfoById(id);

            var bestSellerCourses = await studentCourseRepository.Get10BestSellerCourseIDsInMonth();
            var newestCourses = await courseRepository.Get10NewestCourseIds();
            var mostViewedCourses = await viewRepository.Get10MostViewedCourseIdsInMonth();


            IList<dynamic> dynamicCategoryList = new List<dynamic>();

            for (int i = 0; i < categoryType.Categories.Count; i++)
            {
                var courses = await courseRepository.GetByCategoryId(categoryType.Categories[i].Id);

                categoryType.Categories[i].Courses = null;  //?

                IList<dynamic> dynamicCourseList = new List<dynamic>();

                for (int c = 0; c < courses.Count; c++)
                {
                    dynamic course = new ExpandoObject();
                    course.Id = courses[c].Id;
                    course.Price = courses[c].Price;
                    course.Name = courses[c].Name;
                    course.ImageUrl = courses[c].ImageUrl;
                    course.LastUpdated = courses[c].LastUpdated;
                    course.StatusId = courses[c].StatusId;
                    course.Status = courses[c].Status;
                    course.Discount = courses[c].Discount;
                    //course.ShortDiscription = courses[c].ShortDiscription;
                    //course.DetailDiscription = courses[c].DetailDiscription;
                    course.LecturerId = courses[c].LecturerId;
                    course.Lecturer = await userRepository.GetById(courses[c].LecturerId);
                    course.Rating = await feedbackRepository.GetRatingByCourseId(courses[c].Id);
                    course.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(courses[c].Id);
                    course.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(courses[c].Id);

                    bool flag = false;
                    for (int b = 0; b < bestSellerCourses.Count; b++)
                    {
                        if (bestSellerCourses[b].CourseId == courses[c].Id)
                        {
                            course.Tag = "BestSeller";
                            flag = true;
                            break;
                        }
                    }

                    if (flag == true)
                    {
                        dynamicCourseList.Add(course);
                        continue;
                    }

                    for (int n = 0; n < newestCourses.Count; n++)
                    {
                        if (newestCourses[n].CourseId == courses[c].Id)
                        {
                            course.Tag = "Newest";
                            flag = true;
                            break;
                        }
                    }

                    if (flag == true)
                    {
                        dynamicCourseList.Add(course);
                        continue;
                    }

                    for (int m = 0; m < mostViewedCourses.Count; m++)
                    {
                        if (mostViewedCourses[m].CourseId == courses[c].Id)
                        {
                            course.Tag = "Trending";
                            flag = true;
                            break;
                        }
                    }

                    dynamicCourseList.Add(course);
                }


                dynamicCategoryList.Add(dynamicCourseList);
            }

            return new
            {
                categoryType = categoryType,
                dynamicCategoryList = dynamicCategoryList
            };
        }
    }
}
