namespace Terministrator.Application.Interface
{
    interface IChannel
    {
        string GetApplicationId();
        IApplication GetApplication();
        string GetFirstName();
        string GetLastName();
        string GetUsername();
        bool IsSolo();
    }
}