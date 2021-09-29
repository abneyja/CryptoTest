using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            CryptoTestHelper cth = new CryptoTestCoinbase();
            CryptoTestHelper cth2 = new CryptoTestBinance();
            await cth.get_spot_Price_of_ETHUSD();
            await cth2.get_spot_Price_of_ETHUSD();
        }
    }
}
