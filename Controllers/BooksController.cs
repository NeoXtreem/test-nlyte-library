using System.Configuration;
using System.Web.Http;
using Library.Repositories;
using Library.Services;

namespace Library.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private static readonly LibraryService LibraryService;
        private static readonly int MaxTopWords = int.Parse(ConfigurationManager.AppSettings["MaxTopWords"]);
        private static readonly int MinTopWordsLength = int.Parse(ConfigurationManager.AppSettings["MinTopWordsLength"]);

        // Simulate dependency injection using a static constructor.
        static BooksController() => LibraryService = new LibraryService(new FileBookRepository());

        /// <summary>
        /// Gets the top words of the specified book, or the list of books if no book ID is specified.
        /// </summary>
        /// <param name="id">The book ID</param>
        /// <param name="startsWith">The prefix that all top words should start with</param>
        /// <returns>The top words of the specified book, or the list of books if no book ID is specified.</returns>
        public IHttpActionResult Get(string id = null, [FromUri] string startsWith = null)
        {
            if (string.IsNullOrEmpty(id)) return Json(LibraryService.GetCatalogue());
            return Json(LibraryService.GetTopWords(id, MaxTopWords, MinTopWordsLength, startsWith));
        }
    }
}