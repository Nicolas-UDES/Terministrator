#region Usings

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.TelegramApplication
{
    #region Usings

    using TChannel = Chat;

    #endregion

    class Channel : IChannel
    {
        private readonly Application _application;
        private readonly long _applicationId;
        private readonly string _firstName;
        private readonly bool _isSolo;
        private readonly string _lastName;
        private readonly string _username;

        public Channel(TChannel channel)
        {
            _applicationId = channel.Id;
            _firstName = channel.FirstName;
            _lastName = channel.LastName;
            _username = channel.Title;
            _isSolo = channel.Type == ChatType.Private;
            _application = Application.Instance;
        }

        public string GetApplicationId() => _applicationId.ToString();

        public IApplication GetApplication() => _application;

        public string GetFirstName() => _firstName;

        public string GetLastName() => _lastName;

        public bool IsSolo() => _isSolo;
        public string GetUsername() => _username;

        public long GetOriginalApplicationId() => _applicationId;
    }
}