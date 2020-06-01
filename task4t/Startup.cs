using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(task4t.Startup))]
namespace task4t
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
