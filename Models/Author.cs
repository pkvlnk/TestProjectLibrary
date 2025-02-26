namespace TestProjectLibrary.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
