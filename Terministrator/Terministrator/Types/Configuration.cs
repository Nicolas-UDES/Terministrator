using System;
using System.Configuration;
using System.IO;

namespace Terministrator.Terministrator.Types
{
    public static class Configuration
    {
        private static KeyValueConfigurationCollection _config;

        private static KeyValueConfigurationCollection Config
        {
            get
            {
                if (_config != null)
                {
                    return _config;
                }

                ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = "Login.config" };

                if (!File.Exists(Environment.CurrentDirectory + "\\" + configFileMap.ExeConfigFilename))
                {
                    Console.WriteLine("Please create a Login.config file.");
                    return new KeyValueConfigurationCollection();
                }

                return _config = ((AppSettingsSection)ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None).GetSection("appSettings")).Settings;
            }
        }

        private static string GetDebugConfig(string name)
        {
#if DEBUG
            return Config[name + "Debug"]?.Value ?? Config[name]?.Value;
#else
            return Config[name]?.Value;
#endif
        }

        private static ReplacingRegex GetReplacingRegex(string name)
        {
            return ConfigurationManager.AppSettings[name] != null ? new ReplacingRegex(ConfigurationManager.AppSettings[name], ConfigurationManager.AppSettings[name + "Replace"]) : null;
        }

        private static string _telegramToken;

        public static string TelegramToken
        {
            get { return _telegramToken ?? (_telegramToken = GetDebugConfig("TelegramToken")); }
            set { _telegramToken = value; }
        }

        private static string _discordToken;

        public static string DiscordToken
        {
            get { return _discordToken ?? (_discordToken = GetDebugConfig("DiscordToken")); }
            set { _discordToken = value; }
        }

        private static string[] _defaultBlockedWords;

        public static string[] DefaultBlockedWords
        {
            get { return _defaultBlockedWords ?? (_defaultBlockedWords = ConfigurationManager.AppSettings["DefaultBlockedWords"]?.Split(',')); }
            set { _defaultBlockedWords = value; }
        }

        private static TimeSpan? _maxDelayToAct;

        /// <summary>
        /// Gets or sets the maximum delay to act. Usefull if the bot lost internet for a certain period of time.
        /// </summary>
        /// <value>
        /// The maximum delay to act.
        /// </value>
        public static TimeSpan MaxDelayToAct
        {
            get
            {
                if (_maxDelayToAct.HasValue)
                {
                    return _maxDelayToAct.Value;
                }
                int.TryParse(ConfigurationManager.AppSettings["MaxDelayToAct"], out int maxDelayToAct);
                return (_maxDelayToAct = TimeSpan.FromMilliseconds(maxDelayToAct)).Value;
            }
            set { _maxDelayToAct = value; }
        }

        #region Regex

        /// <summary>
        /// The control characters
        /// </summary>
        private static ReplacingRegex _controlCharacters;

        public static ReplacingRegex ControlCharacters
        {
            get { return _controlCharacters ?? (_controlCharacters = GetReplacingRegex("ControlCharacters")); }
            set { _controlCharacters = value; }
        }

        /// <summary>
        /// The smileys
        /// </summary>
        private static ReplacingRegex _smileys;

        public static ReplacingRegex Smileys
        {
            get { return _smileys ?? (_smileys = GetReplacingRegex("Smileys")); }
            set { _smileys = value; }
        }

        /// <summary>
        /// The quote
        /// </summary>
        private static ReplacingRegex _quote;

        public static ReplacingRegex Quote
        {
            get { return _quote ?? (_quote = GetReplacingRegex("Quote")); }
            set { _quote = value; }
        }

        /// <summary>
        /// The tiret
        /// </summary>
        private static ReplacingRegex _tiret;

        public static ReplacingRegex Tiret
        {
            get { return _tiret ?? (_tiret = GetReplacingRegex("Tiret")); }
            set { _tiret = value; }
        }

        /// <summary>
        /// The repeating character
        /// </summary>
        private static ReplacingRegex _repeatingChar;

        public static ReplacingRegex RepeatingChar
        {
            get { return _repeatingChar ?? (_repeatingChar = GetReplacingRegex("RepeatingChar")); }
            set { _repeatingChar = value; }
        }

        /// <summary>
        /// The repeating chars
        /// </summary>
        private static ReplacingRegex _repeatingChars;

        public static ReplacingRegex RepeatingChars
        {
            get { return _repeatingChars ?? (_repeatingChar = GetReplacingRegex("RepeatingChars")); }
            set { _repeatingChars = value; }
        }

        /// <summary>
        /// The spaces
        /// </summary>
        private static ReplacingRegex _spaces;

        public static ReplacingRegex Spaces
        {
            get { return _spaces ?? (_spaces = GetReplacingRegex("Spaces")); }
            set { _spaces = value; }
        }

        private static ReplacingRegex[] _r9kReplacingRegexes;

        public static ReplacingRegex[] R9KReplacingRegexes
        {
            get { return _r9kReplacingRegexes ?? (_r9kReplacingRegexes = new [] { ControlCharacters, Smileys, Quote, Tiret, RepeatingChar, RepeatingChars, Spaces }); }
            set { _r9kReplacingRegexes = value; }
        }

#endregion

    }
}
