namespace Terministrator.Application.Interface
{
    interface IUser
    {
        string GetApplicationId();
        string GetFirstName();
        string GetLastName();
        string GetUsername();
        IApplication GetApplication();
    }
}