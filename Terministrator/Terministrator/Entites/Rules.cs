#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Rules
    {
        public Rules()
        {
        }

        public Rules(TimeSpan? spamDelay, bool extensionBlocked, bool domainBlocked, bool messageTypeBlocked,
            bool blockedWordsEnabled, bool r9kEnabled)
        {
            SpamDelay = spamDelay;
            ExtensionBlocked = extensionBlocked;
            DomainBlocked = domainBlocked;
            MessageTypeBlocked = messageTypeBlocked;
            BlockedWordsEnabled = blockedWordsEnabled;
            R9KEnabled = r9kEnabled;
        }

        [Key]
        public int RulesId { get; set; }

        public virtual List<Privileges> Privileges { get; set; }

        public TimeSpan? SpamDelay { get; set; }

        public bool ExtensionBlocked { get; set; }

        public virtual List<Extension> Extensions { get; set; }

        public bool DomainBlocked { get; set; }

        public virtual List<Domain> Domains { get; set; }

        public bool MessageTypeBlocked { get; set; }

        public virtual List<MessageType> MessageTypes { get; set; }

        public bool BlockedWordsEnabled { get; set; }

        public virtual List<BlockedWord> BlockedWords { get; set; }

        public bool R9KEnabled { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return
                $"Message Delay: {((SpamDelay ?? TimeSpan.Zero) == TimeSpan.Zero ? "none" : SpamDelay.ToString())}, " +
                $"R9K Enabled: {(R9KEnabled ? "yes" : "no")}. " +
                (!ExtensionBlocked || Extensions.Count > 0
                    ? $"Write /extensions to know which file extensions you {(ExtensionBlocked ? "can't" : "can")} transfert. "
                    : "") +
                (!DomainBlocked || Domains.Count > 0
                    ? $"Write /domains to know which websites you {(DomainBlocked ? "can't" : "can")} send link from. "
                    : "") +
                (!MessageTypeBlocked || MessageTypes.Count > 0
                    ? $"Write /messagetypes to know which kinds of message you {(MessageTypeBlocked ? "can't" : "can")} send. "
                    : "") +
                (BlockedWordsEnabled && BlockedWords.Count > 0
                    ? "Write /blockedwords to know which words you can't include in your messages."
                    : "");
        }

        /// <summary>
        /// Copies the specified rules.
        /// </summary>
        /// <param name="copy">The copy.</param>
        /// <returns>A new Rules object</returns>
        public static Rules Copy(Rules copy)
        {
            return copy == null
                ? null
                : new Rules(copy.SpamDelay, copy.ExtensionBlocked, copy.DomainBlocked, copy.MessageTypeBlocked,
                    copy.BlockedWordsEnabled, copy.R9KEnabled);
        }
    }
}