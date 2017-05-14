#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the messages. Contains all the datas required for a message.
    /// </summary>
    /// <seealso cref="ApplicationReferencable" />
    /// <seealso cref="IMessage" />
    class Message : ApplicationReferencable, IMessage
    {
        public static Action<Message> SendMessage = m => { m.Application.SendMessage(m); };

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="idMessageForApplication">The identifier message for application.</param>
        /// <param name="sentOn">The sent on.</param>
        /// <param name="userToChannel">The user to channel.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="repliesTo">The replies to.</param>
        /// <param name="deleted">if set to <c>true</c> [deleted].</param>
        /// <param name="joinedUser">The joined user.</param>
        public Message(Application application, string idMessageForApplication, DateTime sentOn,
            UserToChannel userToChannel, MessageType messageType, Message repliesTo = null, bool deleted = false,
            UserToChannel joinedUser = null) : base(application, idMessageForApplication)
        {
            UserToChannelId = userToChannel.UserToChannelId;
            UserToChannel = userToChannel;
            RepliesToId = repliesTo?.MessageId;
            RepliesTo = repliesTo;
            MessageTypeId = messageType.MessageTypeId;
            MessageType = messageType;
            SentOn = sentOn;
            Deleted = deleted;
            JoinedUserId = joinedUser?.UserToChannelId;
            JoinedUserToChannel = joinedUser;
        }

        [Key]
        public int MessageId { get; set; }

        [Required]
        [ForeignKey("UserToChannel")]
        public int UserToChannelId { get; set; }

        public virtual UserToChannel UserToChannel { get; set; }

        [ForeignKey("RepliesTo")]
        public int? RepliesToId { get; set; }

        public virtual Message RepliesTo { get; set; }

        [ForeignKey("MessageType")]
        public int MessageTypeId { get; set; }

        public virtual MessageType MessageType { get; set; }

        public virtual List<Text> Texts { get; set; }

        public virtual List<File> Files { get; set; }

        public virtual List<AdSystem> AdSystems { get; set; }

        public DateTime SentOn { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey("JoinedUser")]
        public int? JoinedUserId { get; set; }

        public virtual UserToChannel JoinedUserToChannel { get; set; }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        [NotMapped]
        public string ApplicationId => IdForApplication;

        /// <summary>
        /// Gets the author of the message.
        /// </summary>
        /// <returns>
        /// The author
        /// </returns>
        [NotMapped]
        public IUser From => UserToChannel.User;

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <returns>
        /// The channel
        /// </returns>
        [NotMapped]
        public IChannel Channel => UserToChannel.Channel;

        /// <summary>
        /// Gets the sent date.
        /// </summary>
        /// <returns>
        /// The sent date
        /// </returns>
        [NotMapped]
        public DateTime SentDate => SentOn;

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>
        /// The text
        /// </returns>
        [NotMapped]
        public string Text => Texts != null && Texts.Any() ? Texts?.OrderByDescending(x => x.SetOn).ElementAt(0).ZeText : null;

        /// <summary>
        /// Gets the joined user.
        /// </summary>
        /// <returns>
        /// The joinded user
        /// </returns>
        [NotMapped]
        public IUser JoinedUser => JoinedUserToChannel.User;

        [NotMapped]
        IMessage IMessage.RepliesTo => RepliesTo;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() =>'[' + SentOn.ToString("HH:mm") + "] " + UserToChannel.User + ": " + Text ?? "{no text}";
    }
}