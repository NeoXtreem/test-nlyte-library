using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Tests
{
    [TestClass]
    public class LibraryTests
    {
        private static string _bookText;

        [TestMethod]
        public void MostCommonWordsTest()
        {
            GetTopWordsTest();
        }

        [TestMethod]
        public void SearchTest()
        {
            GetTopWordsTest(BuildRandomString(3));
        }

        private static void GetTopWordsTest(string prefix = "")
        {
            // Arrange
            Random random = new();
            var maxWords = random.Next(5, 10);
            var minLength = random.Next(5, 6);

            var wordCounts = new List<WordCount>();
            for (var i = 1; i < 100; i++)
            {
                // Generate a random word with a length and prefix based on functions of the loop variable.
                wordCounts.Add(new WordCount(BuildRandomString(i % 8 + 1, i % 8 > 3 ? prefix : string.Empty), i));
            }

            _bookText = string.Join(" ", wordCounts
                .SelectMany(x => Enumerable.Repeat(x.Word, x.Count))
                .OrderBy(_ => random.Next()).AsEnumerable());

            var expected = wordCounts
                .Where(w => w.Word.Length >= minLength && w.Word.StartsWith(prefix))
                .OrderByDescending(w => w.Count)
                .Take(maxWords)
                .Select(w => (char.ToUpperInvariant(w.Word.First()) + w.Word.Substring(1).ToLowerInvariant(), w.Count));

            // Act
            var sut = new LibraryService(new FakeBookRepository());
            var result = sut.GetTopWords(Guid.NewGuid().ToString(), maxWords, minLength, string.Empty);

            // Assert

            // Use tuples with primitives for comparison as these are fully value types.
            CollectionAssert.AreEqual(expected.ToArray(), result.Select(w => (w.Word, w.Count)).ToArray());
        }

        private static string BuildRandomString(int length, string prefix = "") => prefix + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()).Substring(0, length);

        private class FakeBookRepository : IBookRepository
        {
            public IEnumerable<BookInfo> GetCatalogue() => throw new NotImplementedException();

            public string GetText(string id) => _bookText;
        }
    }
}