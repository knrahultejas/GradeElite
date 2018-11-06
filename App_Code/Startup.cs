using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GradeElite.Startup))]
namespace GradeElite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
