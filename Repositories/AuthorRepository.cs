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
            foreach (var author in _context.Authors)
            {
                if (author.AuthorId == id && !author.IsDeleted)
                    return author;
            }
            return null;
        }

        public Author? GetByName(string authorName)
        {
            foreach (var author in _context.Authors)
            {
                if (author.AuthorName == authorName && !author.IsDeleted)
                    return author;
            }
            return null;
        }

        public List<Author> GetAll()
        {
            var authorList = new List<Author>();
            foreach (var author in _context.Authors)
            {
                if (!author.IsDeleted)
                    authorList.Add(author);
            }
            return authorList;
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void Update(Author author)
        {
            var existingAuthor = GetById(author.AuthorId);
            if (existingAuthor != null)
            {
                existingAuthor.AuthorName = author.AuthorName;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var author = GetById(id);
            if (author != null)
            {
                author.IsDeleted = true;
                _context.SaveChanges();
            }
        }



    }
}