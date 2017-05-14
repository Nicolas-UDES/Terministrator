namespace Terministrator.Application.Interface
{
    /// <summary>
    /// Gives access to every single application at the same place. Should be deleted in a future upgrade.
    /// </summary>
    static class ApplicationFactory
    {
        public enum Application
        {
            Telegram,
            Discord
        }

        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <value>
        /// The applications.
        /// </value>
        public static IApplication[] Applications { get; } = {
            TelegramApplication.Application.Instance,
            DiscordApplication.Application.Instance
        };

        /// <summary>
        /// Gets the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static IApplication Get(Application app)
        {
            return app == Application.Telegram
                ? TelegramApplication.Application.Instance
                : (IApplication) DiscordApplication.Application.Instance;
        }

        /// <summary>
        /// Gets the specified application.
        /// </summary>
        /// <param name="app">The application's name.</param>
        /// <returns></returns>
        public static IApplication Get(string app)
        {
            return app == TelegramApplication.Application.Instance.ApplicationName
                ? TelegramApplication.Application.Instance
                : (IApplication) DiscordApplication.Application.Instance;
        }
    }
}