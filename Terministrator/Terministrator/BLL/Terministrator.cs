#region Usings

using System;
using System.Deployment.Application;
using System.Reflection;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of Terministartor. Is only used for static answers (no requests to the database).
    /// </summary>
    static class Terministrator
    {
        private static string CurrentVersion => ApplicationDeployment.IsNetworkDeployed
            ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            : Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// User command. Always sent when a user open a private discussion with the bot on Telegram.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void Start(Command command, Core core = null)
        {
            Entites.Message.SendMessage(Message.Answer(command.Message,
                $"./​Terministator\r\n" +
                $"Loading Terministrator v{CurrentVersion}{(char) ('a' + new Random().Next(0, 10))}...\r\n" +
                $"Initializing...\r\n" +
                $"Terministator ready. Send /help to begin troll annihilation."));
        }

        /// <summary>
        /// User command. Send every functions with an explanation for each of them.
        /// </summary>
        /// <remarks>
        /// Should add something to explain the bot's goals as well.
        /// </remarks>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void Help(Command command, Core core = null)
        {
            string nl = Environment.NewLine;
            string us = command.Message.Application.UserSymbols;
            Entites.Message.SendMessage(Message.Answer(command.Message,
                $"*Give*{nl}Synyax: \"/give {us}user amount\". Transfert points from your account to the targeted user.{nl}{nl}" +
                $"*Points*{nl}Synyax: \"/points [{us}user]\". Show how many points you or the targeted user have/has.{nl}{nl}" +
                $"*Set Amounts*{nl}Synyax: \"/setamounts defaultAmount[;messageType-rewardAmount]*\". Set for each message types (text, picture, sticker, etc.) the linked reward amount. If none is explcitely given for a message type, set it to the default one (unammed).{nl}{nl}" +
                $"*Top Posters*{nl}Synyax: \"/topposters\". Gives every users with their current message count.{nl}{nl}" +
                $"*Add Ad*{nl}Synyax: \"/addad adName\". Add an ad to send in the channel following the rules set in the ad system. The command must be replying to the message you want to set as an ad.{nl}{nl}" +
                $"*Set Ad System*{nl}Synyax: \"/setadsystem nbMessages (and|or) periode\". Sets when the ads should be sent. The periode is in minutes.{nl}{nl}" +
                $"*Privileges*{nl}Synyax: \"/privileges\". Tells you what is your privilege group.{nl}{nl}" +
                $"*Set Privileges*{nl}Synyax: \"/setprivileges [{us}user] privilegesName\". Set your or the user's privileges group.{nl}{nl}" +
                $"*Rename Privileges*{nl}Synyax: \"/renameprivileges oldName newName\". Rename a privileges group. The name must not already exist in the channel.{nl}{nl}" +
                $"*Add Privileges*{nl}Synyax: \"/addprivileges name [privilegesToCopy]\". Will create a new privileges group. If given another privileges group, will copy its rules; otherwise it takes the default ones.{nl}{nl}" +
                $"*Reset Blocked Words*{nl}Synyax: \"/resetblockedwords privileges\". Will reset the blocked words to the initial list.{nl}{nl}" +
                $"*Rules*{nl}Synyax: \"/rules [{us}user]\". Show the rules applying on your privileges group.{nl}{nl}" +
                $"*Help*{nl}Synyax: \"/help [{us}user]\". Show this message.{nl}{nl}"));
        }
    }
}