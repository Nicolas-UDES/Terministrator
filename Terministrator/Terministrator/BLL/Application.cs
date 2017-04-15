#region Usings

using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class Application
    {
        public static Entites.Application GetOrCreate(IApplication iApplication)
        {
            return Get(iApplication) ?? Create(iApplication);
        }

        public static Entites.Application UpdateOrCreate(IApplication iApplication)
        {
            Entites.Application application = Get(iApplication);
            return application == null ? Create(iApplication) : Update(iApplication, application);
        }

        public static Entites.Application Get(IApplication iApplication)
        {
            return DAL.Application.Get(iApplication.GetApplicationName());
        }

        public static Entites.Application Create(IApplication iApplication)
        {
            return
                DAL.Application.Create(new Entites.Application(iApplication.GetApplicationName(),
                    iApplication.GetCommandSymbol(), iApplication.GetUserSymbol(), iApplication.Token));
        }

        public static Entites.Application Update(IApplication iApplication, Entites.Application application)
        {
            application.CommandSymbols = iApplication.GetCommandSymbol();
            application.UserSymbols = iApplication.GetUserSymbol();
            application.Token = iApplication.Token;
            return DAL.Application.Update(application);
        }
    }
}