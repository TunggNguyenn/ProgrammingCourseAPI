using AutoMapper;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Utilities;
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
            CreateMap<CategoryViewModel, Category>()
                .ForMember(dest => dest.ImageUrl, source => source.MapFrom(source => source.ImageUrl == null ? UploadImageToCloudinary.Upload(source.Image) : source.ImageUrl));

            CreateMap<Category, CategoryViewModel>();

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
