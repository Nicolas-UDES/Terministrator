#region Usings

using System;
using System.Configuration;
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

            KeyValueConfigurationCollection loginConfig = GetTokenConfig();

            Core core = new Core();
            Application.TelegramApplication.Application.Instance.Token = loginConfig["TelegramToken"].Value;
            core.Register(Application.TelegramApplication.Application.Instance);
            Application.DiscordApplication.Application.Instance.Token = loginConfig["DiscordToken"].Value;
            core.Register(Application.DiscordApplication.Application.Instance);
            core.Start();
        }

        /// <summary>
        /// Gets the token configuration.
        /// </summary>
        // TODO: Send in a static class for config files
        private static KeyValueConfigurationCollection GetTokenConfig()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = "Login.config"};

            if (!File.Exists(Environment.CurrentDirectory + "\\" + configFileMap.ExeConfigFilename))
            {
                Console.WriteLine("Please create a Login.config file.");
                return null;
            }

            return ((AppSettingsSection) ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None).GetSection("appSettings")).Settings;
        }
    }
}