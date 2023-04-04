
[assembly: HostingStartup(typeof(ChatRoom.Areas.Identity.IdentityHostingStartup))]
namespace ChatRoom.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
