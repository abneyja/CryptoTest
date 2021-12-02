using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coinbase;

namespace CryptoTest
{
    class CryptoTestCoinbase : IntervalBuilderSvc
    {
        private CoinbaseClient client;

        public CryptoTestCoinbase()
        {
            //No authentication
            //  - Useful only for Data Endpoints that don't require authentication.
            client = new CoinbaseClient();
        }

        public override async Task get_spot_Price_of_ETHUSD()
        {
            var spot = await client.Data.GetSpotPriceAsync("ETH-USD");
            if (!spot.HasError())
            {
                System.Diagnostics.Debug.WriteLine("Coinbase CallResult Symbol: " + spot.Data.Base + " Currency: " + spot.Data.Currency + " Price: " + spot.Data.Amount);
            }
            else
                System.Diagnostics.Debug.WriteLine("Call error coinbase: " + spot.Errors.ToString());

        }

        public override async Task get_aggTrades(string symbol, DateTime start_time_interval, DateTime end_time_interval, 
            DateTime ending_date_time, string filename_of_csv)
        {
             
        }
    }
}
