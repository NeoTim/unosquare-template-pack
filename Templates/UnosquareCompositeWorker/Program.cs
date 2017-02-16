using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.Swan;
using Unosquare.Swan.Abstractions;

namespace UnosquareCompositeWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            $"API URL: {SettingsProvider<AppSettings>.Instance.Global.ApiUrl}".Info();
        }
    }
}