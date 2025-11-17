using RestAPI.Models;

namespace RestAPI.Repositories
{
    public interface IStudentRepository
    {
        Student? GetById(int id);
        Student? GetByName(string name);
        List<Student> GetAll();
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
    }
}