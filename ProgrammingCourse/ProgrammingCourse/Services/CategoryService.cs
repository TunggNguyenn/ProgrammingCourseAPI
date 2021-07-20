using ProgrammingCourse.Models;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Services
{
    public class CategoryService
    {
        protected readonly CategoryRepository categoryRepository;
        private readonly StudentCourseRepository studentCourseRepository;
        private readonly CourseRepository courseRepository;
        private readonly ViewRepository viewRepository;
        private readonly FeedbackRepository feedbackRepository;
        private readonly UserRepository userRepository;

        public CategoryService(CategoryRepository categoryRepository, StudentCourseRepository studentCourseRepository,
            CourseRepository courseRepository, ViewRepository viewRepository,
            FeedbackRepository feedbackRepository, UserRepository userRepository)
        {
            this.categoryRepository = categoryRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.courseRepository = courseRepository;
            this.viewRepository = viewRepository;
            this.feedbackRepository = feedbackRepository;
            this.userRepository = userRepository;
        }

        public async Task Add(Category category)
        {
            await categoryRepository.Add(category);
        }

        public async Task<Category> GetById(int id)
        {
            return await categoryRepository.GetById(id);
        }

        public async Task<IList<dynamic>> GetCategoryListByCategoryTypeId(int categoryId)
        {
            return await categoryRepository.GetCategoryListByCategoryTypeId(categoryId);
        }

        public async Task<Category> GetWithAllInfoById(int id)
        {
            return await categoryRepository.GetWithAllInfoById(id);
        }

        public async Task<dynamic> GetWithAllInfoByName(string name)
        {
            var bestSellerCourses = await studentCourseRepository.Get10BestSellerCourseIDsInMonth();
            var newestCourses = await courseRepository.Get10NewestCourseIds();
            var mostViewedCourses = await viewRepository.Get10MostViewedCourseIdsInMonth();

            var category = await categoryRepository.GetWithAllInfoByName(name);

            dynamic dynamicCategory = new ExpandoObject();
            dynamicCategory.id = category.Id;
            dynamicCategory.name = category.Name;
            dynamicCategory.label = category.Label;
            dynamicCategory.imageUrl = category.ImageUrl;
            dynamicCategory.categoryTypeId = category.CategoryTypeId;
            dynamicCategory.categoryTypeName = category.CategoryType.Name;


            IList<dynamic> dynamicCourseList = new List<dynamic>();

            for (int c = 0; c < category.Courses.Count; c++)
            {
                dynamic course = new ExpandoObject();
                course.id = category.Courses[c].Id;
                course.price = category.Courses[c].Price;
                course.name = category.Courses[c].Name;
                course.imageUrl = category.Courses[c].ImageUrl;
                course.lastUpdated = category.Courses[c].LastUpdated;
                course.statusId = category.Courses[c].StatusId;
                course.status = category.Courses[c].Status;
                course.discount = category.Courses[c].Discount;
                //course.ShortDiscription = courses[c].ShortDiscription;
                //course.DetailDiscription = courses[c].DetailDiscription;
                course.lecturerId = category.Courses[c].LecturerId;
                course.lecturer = await userRepository.GetById(category.Courses[c].LecturerId);
                course.rating = await feedbackRepository.GetRatingByCourseId(category.Courses[c].Id);
                course.reviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(category.Courses[c].Id);
                course.registeredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(category.Courses[c].Id);

                bool flag = false;
                for (int b = 0; b < bestSellerCourses.Count; b++)
                {
                    if (bestSellerCourses[b].CourseId == category.Courses[c].Id)
                    {
                        course.tag = "BestSeller";
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
                    if (newestCourses[n].CourseId == category.Courses[c].Id)
                    {
                        course.tag = "Newest";
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
                    if (mostViewedCourses[m].CourseId == category.Courses[c].Id)
                    {
                        course.tag = "Trending";
                        flag = true;
                        break;
                    }
                }

                dynamicCourseList.Add(course);
            }

            dynamicCategory.courses = dynamicCourseList;

            return dynamicCategory;
        }

        public async Task<List<Category>> GetAll()
        {
            return await categoryRepository.GetAll();
        }

        public async Task Remove(Category category)
        {
            await categoryRepository.Remove(category);
        }


        public async Task Update(Category category)
        {
            await categoryRepository.Update(category);
        }

        public async Task<dynamic> GetMostRegisteredCategories()
        {
            return await categoryRepository.GetMostRegisteredCategories();
        }
    }
}
