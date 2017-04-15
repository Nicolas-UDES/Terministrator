#region Usings

using System;

#endregion

namespace Terministrator.Application.Interface
{
    interface IMessage
    {
        string GetApplicationId();
        IUser GetFrom();
        IChannel GetChannel();
        DateTime GetSentDate();
        string GetText();
        IMessage GetRepliesTo();
        IUser GetJoinedUser();
    }
}