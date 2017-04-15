using System.Collections.Generic;

namespace Terministrator.Application.Interface
{
    static class ApplicationFactory
    {
        public static IApplication[] Applications { get; } = { TelegramApplication.Application.Instance, DiscordApplication.Application.Instance };

        public enum Application
        {
            Telegram,
            Discord
        }

        public static IApplication Get(Application app)
        {
            return app == Application.Telegram
                ? (IApplication) TelegramApplication.Application.Instance
                : (IApplication) DiscordApplication.Application.Instance;
        }

        public static IApplication Get(string app)
        {
            return app == TelegramApplication.Application.Instance.GetApplicationName()
                ? (IApplication) TelegramApplication.Application.Instance
                : (IApplication) DiscordApplication.Application.Instance;
        }
    }
}