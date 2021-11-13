using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace CryptoTest
{
    [Serializable()]
    class CryptoTestInfo
    {
        static readonly int spanAttr = int.Parse(ConfigurationManager.AppSettings.Get("time_span"));
        static readonly int intervalAttr = int.Parse(ConfigurationManager.AppSettings.Get("number_of_intervals"));

        static CryptoTestInfo() { }
        public CryptoTestInfo(string starting_date, string duration, string average_price, string trade_count, string moving_average)
        {
            this.starting_date = starting_date;
            this.duration = duration;
            this.average_price = average_price;
            this.trade_count = trade_count;
            this.moving_average = moving_average;
        }

        public string starting_date { get; set; }
        public string duration { get; set; }
        public string average_price { get; set; }
        public string trade_count { get; set; }
        public string moving_average { get; set; }
    }
}
