using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClinicaPilotagemWeb.Startup))]
namespace ClinicaPilotagemWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
