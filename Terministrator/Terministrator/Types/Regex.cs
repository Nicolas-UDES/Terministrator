namespace Terministrator.Terministrator.Types
{
    /// <summary>
    /// Holds the regexes to find and replace text. Should be in the app.config in the future.
    /// </summary>
    public static class Regex
    {
        /// <summary>
        /// The control characters
        /// </summary>
        public const string ControlCharacters = @"[\cA-\cZ]";

        /// <summary>
        /// The control characters replacement
        /// </summary>
        public const string ControlCharactersReplace = @"";

        /// <summary>
        /// The smileys
        /// </summary>
        public const string Smileys =
            @"(\:\w +\:|\<[\/\\] ? 3 |[\(\)\\\D |\*\$][\-\^]?[\:\;\=]|[\:\;\=B8] [\-\^]?[3DOPp\@\$\*\\\)\(\/\|])(?=\s|[\!\.\?]|$)";

        /// <summary>
        /// The smileys replacement
        /// </summary>
        public const string SmileysReplace = @" ";

        /// <summary>
        /// The quote
        /// </summary>
        public const string Quote = @"([a-zA-Z])'([a-zA-Z])";

        /// <summary>
        /// The quote replacement
        /// </summary>
        public const string QuoteReplace = @"$1$2";

        /// <summary>
        /// The tiret
        /// </summary>
        public const string Tiret = @"(?<!\w)-+|-+(?!\w)";

        /// <summary>
        /// The tiret replacement
        /// </summary>
        public const string TiretReplace = @" ";

        /// <summary>
        /// The repeating character
        /// </summary>
        public const string RepeatingChar = @"(.)\1{2,}";

        /// <summary>
        /// The repeating character replacement
        /// </summary>
        public const string RepeatingCharReplace = @"$1$1";

        /// <summary>
        /// The repeating chars
        /// </summary>
        public const string RepeatingChars = @"(..)\1{2,}";

        /// <summary>
        /// The repeating chars replacement
        /// </summary>
        public const string RepeatingCharsReplace = @"$1$1";

        /// <summary>
        /// The spaces
        /// </summary>
        public const string Spaces = @"[\s]{2,}";

        /// <summary>
        /// The spaces replacement
        /// </summary>
        public const string SpacesReplace = @" ";
    }
}