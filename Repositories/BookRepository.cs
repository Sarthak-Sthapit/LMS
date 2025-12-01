
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.BookId == id && !b.IsDeleted);
        }

        public Book? GetByTitle(string title)
        {
            return _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Title == title && !b.IsDeleted);
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Author)
                .Where(b => !b.IsDeleted)
                .ToList();
        }

        public List<Book> GetByAuthorId(int authorId)
        {
            return _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId && !b.IsDeleted)
                .ToList();
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            
            
         
            var existingBook = _context.Books
                .AsNoTracking()
                .FirstOrDefault(b => b.BookId == book.BookId && !b.IsDeleted);
            
            if (existingBook != null)
            {
                
                _context.Books.Attach(book);

                _context.Entry(book).Property(b => b.Title).IsModified = true;
                _context.Entry(book).Property(b => b.AuthorId).IsModified = true;
                _context.Entry(book).Property(b => b.Publisher).IsModified = true;
                _context.Entry(book).Property(b => b.Barcode).IsModified = true;
                _context.Entry(book).Property(b => b.ISBN).IsModified = true;
                _context.Entry(book).Property(b => b.SubjectGenre).IsModified = true;
                _context.Entry(book).Property(b => b.PublicationDate).IsModified = true;
                
            
                
                _context.SaveChanges();
                Console.WriteLine($" SaveChanges completed ");
            }
            else
            {
                Console.WriteLine($" ERROR: Book not found! ");
            }
        }

        public void Delete(int id)
        {
            var book = _context.Books
                .FirstOrDefault(b => b.BookId == id && !b.IsDeleted);
            
            if (book != null)
            {
                book.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}