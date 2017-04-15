#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Application : IApplication
    {
        private IApplication _application;

        [Key]
        [MaxLength(128)]
        public string ApplicationName { get; set; }

        public string CommandSymbols { get; set; }

        public string UserSymbols { get; set; }

        public string Token { get; set; }

        public virtual List<Channel> Channels { get; set; }

        public virtual List<User> Users { get; set; }

        public Application()
        {
            
        }

        public Application(string applicationName, string commandSymbols, string userSymbols, string token)
        {
            ApplicationName = applicationName;
            CommandSymbols = commandSymbols;
            UserSymbols = userSymbols;
            Token = token;
        }

        public override string ToString()
        {
            return ApplicationName;
        }

        public string GetApplicationName()
        {
            return ApplicationName;
        }

        private IApplication GetApplication()
        {
            return _application ?? (_application = ApplicationFactory.Get(ApplicationName));
        }

        public void SetMessageDestination(Action<IMessage> receivedMessage)
        {
            GetApplication().SetMessageDestination(receivedMessage);
        }

        public void Start()
        {
            GetApplication().Start();
        }

        public void Stop()
        {
            GetApplication().Stop();
        }

        public Task<string> SendMessage(IMessage message)
        {
            return GetApplication().SendMessage(message);
        }

        public void EditMessage(IMessage message)
        {
            GetApplication().EditMessage(message);
        }

        public IUser GetTerministrator()
        {
            return GetApplication().GetTerministrator();
        }

        public bool CanKick(IChannel channel)
        {
            return GetApplication().CanKick(channel);
        }

        public void Kick(IUser user, IChannel channel)
        {
            GetApplication().Kick(user, channel);
        }

        public List<IUser> Mods(IChannel channel)
        {
            return GetApplication().Mods(channel);
        }

        public TimeSpan? Ping(TimeSpan? max = null)
        {
            return GetApplication().Ping(max);
        }

        public string GetCommandSymbol()
        {
            return CommandSymbols;
        }

        public string GetUserSymbol()
        {
            return UserSymbols;
        }
    }
}