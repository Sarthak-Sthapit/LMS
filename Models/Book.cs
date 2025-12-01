// Models/Book.cs - ENHANCED VERSION
namespace RestAPI.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string SubjectGenre { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        
        // ✅ NEW: Availability tracking
        public int TotalCopies { get; set; } = 1; // Total copies available in library
        public int AvailableCopies { get; set; } = 1; // Copies currently available
        
        public bool IsDeleted { get; set; } = false;
        
        // ✅ Computed property
        public bool IsAvailable => AvailableCopies > 0;
        
        // Navigation properties
        public Author Author { get; set; } = null!;
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}