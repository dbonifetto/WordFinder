using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordFinder.POC
{
    public interface IWordFinder
    {
        Task<IEnumerable<string>> Find(IEnumerable<string> wordStream);
    }
}
