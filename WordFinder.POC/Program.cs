using System;
using System.Collections.Generic;

namespace WordFinder.POC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Note: Use your own matrix and wordStream
            WordFinder wordFinder = new WordFinder(matrix);
            var ranking = wordFinder.Find(wordStream);

            IEnumerable<string> rankingList = ranking.Result;

            foreach (string word in rankingList)
            {
                Console.Write($"{word} \n");
            }

            Console.ReadLine();
        }
    }
}
