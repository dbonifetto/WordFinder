namespace WordFinder.POC
{
    public class WordAppearance
    {
        public WordAppearance(string word, int appearance)
        {
            this.Word = word;
            this.Appearance = appearance;
        }

        public string Word { get; private set; }
        public int Appearance { get; private set; }        
    }
}
