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
            foreach (var issue in _context.Issues.Include(i => i.Book).Include(i => i.Student))
            {
                if (issue.IssueId == id && !issue.IsDeleted)
                    return issue;
            }
            return null;
        }

        public List<Issue> GetAll()
        {
            var issueList = new List<Issue>();
            foreach (var issue in _context.Issues.Include(i => i.Book).Include(i => i.Student))
            {
                if (!issue.IsDeleted)
                    issueList.Add(issue);
            }
            return issueList;
        }

        public List<Issue> GetByStudentId(int studentId)
        {
            var issueList = new List<Issue>();
            foreach (var issue in _context.Issues.Include(i => i.Book).Include(i => i.Student))
            {
                if (issue.StudentId == studentId && !issue.IsDeleted)
                    issueList.Add(issue);
            }
            return issueList;
        }

        public List<Issue> GetByBookId(int bookId)
        {
            var issueList = new List<Issue>();
            foreach (var issue in _context.Issues.Include(i => i.Book).Include(i => i.Student))
            {
                if (issue.BookId == bookId && !issue.IsDeleted)
                    issueList.Add(issue);
            }
            return issueList;
        }

        public void Add(Issue issue)
        {
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void Update(Issue issue)
        {
            var existingIssue = GetById(issue.IssueId);
            if (existingIssue != null)
            {
                existingIssue.BookId = issue.BookId;
                existingIssue.StudentId = issue.StudentId;
                existingIssue.IssueDate = issue.IssueDate;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var issue = GetById(id);
            if (issue != null)
            {
                issue.IsDeleted = true; 
                _context.SaveChanges();
            }
        }
    }
}