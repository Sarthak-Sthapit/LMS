using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Student? GetById(int id)
        {
            foreach (var student in _context.Students)
            {
                if (student.StudentId == id && !student.IsDeleted)
                    return student;
            }
            return null;
        }

        public Student? GetByName(string name)
        {
            foreach (var student in _context.Students)
            {
                if (student.Name == name && !student.IsDeleted)
                    return student;
            }
            return null;
        }

        public List<Student> GetAll()
        {
            var studentList = new List<Student>();
            foreach (var student in _context.Students)
            {
                if (!student.IsDeleted)
                    studentList.Add(student);
            }
            return studentList;
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Update(Student student)
        {
            var existingStudent = GetById(student.StudentId);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Address = student.Address;
                existingStudent.ContactNo = student.ContactNo;
                existingStudent.Faculty = student.Faculty;
                existingStudent.Semester = student.Semester;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var student = GetById(id);
            if (student != null)
            {
                student.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}