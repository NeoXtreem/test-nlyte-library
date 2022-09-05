namespace Library.Models
{
    public class BookInfo
    {
        public BookInfo(string title)
        {
            Id = title;
            Title = title;
        }

        public BookInfo(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; }

        public string Title { get; }
    }
}