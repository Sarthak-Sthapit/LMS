
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;
        
        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public Author? GetById(int id)
        {
            return _context.Authors
                .FirstOrDefault(a => a.AuthorId == id && !a.IsDeleted);
        }

        public Author? GetByName(string authorName)
        {
            return _context.Authors
                .FirstOrDefault(a => a.AuthorName == authorName && !a.IsDeleted);
        }

        public List<Author> GetAll()
        {
            return _context.Authors
                .Where(a => !a.IsDeleted)
                .ToList();
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void Update(Author author)
        {
            var existingAuthor = _context.Authors
                .FirstOrDefault(a => a.AuthorId == author.AuthorId && !a.IsDeleted);
            
            if (existingAuthor != null)
            {
                existingAuthor.AuthorName = author.AuthorName;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var author = _context.Authors
                .FirstOrDefault(a => a.AuthorId == id && !a.IsDeleted);
            
            if (author != null)
            {
                author.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}