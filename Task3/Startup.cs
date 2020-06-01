using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Task3.StartupOwin))]

namespace Task3
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
