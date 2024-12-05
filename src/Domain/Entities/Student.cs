
namespace Domain.Entities
{
    public class Student : User
    {
        public int StudentNumber { get; set; }
        public ICollection<Course> EnrolledCourses {  get; set; }
    }
}
