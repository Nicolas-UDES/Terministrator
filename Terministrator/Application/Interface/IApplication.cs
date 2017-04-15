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
        string GetCommandSymbol();
        string GetUserSymbol();
        string GetApplicationName();
        void SetMessageDestination(Action<IMessage> receivedMessage);
        void Start();
        void Stop();
        Task<string> SendMessage(IMessage message);
        void EditMessage(IMessage message);
        IUser GetTerministrator();
        bool CanKick(IChannel channel);
        void Kick(IUser user, IChannel channel);
        List<IUser> Mods(IChannel channel);
        TimeSpan? Ping(TimeSpan? max = null);
    }
}