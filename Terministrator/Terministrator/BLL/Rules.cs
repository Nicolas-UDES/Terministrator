using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Types;
using Regex = System.Text.RegularExpressions.Regex;

namespace Terministrator.Terministrator.BLL
{
    internal static class Rules
    {
        // 3119/4500
        private const double MuteTimeConstant = 0.69311111111111;
        private static readonly double SignalRatio = 0.1;
        private static readonly TimeSpan TimeoutLimit = TimeSpan.FromDays(365);

        internal static void ReceivedMessage(Entites.Message message, bool isCommand)
        {
            if (!isCommand && message.Texts.Any() && (message.UserToChannel?.Privileges?.Rules?.R9KEnabled ?? false))
            {
                ApplyR9K(message);
            }
        }

        private static void ApplyR9K(Entites.Message message)
        {

            Entites.Text text = message.Texts.OrderByDescending(x => x.SetOn).First();
            text.R9KText = ToR9KText(text.ZeText);

            // Check if not enough content or if we know the message
            if (text.ZeText.Length == 0 ||
                text.ZeText.Length > 10 && (text.R9KText.Length / Convert.ToDouble(text.ZeText.Length) < SignalRatio ||
                Text.SearchAndLink(text).SimilarTexts != null))
            {
                Fail(message);
            }
        }

        private static void Fail(Entites.Message message)
        {
            message.UserToChannel.NbSilences++;
            TimeSpan muteTime = GetMuteTime(message.UserToChannel.NbSilences);

            // Someone abusing the system in some way or if someone gets failed while already muted
            if (muteTime > TimeoutLimit || message.UserToChannel.SilencedTo - DateTime.UtcNow > TimeSpan.FromSeconds(1))
            {
                Kick(message.UserToChannel);
                return;
            }

            message.UserToChannel.SilencedTo = DateTime.UtcNow + muteTime;
            UserToChannel.Update(message.UserToChannel);
            DAL.UserToChannel.LoadChannel(message.UserToChannel);
            Entites.Message.SendMessage(Message.Answer(message, "This room is in r9k mode and the message you attempted to send is not unique."));
        }

        private static void Kick(Entites.UserToChannel userToChannel)
        {
            if (userToChannel.Application.CanKick(userToChannel.Channel))
            {
                userToChannel.Application.Kick(userToChannel.User, userToChannel.Channel);
            }
            else
            {
                SendWarningMessages(userToChannel);
            }
        }

        private static void SendWarningMessages(Entites.UserToChannel userToChannel)
        {
            foreach (IUser user in userToChannel.Application.Mods(DAL.UserToChannel.LoadChannel(userToChannel).Channel))
            {
                Entites.Channel c = Channel.GetPrivateChannel(User.GetOrCreate(user));
                if (c != null)
                {
                    userToChannel.Application.SendMessage(Message.Create("Warning!", userToChannel));
                }
            }
        }

        public static string ToR9KText(string msg)
        {
            msg = msg.ToLower();
            msg = RegexReplace(msg, Types.Regex.ControlCharacters, Types.Regex.ControlCharactersReplace);
            msg = RegexReplace(msg, Types.Regex.Smileys, Types.Regex.SmileysReplace);
            msg = RegexReplace(msg, Types.Regex.Quote, Types.Regex.QuoteReplace);
            msg = RegexReplace(msg, Types.Regex.Tiret, Types.Regex.TiretReplace);
            msg = RegexReplace(msg, Types.Regex.RepeatingChar, Types.Regex.RepeatingCharReplace);
            msg = RegexReplace(msg, Types.Regex.RepeatingChars, Types.Regex.RepeatingCharsReplace);
            msg = msg.Trim();
            msg = RegexReplace(msg, Types.Regex.Spaces, Types.Regex.SpacesReplace);

            return msg;
        }

        private static string RegexReplace(string text, string pattern, string replacement)
        {
            return new Regex(pattern, RegexOptions.None).Replace(text, replacement);
        }

        private static TimeSpan GetMuteTime(int nbMutes)
        {
            return nbMutes <= 0 ? TimeSpan.Zero : TimeSpan.FromSeconds(2.5 * Math.Pow(Math.E, MuteTimeConstant * nbMutes));
        }

        public static Entites.Rules Create(Entites.Rules rules)
        {
            return DAL.Rules.Create(rules);
        }

        public static Entites.Rules GetNewModRules()
        {
            return new Entites.Rules(null, false, false, false, false, false);
        }

        public static Entites.Rules GetNewUserRules()
        {
            return new Entites.Rules(TimeSpan.FromSeconds(10), true, true, true, false, true);
        }

        public static void GetRules(Command command, Core core = null)
        {
            command.Message.Application.SendMessage(Message.Answer(command.Message, command.Message.UserToChannel.Privileges.Rules.ToString()));
        }
    }
}
