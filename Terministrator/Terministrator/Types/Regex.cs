namespace Terministrator.Terministrator.Types
{
    /// <summary>
    /// Holds the regexes to find and replace text. Should be in the app.config in the future.
    /// </summary>
    public static class Regex
    {
        public const string ControlCharacters = @"[\cA-\cZ]";
        public const string ControlCharactersReplace = @"";

        public const string Smileys =
            @"(\:\w +\:|\<[\/\\] ? 3 |[\(\)\\\D |\*\$][\-\^]?[\:\;\=]|[\:\;\=B8] [\-\^]?[3DOPp\@\$\*\\\)\(\/\|])(?=\s|[\!\.\?]|$)";

        public const string SmileysReplace = @" ";
        public const string Quote = @"([a-zA-Z])'([a-zA-Z])";
        public const string QuoteReplace = @"$1$2";
        public const string Tiret = @"(?<!\w)-+|-+(?!\w)";
        public const string TiretReplace = @" ";
        public const string RepeatingChar = @"(.)\1{2,}";
        public const string RepeatingCharReplace = @"$1$1";
        public const string RepeatingChars = @"(..)\1{2,}";
        public const string RepeatingCharsReplace = @"$1$1";
        public const string Spaces = @"[\s]{2,}";
        public const string SpacesReplace = @" ";
    }
}