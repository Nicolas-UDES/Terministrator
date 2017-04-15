#region Usings

using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    #region Usings

    using TChannel = DiscordChannel;
    using TPrivateChannel = DiscordPrivateChannel;

    #endregion

    class Channel : IChannel
    {
        private readonly Application _application;
        private readonly string _applicationId;
        private readonly string _firstName;
        private readonly bool _isSolo;
        private readonly string _lastName;
        private readonly string _username;

        public Channel(TChannel channel)
        {
            _applicationId = channel.ID;
            _firstName = channel.Name;
            _lastName = channel.Topic;
            _username = channel.Parent.Name;
            _isSolo = false;
            _application = Application.Instance;
        }

        public Channel(TPrivateChannel channel)
        {
            _applicationId = channel.ID;
            _firstName = channel.Recipient.Username;
            _lastName = null;
            _username = channel.Recipient.Email;
            _isSolo = channel.Private;
            _application = Application.Instance;
        }

        public string GetApplicationId() => _applicationId;

        public IApplication GetApplication() => _application;

        public string GetFirstName() => _firstName;

        public string GetLastName() => _lastName;

        public string GetUsername() => _username;

        public bool IsSolo() => _isSolo;
    }
}