using Unosquare.Labs.LiteLib;
using Unosquare.Swan.Abstractions;

namespace UnosquareCompositeWorker
{
    internal class AppData : LiteDbContext
    {
        public AppData() : base(SettingsProvider<AppSettings>.Instance.Global.DatabasePath)
        {
        }

        public LiteDbSet<Order> Orders { get; set; }
    }
}
