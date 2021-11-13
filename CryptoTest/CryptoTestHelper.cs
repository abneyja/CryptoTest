using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    public abstract class CryptoTestHelper
    {
        public abstract Task get_spot_Price_of_ETHUSD();
        public abstract Task get_aggTrades(string symbol, int intervalAttr, int spanAttr, DateTime start_time_interval, 
            DateTime end_time_interval, DateTime ending_date_time, string filename_of_csv);


    }
}
