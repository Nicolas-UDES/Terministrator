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
    class Message : ApplicationReferencable, IMessage
    {
        public static Action<Message> SendMessage = m => { m.Application.SendMessage(m); };

        public Message()
        {
        }

        public Message(Message message) : base(message.Application, message.IdForApplication)
        {
            MessageId = message.MessageId;
            UserToChannelId = message.UserToChannelId;
            UserToChannel = message.UserToChannel;
            RepliesToId = message.RepliesToId;
            RepliesTo = message.RepliesTo;
            SentOn = message.SentOn;
            Deleted = message.Deleted;
            MessageTypeId = message.MessageTypeId;
            MessageType = message.MessageType;
            Texts = message.Texts;
            Files = message.Files;
            AdSystems = message.AdSystems;
            JoinedUserId = message.JoinedUserId;
            JoinedUser = message.JoinedUser;
        }

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
            JoinedUser = joinedUser;
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

        public virtual UserToChannel JoinedUser { get; set; }

        public string GetApplicationId()
        {
            return IdForApplication;
        }

        public IUser GetFrom()
        {
            return UserToChannel.User;
        }

        public IChannel GetChannel()
        {
            return UserToChannel.Channel;
        }

        public DateTime GetSentDate()
        {
            return SentOn;
        }

        public string GetText()
        {
            return Texts != null && Texts.Any() ? Texts?.OrderByDescending(x => x.SetOn).ElementAt(0).ZeText : null;
        }

        public IMessage GetRepliesTo()
        {
            return RepliesTo;
        }

        public IUser GetJoinedUser()
        {
            return JoinedUser.User;
        }

        public override string ToString()
        {
            return '[' + SentOn.ToString("HH:mm") + "] " + UserToChannel.User + ": " + GetText() ?? "{no text}";
        }
    }
}