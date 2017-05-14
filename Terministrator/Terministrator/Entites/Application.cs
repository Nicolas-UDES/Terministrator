#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the applications. Contains all the datas required for an application.
    /// These calls the ApplicationFactory to know which application they represent and let the code call directly said application.
    /// Eg: Send a message to an application we just fetched from the database.
    /// </summary>
    class Application : IApplication
    {
        private IApplication _application;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="commandSymbols">The command symbols.</param>
        /// <param name="userSymbols">The user symbols.</param>
        /// <param name="token">The token.</param>
        public Application(string applicationName, string commandSymbols, string userSymbols, string token)
        {
            ApplicationName = applicationName;
            CommandSymbols = commandSymbols;
            UserSymbols = userSymbols;
            Token = token;
        }

        [Key]
        [MaxLength(128)]
        public string ApplicationName { get; set; }

        public string CommandSymbols { get; set; }

        public string UserSymbols { get; set; }

        public virtual List<Channel> Channels { get; set; }

        public virtual List<User> Users { get; set; }

        public string Token { get; set; }

        /// <summary>
        /// Sets the message destination.
        /// </summary>
        /// <param name="receivedMessage">The received message method.</param>
        public void SetMessageDestination(Action<IMessage> receivedMessage)
        {
            GetApplication().SetMessageDestination(receivedMessage);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            GetApplication().Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            GetApplication().Stop();
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// A task containing the ID of the message sent.
        /// </returns>
        public Task<string> SendMessage(IMessage message)
        {
            return GetApplication().SendMessage(message);
        }

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void EditMessage(IMessage message)
        {
            GetApplication().EditMessage(message);
        }

        /// <summary>
        /// Gets the terministrator.
        /// </summary>
        /// <returns>
        /// Gets the user Terministrator on the application.
        /// </returns>
        public IUser Terministrator => GetApplication().Terministrator;

        /// <summary>
        /// Determines whether this instance can kick the specified user.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///   <c>true</c> if this instance can kick the specified user; otherwise, <c>false</c>.
        /// </returns>
        public bool CanKick(IChannel channel)
        {
            return GetApplication().CanKick(channel);
        }

        /// <summary>
        /// Kicks the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="channel">The channel.</param>
        public void Kick(IUser user, IChannel channel)
        {
            GetApplication().Kick(user, channel);
        }

        /// <summary>
        /// Gets the mods of the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        /// A collection of user which are moderators.
        /// </returns>
        public List<IUser> GetMods(IChannel channel)
        {
            return GetApplication().GetMods(channel);
        }

        /// <summary>
        /// Pings the application.
        /// </summary>
        /// <param name="max">The maximum time a ping can take.</param>
        /// <returns>
        /// The time necessary to ping. Null if no connection.
        /// </returns>
        public TimeSpan? Ping(TimeSpan? max = null)
        {
            return GetApplication().Ping(max);
        }

        /// <summary>
        /// Gets the command symbol.
        /// </summary>
        /// <returns>
        /// The command symbol.
        /// </returns>
        public string GetCommandSymbol()
        {
            return CommandSymbols;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ApplicationName;
        }

        /// <summary>
        /// Gets the application (from the application factory).
        /// </summary>
        /// <returns></returns>
        private IApplication GetApplication()
        {
            return _application ?? (_application = ApplicationFactory.Get(ApplicationName));
        }
    }
}