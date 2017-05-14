#region Usings

using System;

#endregion

namespace Terministrator.Application.Interface
{
    /// <summary>
    /// Interface of what a message should implement for the Core.
    /// </summary>
    interface IMessage
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>The application identifier</returns>
        string ApplicationId { get; }

        /// <summary>
        /// Gets the author of the message.
        /// </summary>
        /// <returns>The author</returns>
        IUser From { get; }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <returns>The channel</returns>
        IChannel Channel { get; }

        /// <summary>
        /// Gets the sent date.
        /// </summary>
        /// <returns>The sent date</returns>
        DateTime SentDate { get; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>The text</returns>
        string Text { get; }

        /// <summary>
        /// Gets the message replied by this one.
        /// </summary>
        /// <returns>The message being replied to</returns>
        IMessage RepliesTo { get; }

        /// <summary>
        /// Gets the joined user.
        /// </summary>
        /// <returns>The joinded user</returns>
        IUser JoinedUser { get; }
    }
}