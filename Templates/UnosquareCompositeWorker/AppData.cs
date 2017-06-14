using Unosquare.Labs.LiteLib;
using Unosquare.Swan.Abstractions;

namespace UnosquareCompositeWorker
{
    internal class AppData : LiteDbContext
    {
        public AppData() : base(SettingsProvider<AppSettings>.Instance.Global.DatabasePath)
        {
        }

        // TODO: Include your DTOs here
        public LiteDbSet<Order> Orders { get; set; }
    }
}
