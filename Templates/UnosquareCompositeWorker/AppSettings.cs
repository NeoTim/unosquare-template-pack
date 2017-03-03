using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.Swan;

namespace UnosquareCompositeWorker
{
    public class AppSettings
    {
        public string ApiUrl { get; set; } = "http://localhost:7000";

        public string DatabasePath { get; set; } = Path.Combine(Runtime.EntryAssemblyDirectory, "database.sql");
    }
}
