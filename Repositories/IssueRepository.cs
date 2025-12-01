using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }

        public Issue? GetById(int id)
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .FirstOrDefault(i => i.IssueId == id && !i.IsDeleted);
        }

        public List<Issue> GetAll()
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .Where(i => !i.IsDeleted)
                .ToList();
        }

        public List<Issue> GetByStudentId(int studentId)
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .Where(i => i.StudentId == studentId && !i.IsDeleted)
                .ToList();
        }

        public List<Issue> GetByBookId(int bookId)
        {
            return _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Student)
                .Where(i => i.BookId == bookId && !i.IsDeleted)
                .ToList();
        }

        public void Add(Issue issue)
        {
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void Update(Issue issue)
        {
            var existingIssue = _context.Issues
                .FirstOrDefault(i => i.IssueId == issue.IssueId && !i.IsDeleted);
            
            if (existingIssue != null)
            {
                existingIssue.BookId = issue.BookId;
                existingIssue.StudentId = issue.StudentId;
                existingIssue.IssueDate = issue.IssueDate;
                existingIssue.DueDate = issue.DueDate;
                existingIssue.ReturnDate = issue.ReturnDate;
                existingIssue.IsReturned = issue.IsReturned;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var issue = _context.Issues
                .FirstOrDefault(i => i.IssueId == id && !i.IsDeleted);
            
            if (issue != null)
            {
                issue.IsDeleted = true; 
                _context.SaveChanges();
            }
        }
    }
}