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
            return _context.Students
                .FirstOrDefault(s => s.StudentId == id && !s.IsDeleted);
        }

        public Student? GetByName(string name)
        {
            return _context.Students
                .FirstOrDefault(s => s.Name == name && !s.IsDeleted);
        }

        public List<Student> GetAll()
        {
            return _context.Students
                .Where(s => !s.IsDeleted)
                .ToList();
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Update(Student student)
        {
            var existingStudent = _context.Students
                .FirstOrDefault(s => s.StudentId == student.StudentId && !s.IsDeleted);
            
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
            var student = _context.Students
                .FirstOrDefault(s => s.StudentId == id && !s.IsDeleted);
            
            if (student != null)
            {
                student.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}