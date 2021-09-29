using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net;

namespace CryptoTest
{
    public class CryptoTestBinance : CryptoTestHelper
    {
        private BinanceClient client;
        public CryptoTestBinance()
        {
            //No authentication
            //  - Useful only for Data Endpoints that don't require authentication.
            client = new BinanceClient();
        }
        public override async Task get_spot_Price_of_ETHUSD()
        {
            var callResult = await client.Spot.Market.GetPricesAsync();
            // Make sure to check if the call was successful
            if (!callResult.Success)
            {
                // Call failed, check callResult.Error for more info
                System.Diagnostics.Debug.WriteLine("Call error: " + callResult.Error.ToString());
            }
            else
            {
                var callResultDict = callResult.Data.ToDictionary(symbol => symbol.Symbol.ToString(), price => price.Price.ToString());

                if (callResultDict.ContainsKey("ETHUSDC"))
                {
                    var value = callResultDict["ETHUSDC"];
                    System.Diagnostics.Debug.WriteLine("Bianance CallResult Symbol: ETH Currency: USD Price: " + value);
                }               

            }
        }
    }
}
