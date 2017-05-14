#region Usings

using System;
using Terministrator.Terministrator.Types;
using System.IO;
using Terministrator.Terministrator;

#endregion

namespace Terministrator
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            Core core = new Core();
            Application.TelegramApplication.Application.Instance.Token = Configuration.TelegramToken;
            core.Register(Application.TelegramApplication.Application.Instance);
            Application.DiscordApplication.Application.Instance.Token = Configuration.DiscordToken;
            core.Register(Application.DiscordApplication.Application.Instance);
            core.Start();
        }
    }
}