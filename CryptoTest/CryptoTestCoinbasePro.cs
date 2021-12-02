using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coinbase.Pro;

namespace CryptoTest
{
    class CryptoTestCoinbasePro : IntervalBuilderSvc

    {
        private CoinbaseProClient client;

        public CryptoTestCoinbasePro()
        {
            //No authentication
            //  - Useful only for Data Endpoints that don't require authentication.
            client = new CoinbaseProClient();
        }

        public override async Task get_spot_Price_of_ETHUSD()
        {
            /*var spot = await client.MarketData
            if (!spot.HasError())
            {
                System.Diagnostics.Debug.WriteLine("Coinbase CallResult Symbol: " + spot.Data.Base + " Currency: " + spot.Data.Currency + " Price: " + spot.Data.Amount);
            }
            else
                System.Diagnostics.Debug.WriteLine("Call error coinbase: " + spot.Errors.ToString());
            */
        }

        public override async Task get_aggTrades(string symbol, DateTime start_time_interval, DateTime end_time_interval,
            DateTime ending_date_time, string filename_of_csv)
        {

        }
    }
}
