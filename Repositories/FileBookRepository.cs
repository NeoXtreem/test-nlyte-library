using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Repositories
{
    internal class FileBookRepository : IBookRepository
    {
        private static readonly string BookFileExtension = $".{ConfigurationManager.AppSettings["BookFileExtension"]}";
        private readonly string _booksPath = HostingEnvironment.MapPath($"~/{ConfigurationManager.AppSettings["BooksPath"]}");
        private readonly Dictionary<string, string> _bookCache = new();

        public IEnumerable<BookInfo> GetCatalogue()
        {
            return Directory.GetFiles(_booksPath, $"*{BookFileExtension}")
                .Select(Path.GetFileNameWithoutExtension)
                .Select(f => new BookInfo(f));
        }

        public string GetText(string id)
        {
            // Load the book text from file (from the cache if possible).
            if (!_bookCache.TryGetValue(id, out var text))
            {
                text = File.ReadAllText(Path.Combine(_booksPath, id + BookFileExtension));
                _bookCache.Add(id, text);
            }

            return text;
        }
    }
}