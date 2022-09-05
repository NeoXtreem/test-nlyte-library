using System;
using System.Collections.Generic;
using System.Linq;
using Library.Models;
using Library.Repositories.Interfaces;

namespace Library.Services
{
    internal class LibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly Dictionary<string, WordCount[]> _wordCountsCache = new();
        private readonly Dictionary<(string, string), WordCount[]> _topWordsCache = new();

        public LibraryService(IBookRepository bookRepository) => _bookRepository = bookRepository;

        public IEnumerable<BookInfo> GetCatalogue() => _bookRepository.GetCatalogue();

        public IEnumerable<WordCount> GetTopWords(string id, int maxWords, int minLength, string startsWith)
        {
            // Get the counts for all words in the book with specified ID (from the cache if possible).
            if (!_wordCountsCache.TryGetValue(id, out var wordCounts))
            {
                wordCounts = _bookRepository.GetText(id)
                    .Split(new[] { " ", ".", ",", ";", ":", "?", "!", "--", "—", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(w => w.Trim())
                    .Select(w => char.ToUpperInvariant(w.First()) + w.Substring(1).ToLowerInvariant())
                    .Where(w => w.Length >= minLength)
                    .GroupBy(w => w)
                    .Select(g => new WordCount(g.Key, g.Count()))
                    .ToArray();

                _wordCountsCache.Add(id, wordCounts);
            }

            // Get the top words for the book with specified ID and starting with the specified text (from the cache if possible).
            var topWordsKey = (id, startsWith);
            if (!_topWordsCache.TryGetValue(topWordsKey, out var topWords))
            {
                topWords = wordCounts
                    .Where(w => w.Word.StartsWith(startsWith ?? string.Empty, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(w => w.Count)
                    .Take(maxWords)
                    .ToArray();

                _topWordsCache.Add(topWordsKey, topWords);
            }

            return topWords;
        }
    }
}