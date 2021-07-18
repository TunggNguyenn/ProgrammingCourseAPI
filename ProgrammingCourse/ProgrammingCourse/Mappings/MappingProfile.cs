using AutoMapper;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<CategoryTypeViewModel, CategoryType>().ReverseMap();
            CreateMap<CourseViewModel, Course>().ReverseMap();
            CreateMap<FeedbackViewModel, Feedback>().ReverseMap();
            CreateMap<LectureViewModel, Lecture>().ReverseMap();
            CreateMap<StatusViewModel, Status>().ReverseMap();
            CreateMap<StudentCourseViewModel, StudentCourse>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<ViewViewModel, View>().ReverseMap();
            CreateMap<WatchListViewModel, WatchList>().ReverseMap();
            CreateMap<CartViewModel, Cart>().ReverseMap();
        }
    }
}
