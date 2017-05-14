#region Usings

using System.Collections.Generic;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the channels. Contains all the datas required for a channel.
    /// </summary>
    /// <seealso cref="Namable" />
    /// <seealso cref="IChannel" />
    class Channel : Namable, IChannel
    {
        public Channel()
        {
        }

        public Channel(Application application, string idChannelForApplication, bool isSolo, AdSystem adSystem = null)
        {
            ApplicationName = application.ApplicationName;
            Application = application;
            IdForApplication = idChannelForApplication;
            Private = isSolo;
            AdSystem = adSystem;
        }

        public bool Private { get; set; }

        public virtual List<UserToChannel> Users { get; set; }

        public virtual AdSystem AdSystem { get; set; }

        public virtual PointSystem PointSystem { get; set; }

        public virtual List<Privileges> Privileges { get; set; }

        public string ApplicationId => IdForApplication;

        IApplication IChannel.Application => Application;

        bool IChannel.IsSolo => Private;
    }
}