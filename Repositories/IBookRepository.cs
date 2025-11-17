using RestAPI.Models;

namespace RestAPI.Repositories
{
    public interface IBookRepository
    {
        Book? GetById(int id);
        Book? GetByTitle(string title);
        List<Book> GetAll();
        List<Book> GetByAuthorId(int authorId);
        void Add(Book book);
        void Update(Book book);
        void Delete(int id);
    }
}