namespace RestAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; } //primary key
        public string AuthorName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } //soft delete flag 

        //navigation property
        public ICollection<Book> Books { get; set; } = new List<Book>();
        //means one author can have many books one to many relation
        


    }
}