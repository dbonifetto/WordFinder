using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dasync.Collections;

namespace WordFinder.POC
{
    public class WordFinder : IWordFinder
    {
        private IEnumerable<string> HorizontalWordsMatrix { get; set; }
        private IEnumerable<string> VerticalWordsMatrix { get; set; }

        private int ColumnsQuantity { get; }
        private int RowsQuantity { get; }

        private readonly ConcurrentBag<WordAppearance> WordAppearences;

        private const int WordsToReturn = 10;

        public WordFinder(IEnumerable<string> matrix)
        {
            this.ColumnsQuantity = matrix.First().Count();
            this.RowsQuantity = matrix.Count();

            this.HorizontalWordsMatrix = matrix;
            this.VerticalWordsMatrix = this.GetVerticalWords(matrix);

            this.WordAppearences = new ConcurrentBag<WordAppearance>();
        }

        public async Task<IEnumerable<string>> Find(IEnumerable<string> wordStream)
        {
            var results = await ProcessParallel(wordStream);

            return results.OrderByDescending(o => o.Appearance).Select(s => s.Word).Take(WordsToReturn).ToList();
        }

        private IEnumerable<string> GetVerticalWords(IEnumerable<string> matrix)
        {
            List<string> verticalWordsMatrix = new List<string>();
            string[] matrixArray = matrix.ToArray();

            for (int columnIndex = 0; columnIndex < this.ColumnsQuantity; columnIndex++)
            {
                StringBuilder verticalWord = new StringBuilder(this.ColumnsQuantity);
                for (int rowIndex = 0; rowIndex < this.RowsQuantity; rowIndex++)
                {
                    verticalWord.Append(matrixArray[rowIndex].ElementAt(columnIndex));
                }

                verticalWordsMatrix.Add(verticalWord.ToString());
            }

            return verticalWordsMatrix;
        }        

        private async Task<ConcurrentBag<WordAppearance>> ProcessParallel(IEnumerable<string> wordStream)
        {
            await wordStream.ParallelForEachAsync(async word =>
            {
                var result = FindWord(word);
                if (!this.WordAppearences.Any(a => a.Word == word))
                    WordAppearences.Add(result);
            }, 0);

            return this.WordAppearences;
        }

        private WordAppearance FindWord(string word)
        {
            if (!this.HorizontalWordsMatrix.Any(s => s.Contains(word)) &&!this.VerticalWordsMatrix.Any(s => s.Contains(word)))
            {
                return new WordAppearance(string.Empty, 0);
            }
            else
            {
                int horizontalAppearances = this.HorizontalWordsMatrix != null ?
                                            this.HorizontalWordsMatrix.Where(s => s.Contains(word)).Count() :
                                            0;
                int verticalAppearances = this.VerticalWordsMatrix != null ?
                                          this.VerticalWordsMatrix.Where(s => s.Contains(word)).Count() :
                                          0;
                int totalAppearances = horizontalAppearances + verticalAppearances;

                return new WordAppearance(word, totalAppearances);
            }
        }
    }
}
