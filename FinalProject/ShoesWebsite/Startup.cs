using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShoesWebsite.Startup))]
namespace ShoesWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
