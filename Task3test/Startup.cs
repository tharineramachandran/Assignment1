using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Task3test.Startup))]
namespace Task3test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
