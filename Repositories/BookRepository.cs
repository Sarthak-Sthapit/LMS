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
            foreach (var book in _context.Books.Include(b => b.Author))
            {
                if (book.BookId == id && !book.IsDeleted)
                    return book;
            }
            return null;
        }

        public Book? GetByTitle(string title)
        {
            foreach (var book in _context.Books.Include(b => b.Author))
            {
                if (book.Title == title && !book.IsDeleted)
                    return book;
            }
            return null;
        }

        public List<Book> GetAll()
        {
            var bookList = new List<Book>();
            foreach (var book in _context.Books.Include(b => b.Author))
            {
                if (!book.IsDeleted)
                    bookList.Add(book);
            }
            return bookList;
        }

        public List<Book> GetByAuthorId(int authorId)
        {
            var bookList = new List<Book>();
            foreach (var book in _context.Books.Include(b => b.Author))
            {
                if (book.AuthorId == authorId && !book.IsDeleted)
                    bookList.Add(book);
            }
            return bookList;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book book)
        {
            var existingBook = GetById(book.BookId);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.AuthorId = book.AuthorId;
                existingBook.Publisher = book.Publisher;
                existingBook.Barcode = book.Barcode;
                existingBook.ISBN = book.ISBN;
                existingBook.SubjectGenre = book.SubjectGenre;
                existingBook.PublicationDate = book.PublicationDate;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                book.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}