namespace Library.Models
{
    public class WordCount
    {
        public WordCount(string word, int count)
        {
            Word = word;
            Count = count;
        }

        public string Word { get; }

        public int Count { get; }
    }
}