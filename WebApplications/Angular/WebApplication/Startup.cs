using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(My.Angular.WebApplication.Startup))]
namespace My.Angular.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
