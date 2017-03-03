using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.LiteLibWebApi;
using Unosquare.Swan;
using Unosquare.Swan.Abstractions;

namespace UnosquareCompositeWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Runtime.WriteWelcomeBanner();
            $"API URL: {SettingsProvider<AppSettings>.Instance.Global.ApiUrl}".Info();

            // Setup the internal database
            var dbInstance = new AppData();

            // Start the web server
            using (var server = new WebServer(SettingsProvider<AppSettings>.Instance.Global.ApiUrl))
            {
                // Register LiteLibWebApi module
                server.RegisterModule(new LiteLibModule<AppData>(dbInstance));
                server.RunAsync();

                "Press any key to close...".Info();
                Terminal.ReadKey(true, true);
            }
        }
    }
}