#region Usings

using System;

#endregion

namespace Terministrator.Application.Interface
{
    interface IMessage
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>The application identifier</returns>
        string GetApplicationId();

        /// <summary>
        /// Gets the author of the message.
        /// </summary>
        /// <returns>The author</returns>
        IUser GetFrom();

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <returns>The channel</returns>
        IChannel GetChannel();

        /// <summary>
        /// Gets the sent date.
        /// </summary>
        /// <returns>The sent date</returns>
        DateTime GetSentDate();

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>The text</returns>
        string GetText();

        /// <summary>
        /// Gets the message replied by this one.
        /// </summary>
        /// <returns>The message being replied to</returns>
        IMessage GetRepliesTo();

        /// <summary>
        /// Gets the joined user.
        /// </summary>
        /// <returns>The joinded user</returns>
        IUser GetJoinedUser();
    }
}