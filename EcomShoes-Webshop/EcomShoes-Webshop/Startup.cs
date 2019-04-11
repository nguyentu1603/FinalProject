using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EcomShoes_Webshop.Startup))]
namespace EcomShoes_Webshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
