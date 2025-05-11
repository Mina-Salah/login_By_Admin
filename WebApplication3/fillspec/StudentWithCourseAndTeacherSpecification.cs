using WebApplication3.GenaricISpecification;
using WebApplication3.Models;

namespace WebApplication3.fillspec
{
    public class StudentWithCourseAndTeacherSpecification : BaseSpecification<Student>
    {
        public StudentWithCourseAndTeacherSpecification()
            : base(s => !s.IsDeleted) // فقط الطلاب غير المحذوفين
        {
            AddInclude(s => s.Course);
            AddInclude(s => s.Teacher);
        }

        public StudentWithCourseAndTeacherSpecification(int id)
            : base(s => s.Id == id)
        {
            AddInclude(s => s.Course);
            AddInclude(s => s.Teacher);
        }
    }
}
