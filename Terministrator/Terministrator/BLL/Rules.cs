#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Types;
using Regex = Terministrator.Terministrator.Types.Regex;

#endregion

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
            if (!isCommand && message.Texts.Any())
            {
                if (message.UserToChannel?.Privileges?.Rules?.R9KEnabled ?? false)
                {
                    ApplyR9K(message);
                }
                if (message.UserToChannel?.Privileges?.Rules?.BlockedWordsEnabled ?? false)
                {
                    ApplyBlockedWords(message);
                }
            }
        }

        private static void ApplyBlockedWords(Entites.Message message)
        {
            foreach (Entites.BlockedWord blockedWord in message.UserToChannel.Privileges.Rules.BlockedWords)
            {
                if (message.GetText().Contains(blockedWord.Word))
                {
                    Fail(message, "Your message contains a blocked word. Be careful or you will be kicked.");
                    return;
                }
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
                Fail(message, "Your privileges group makes you in r9k mode and the message you attempted to send is not unique. Be careful or you will be kicked.");
            }
        }

        private static void Fail(Entites.Message message, string reason)
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
            Entites.Message.SendMessage(Message.Answer(message, reason));
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
            msg = RegexReplace(msg, Regex.ControlCharacters, Regex.ControlCharactersReplace);
            msg = RegexReplace(msg, Regex.Smileys, Regex.SmileysReplace);
            msg = RegexReplace(msg, Regex.Quote, Regex.QuoteReplace);
            msg = RegexReplace(msg, Regex.Tiret, Regex.TiretReplace);
            msg = RegexReplace(msg, Regex.RepeatingChar, Regex.RepeatingCharReplace);
            msg = RegexReplace(msg, Regex.RepeatingChars, Regex.RepeatingCharsReplace);
            msg = msg.Trim();
            msg = RegexReplace(msg, Regex.Spaces, Regex.SpacesReplace);

            return msg;
        }

        private static string RegexReplace(string text, string pattern, string replacement)
        {
            return new System.Text.RegularExpressions.Regex(pattern, RegexOptions.None).Replace(text, replacement);
        }

        private static TimeSpan GetMuteTime(int nbMutes)
        {
            return nbMutes <= 0
                ? TimeSpan.Zero
                : TimeSpan.FromSeconds(2.5 * Math.Pow(Math.E, MuteTimeConstant * nbMutes));
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
            Entites.Rules rules = new Entites.Rules(TimeSpan.FromSeconds(10), true, true, true, false, true)
            {
                BlockedWords = BlockedWord.GetDefaultBlockedWords()
            };
            return rules;
        }

        public static void GetRules(Command command, Core core = null)
        {
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                command.Message.UserToChannel.Privileges.Rules.ToString()));
        }

        public static void SetRules(Command command, Core core = null)
        {
            if (Tools.IsNotAdminThenSendWarning(command))
            {
                return;
            }

            string[] arguements = command.SplitArguements(count: 7);
            if (arguements.Length < 7)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    "Incorect syntax. Please use the following one: /setrules privileges messageDelayInSeconds extensionBlocked? domainBlocked? messageTypeBlocked? blockedWordsEnabled? r9kEnabled?. Note that the fields ending by ? must be either 'y' or 'n'."));
                return;
            }
            for (int i = 2; i < arguements.Length; ++i)
            {
                if (!arguements[i].Equals("y", StringComparison.InvariantCultureIgnoreCase) && !arguements[i].Equals("n", StringComparison.InvariantCultureIgnoreCase))
                {
                    command.Message.Application.SendMessage(Message.Answer(command.Message,
                        "Incorect syntax. Please use the following one: /setrules privileges messageDelayInSeconds extensionBlocked? domainBlocked? messageTypeBlocked? blockedWordsEnabled? r9kEnabled?. Note that the fields ending by ? must be either 'y' or 'n'."));
                    return;
                }
            }

            int messageDelay;
            if (!int.TryParse(arguements[1], out messageDelay))
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"Please write a time in seconds rather than {arguements[1]}"));
                return;
            }

            Entites.Privileges privileges = Privileges.GetPrivileges(command.Message.UserToChannel.Channel, arguements[0]);
            if (privileges == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No privileges group nammed {arguements[0]} was found."));
                return;
            }
            DAL.Privileges.LoadRules(privileges);

            privileges.Rules.SpamDelay = messageDelay <= 0 ? null : (TimeSpan?) TimeSpan.FromSeconds(messageDelay);
            privileges.Rules.ExtensionBlocked = "y".Equals(arguements[2], StringComparison.InvariantCultureIgnoreCase);
            privileges.Rules.DomainBlocked = "y".Equals(arguements[3], StringComparison.InvariantCultureIgnoreCase);
            privileges.Rules.MessageTypeBlocked = "y".Equals(arguements[4], StringComparison.InvariantCultureIgnoreCase);
            privileges.Rules.BlockedWordsEnabled = "y".Equals(arguements[5], StringComparison.InvariantCultureIgnoreCase);
            privileges.Rules.R9KEnabled = "y".Equals(arguements[6], StringComparison.InvariantCultureIgnoreCase);
            DAL.Rules.Update(privileges.Rules);
            command.Message.Application.SendMessage(Message.Answer(command.Message, $"The rules of {arguements[0]} were successfully updated."));
        }

        public static void ResetBlockedWords(Command command, Core core = null)
        {
            if (Tools.IsNotAdminThenSendWarning(command))
            {
                return;
            }

            if (string.IsNullOrEmpty(command.Arguement))
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    "You must specify the privileges group which's blocked words shall be reseted."));
                return;
            }

            Entites.Privileges privileges = Privileges.GetPrivileges(command.Message.UserToChannel.Channel, command.Arguement);
            if (privileges == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No privileges group nammed {command.Arguement} was found."));
                return;
            }

            DAL.Privileges.LoadRules(privileges).Rules.BlockedWords = BlockedWord.GetDefaultBlockedWords();
            DAL.Rules.UpdateBlockedWords(privileges.Rules);
            command.Message.Application.SendMessage(Message.Answer(command.Message, $"Blocked words of {command.Arguement} were successfully reinitialized."));
        }
    }
}