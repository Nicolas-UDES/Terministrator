using System.Text.RegularExpressions;

namespace Terministrator.Terministrator.Types
{
    public class ReplacingRegex
    {
        private readonly Regex _regex;
        private readonly string _replace;

        public ReplacingRegex(string pattern, string replace)
        {
            _regex = new Regex(pattern, RegexOptions.None);
            _replace = replace;
        }

        public string Replace(string text)
        {
            return _regex.Replace(text, _replace);
        }
    }
}
