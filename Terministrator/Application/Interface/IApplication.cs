#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Terministrator.Application.Interface
{
    interface IApplication
    {
        string Token { get; set; }

        /// <summary>
        /// Gets the command symbol.
        /// </summary>
        /// <returns>The command symbol.</returns>
        string GetCommandSymbol();

        /// <summary>
        /// Gets the user symbol.
        /// </summary>
        /// <returns>The user symbol.</returns>
        string GetUserSymbol();

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <returns>The name of the application.</returns>
        string GetApplicationName();

        /// <summary>
        /// Sets the message destination.
        /// </summary>
        /// <param name="receivedMessage">The received message method.</param>
        void SetMessageDestination(Action<IMessage> receivedMessage);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A task containing the ID of the message sent.</returns>
        Task<string> SendMessage(IMessage message);

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="message">The message.</param>
        void EditMessage(IMessage message);

        /// <summary>
        /// Gets the terministrator.
        /// </summary>
        /// <returns>Gets the user Terministrator on the application.</returns>
        IUser GetTerministrator();

        /// <summary>
        /// Determines whether this instance can kick the specified user.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///   <c>true</c> if this instance can kick the specified user; otherwise, <c>false</c>.
        /// </returns>
        bool CanKick(IChannel channel);

        /// <summary>
        /// Kicks the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="channel">The channel.</param>
        void Kick(IUser user, IChannel channel);

        /// <summary>
        /// Gets the mods of the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>A collection of user which are moderators.</returns>
        List<IUser> GetMods(IChannel channel);

        /// <summary>
        /// Pings the application.
        /// </summary>
        /// <param name="max">The maximum time a ping can take.</param>
        /// <returns>The time necessary to ping. Null if no connection.</returns>
        TimeSpan? Ping(TimeSpan? max = null);

    }
}