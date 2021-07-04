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

        public CategoryTypeService(CourseService courseService, CategoryTypeRepository categoryTypeRepository, 
            StudentCourseRepository studentCourseRepository, CategoryRepository categoryRepository, 
            FeedbackRepository feedbackRepository)
        {
            this.courseService = courseService;

            this.categoryTypeRepository = categoryTypeRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.categoryRepository = categoryRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public async Task<CategoryType> GetById(int id)
        {
            return await categoryTypeRepository.GetById(id);
        }

        public async Task Add(CategoryType categoryType)
        {
            await categoryTypeRepository.Add(categoryType);
        }


        public async Task<IEnumerable<CategoryType>> GetAll()
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


        public async Task<dynamic> GetFormattedCategoryTypeById(int id)
        {
            var categoryType = await GetById(id);
            var categories = await categoryRepository.GetByCategoryTypeId(id);
            var bestSellerCourses = await courseService.Get10BestSellerCourses();
            var newestCourses = await courseService.Get10NewestCourses();
            var mostViewedCourses = await courseService.Get10MostViewedCourses();

            IList<dynamic> dynamicBestSellerCourses = new List<dynamic>();
            for (int i = 0; i < bestSellerCourses.Count; i++)
            {
                dynamic course = new ExpandoObject();
                course.Id = bestSellerCourses[i].Id;
                course.Tag = "BestSeller";
                course.Price = bestSellerCourses[i].Price;
                course.Name = bestSellerCourses[i].Name;
                course.ImageUrl = bestSellerCourses[i].ImageUrl;
                course.LastUpdated = bestSellerCourses[i].LastUpdated;
                course.StatusId = bestSellerCourses[i].StatusId;
                //course.Status = bestSellerCourses[i].Status;
                course.Discount = bestSellerCourses[i].Discount;
                course.ShortDiscription = bestSellerCourses[i].ShortDiscription;
                course.DetailDiscription = bestSellerCourses[i].DetailDiscription;
                course.CategoryId = bestSellerCourses[i].CategoryId;
                course.LecturerId = bestSellerCourses[i].LecturerId;
                //course.Lecturer = bestSellerCourses[i].Lecturer;
                course.Rating = await feedbackRepository.GetRatingByCourseId(bestSellerCourses[i].Id);
                course.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(bestSellerCourses[i].Id);
                course.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(bestSellerCourses[i].Id);

                dynamicBestSellerCourses.Add(course);
            }

            IList<dynamic> dynamicCategories = new List<dynamic>();

            for (int i = 0; i < categories.Count; i++)
            {
                dynamic dynamicCategory = new ExpandoObject();
                dynamicCategory.Id = categories[i].Id;
                dynamicCategory.Name = categories[i].Name;
                dynamicCategory.Label = categories[i].Label;
                dynamicCategory.ImageUrl = categories[i].ImageUrl;

                var courses = await courseService.GetByCategoryId(categories[i].Id);

                IList<dynamic> dynamicCourses = new List<dynamic>();

                for(int c = 0; c < courses.Count; c++)
                {
                    dynamic course = new ExpandoObject();
                    course.Id = courses[c].Id;
                    course.Price = courses[c].Price;
                    course.Name = courses[c].Name;
                    course.ImageUrl = courses[c].ImageUrl;
                    course.LastUpdated = courses[c].LastUpdated;
                    course.StatusId = courses[c].StatusId;
                    //course.Status = courses[c].Status;
                    course.Discount = courses[c].Discount;
                    course.ShortDiscription = courses[c].ShortDiscription;
                    course.DetailDiscription = courses[c].DetailDiscription;
                    course.CategoryId = courses[c].CategoryId;
                    course.LecturerId = courses[c].LecturerId;
                    //course.Lecturer = courses[c].Lecturer;
                    course.Rating = await feedbackRepository.GetRatingByCourseId(courses[c].Id);
                    course.ReviewerNumber = await feedbackRepository.GetReviewerNumberByCourseId(courses[c].Id);
                    course.RegisteredNumber = await studentCourseRepository.GetRegisteredNumberByCourseId(courses[c].Id);

                    bool flag = false;
                    for (int b = 0; b < bestSellerCourses.Count; b++)
                    {
                        if(bestSellerCourses[b].Id == courses[c].Id)
                        {
                            course.Tag = "BestSeller";
                            flag = true;
                            break;
                        }
                    }

                    if(flag == true)
                    {
                        dynamicCourses.Add(course);
                        continue;
                    }

                    for (int n = 0; n < newestCourses.Count; n++)
                    {
                        if (newestCourses[n].Id == courses[c].Id)
                        {
                            course.Tag = "Newest";
                            flag = true;
                            break;
                        }
                    }

                    if (flag == true)
                    {
                        dynamicCourses.Add(course);
                        continue;
                    }

                    for (int m = 0; m < mostViewedCourses.Count; m++)
                    {
                        if (mostViewedCourses[m].Id == courses[c].Id)
                        {
                            course.Tag = "Trending";
                            flag = true;
                            break;
                        }
                    }

                    dynamicCourses.Add(course);
                }

                dynamicCategory.Courses = dynamicCourses;

                dynamicCategories.Add(dynamicCategory);
            }

            return new 
            { 
                Id = categoryType.Id,
                Name = categoryType.Name,
                Categories = dynamicCategories,
                BestSeller = dynamicBestSellerCourses
            };
        }
    }
}
