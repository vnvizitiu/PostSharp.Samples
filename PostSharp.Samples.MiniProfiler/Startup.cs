using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PostSharp.Samples.MiniProfiler.Startup))]
namespace PostSharp.Samples.MiniProfiler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
