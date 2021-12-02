using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    public abstract class IntervalBuilderSvc
    {
        protected static readonly string platformAttr = CryptoLib.platform_name;
        protected static readonly int spanAttr = int.Parse(CryptoLib.interval_size);
        protected static readonly int intervalAttr = int.Parse(CryptoLib.interval_timespan);

        public async Task buildInterval(DateTime starting_date_time, DateTime ending_date_time)
        {      
            var section = (SymbolSection)ConfigurationManager.GetSection("symbol");
            foreach (var s in section.SymbolList)
            {
                DateTime start_time_interval = starting_date_time;
                DateTime end_time_interval = start_time_interval.AddMinutes(spanAttr);
                CryptoLib.deleteSerializedObject();

                while (end_time_interval.CompareTo(ending_date_time) <= 0)
                {
                    string filename_of_csv = platformAttr + "-" + s + "-" + starting_date_time.ToString("yyyyMMdd_HHmm") + "_" + ending_date_time.ToString("yyyyMMdd_HHmm");
                    await new CryptoTestBinance().get_aggTrades(s, start_time_interval, end_time_interval, ending_date_time, filename_of_csv);
                    start_time_interval = end_time_interval;
                    end_time_interval = end_time_interval.AddMinutes(spanAttr);
                }          
            }

        }

        public abstract Task get_spot_Price_of_ETHUSD();
        public abstract Task get_aggTrades(string symbol, DateTime start_time_interval, DateTime end_time_interval,
            DateTime ending_date_time, string filename_of_csv);
    }

    public class SymbolSection : ConfigurationSection
    {
        [ConfigurationProperty("SymbolList")]
        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        public CommaDelimitedStringCollection SymbolList
        {
            get { return (CommaDelimitedStringCollection)base["SymbolList"]; }
        }
    }
}
