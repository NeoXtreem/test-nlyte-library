using Library.Models;
using System.Collections.Generic;

namespace Library.Repositories.Interfaces
{
    internal interface IBookRepository
    {
        IEnumerable<BookInfo> GetCatalogue();

        string GetText(string id);
    }
}