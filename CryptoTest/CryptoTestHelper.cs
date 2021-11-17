using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    public abstract class CryptoTestHelper
    {
        protected static readonly int spanAttr = int.Parse(ConfigurationManager.AppSettings.Get("time_span"));
        protected static readonly int intervalAttr = int.Parse(ConfigurationManager.AppSettings.Get("number_of_intervals"));
        public abstract Task get_spot_Price_of_ETHUSD();
        public abstract Task get_aggTrades(string symbol, DateTime start_time_interval, DateTime end_time_interval,
            DateTime ending_date_time, string filename_of_csv);


    }
}
