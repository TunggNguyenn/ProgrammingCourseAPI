using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCourse.Models
{
    public class ProgrammingCourseDbContext : IdentityDbContext<User>
    {
        public ProgrammingCourseDbContext(DbContextOptions<ProgrammingCourseDbContext> options) : base(options)
        {

        }


        public DbSet<Status> Statuses { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<View> Views { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CourseCart> CourseCarts { get; set; }
        public DbSet<CourseProcess> CourseProcesses { get; set; }
    }
}
