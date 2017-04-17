#region Usings

using System;
using System.Diagnostics;
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
        // 10%
        private static readonly double SignalRatio = 0.1;
        private static readonly TimeSpan TimeoutLimit = TimeSpan.FromDays(365);

        /// <summary>
        /// Apply the rules on a newly received message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isCommand">If the message was recognized as a command.</param>
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
                if ((message.UserToChannel?.Privileges?.Rules?.SpamDelay.HasValue ?? false) && message.UserToChannel.Privileges.Rules.SpamDelay > TimeSpan.Zero)
                {
                    ApplySpamDelay(message);
                }
            }
        }

        /// <summary>
        /// Applies the spam delay. If the delay wasn't respected, <see cref="Fail"/> them.
        /// </summary>
        /// <param name="message">The message to analyze.</param>
        private static void ApplySpamDelay(Entites.Message message)
        {
            Debug.Assert(message.UserToChannel.Privileges.Rules.SpamDelay != null, "message.UserToChannel.Privileges.Rules.SpamDelay != null");
            if (message.SentOn - UserToChannel.GetMessageBefore(message.UserToChannel, message.SentOn).SentOn < message.UserToChannel.Privileges.Rules.SpamDelay.Value)
            {
                Fail(message, "You are sending messages too quickly. Be careful or you will be kicked.");
            }
        }

        /// <summary>
        /// Applies the blocked words filter. If a blocked word is found in the message, <see cref="Fail"/> them.
        /// </summary>
        /// <param name="message">The message to analyze.</param>
        private static void ApplyBlockedWords(Entites.Message message)
        {
            foreach (Entites.BlockedWord blockedWord in message.UserToChannel.Privileges.Rules.BlockedWords)
            {
                if (message.GetText().IndexOf(blockedWord.Word, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    Fail(message, "Your message contains a blocked word. Be careful or you will be kicked.");
                    return;
                }
            }
        }

        /// <summary>
        /// Applies the r9k filter. If a message with an identical r9k text is found, <see cref="Fail"/> them.
        /// </summary>
        /// <param name="message">The message to analyze.</param>
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

        /// <summary>
        /// Fails the user due to the specified message. Warn them if they weren't muted, kick them otherwise.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="reason">The reason.</param>
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

        /// <summary>
        /// Kicks the specified user from the channel if possible. Otherwise warn the mods.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
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

        /// <summary>
        /// Sends a warning message to the mods about someone whom should be kicked.
        /// </summary>
        /// <param name="userToChannel">The user to channel to warn about.</param>
        private static void SendWarningMessages(Entites.UserToChannel userToChannel)
        {
            foreach (IUser user in userToChannel.Application.GetMods(DAL.UserToChannel.LoadChannel(userToChannel).Channel))
            {
                Entites.Channel c = Channel.GetPrivateChannel(User.GetOrCreate(user));
                if (c != null)
                {
                    userToChannel.Application.SendMessage(Message.Create("Warning!", userToChannel));
                }
            }
        }

        /// <summary>
        /// Apply the R9K filter to a string.
        /// </summary>
        /// <param name="msg">The text.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Shorten the use of a regex to replace a pattern in a text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>The replaced text.</returns>
        private static string RegexReplace(string text, string pattern, string replacement)
        {
            return new System.Text.RegularExpressions.Regex(pattern, RegexOptions.None).Replace(text, replacement);
        }

        /// <summary>
        /// Calculates the mute time one should get if this is their Nth mute.
        /// </summary>
        /// <param name="nbMutes">The nb mutes, greather than 0</param>
        /// <returns></returns>
        private static TimeSpan GetMuteTime(int nbMutes)
        {
            return nbMutes <= 0
                ? TimeSpan.Zero
                : TimeSpan.FromSeconds(2.5 * Math.Pow(Math.E, MuteTimeConstant * nbMutes));
        }

        /// <summary>
        /// Creates the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with an updated ID.</returns>
        public static Entites.Rules Create(Entites.Rules rules)
        {
            return DAL.Rules.Create(rules);
        }

        /// <summary>
        /// Creates a rules object with as few rules as possible.
        /// </summary>
        /// <returns>The requested rules</returns>
        public static Entites.Rules GetNewModRules()
        {
            return new Entites.Rules(null, false, false, false, false, false);
        }

        /// <summary>
        /// Creates a rules object with a maximum amount of rules.
        /// </summary>
        /// <returns>The requested rules</returns>
        public static Entites.Rules GetNewUserRules()
        {
            Entites.Rules rules = new Entites.Rules(TimeSpan.FromSeconds(10), true, true, true, true, true)
            {
                BlockedWords = BlockedWord.GetDefaultBlockedWords()
            };
            return rules;
        }

        /// <summary>
        /// User command. Gets the rules applied to them.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GetRules(Command command, Core core = null)
        {
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                command.Message.UserToChannel.Privileges.Rules.ToString()));
        }

        /// <summary>
        /// Mod command. Sets the rules applying to a privileges group.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void SetRules(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
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

        /// <summary>
        /// Mod command. Resets the blocked words set on a privileges group.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void ResetBlockedWords(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
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