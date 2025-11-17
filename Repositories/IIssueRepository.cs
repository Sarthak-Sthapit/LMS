using RestAPI.Models;

namespace RestAPI.Repositories
{
    public interface IIssueRepository
    {
        Issue? GetById(int id);
        List<Issue> GetAll();
        List<Issue> GetByStudentId(int studentId);
        List<Issue> GetByBookId(int bookId);
        void Add(Issue issue);
        void Update(Issue issue);
        void Delete(int id);
    }
}