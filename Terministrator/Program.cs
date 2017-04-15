#region Usings

using System;
using System.Configuration;
using Terministrator.Terministrator;

#endregion

namespace Terministrator
{
    static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            Core core = new Core();
            Application.TelegramApplication.Application.Instance.Token = ConfigurationManager.AppSettings["TelegramTokenDebug"];
            core.Register(Application.TelegramApplication.Application.Instance);
            Application.DiscordApplication.Application.Instance.Token = ConfigurationManager.AppSettings["DiscordToken"];
            core.Register(Application.DiscordApplication.Application.Instance);
            core.Start();
        }
    }
}