using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZensarProject.Startup))]
namespace ZensarProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
