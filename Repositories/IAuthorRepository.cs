using RestAPI.Models;

namespace RestAPI.Repositories
{
    public interface IAuthorRepository
    {
        Author? GetById(int id);
        Author? GetByName(string authorName);
        List<Author> GetAll();
        void Add(Author author);
        void Update(Author author);
        void Delete(int id);
    }
}