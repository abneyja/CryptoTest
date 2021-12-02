using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CryptoTest
{
    public class CryptoTestBinance : IntervalBuilderSvc
    {
        private static int LIMIT = 1000;        
        
        private BinanceClient client;
        private double in_minutes;
        private int trade_count = 0;
        private decimal average_price = 0.0M;
        private decimal price = 0.0M;

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

        public override async Task get_aggTrades(string symbol, DateTime start_time_interval, DateTime end_time_interval,
            DateTime ending_date_time, string filename_of_csv)
        {            
            DateTime new_starting_date_time = start_time_interval;
            DateTime new_ending_date_time = new_starting_date_time.AddSeconds(15);
            in_minutes = (end_time_interval.Subtract(start_time_interval)).TotalMinutes;

            for(int i = 0; i < in_minutes * 4; i++)
            {
                var callResult = await client.Spot.Market.GetAggregatedTradeHistoryAsync(symbol, null, new_starting_date_time, new_ending_date_time, LIMIT);

                new_starting_date_time = new_starting_date_time.AddSeconds(15);
                new_ending_date_time = new_starting_date_time.AddSeconds(15);

                if (!callResult.Success)
                {
                    // Call failed, check callResult.Error for more info
                    System.Diagnostics.Debug.WriteLine("Call error: " + callResult.Error.ToString());
                    Console.WriteLine("Call error: " + callResult.Error.ToString());
                }
                else
                {
                    var callResultList = callResult.Data.ToList();
                    for (int j = 0; j < callResultList.Count; j++)
                    {   
                        Console.WriteLine("Aggregate ID: " + callResultList[j].LastTradeId);
                        Console.WriteLine("Price: " + callResultList[j].Price);
                        Console.WriteLine("Quantity: " + callResultList[j].Quantity);
                        Console.WriteLine("Trade time: " + callResultList[j].TradeTime + "\n");
                        price += callResultList[j].Price;
                    }                    
                    trade_count += callResultList.Count;
                    average_price = Decimal.Round(price / trade_count, 2);
                }
            }

            try { 
                var FileName = Directory.GetCurrentDirectory() + "CryptoTestInfo.bin";

                if (File.Exists(FileName))
                {
                    Console.WriteLine("Reading saved file");

                    List<CryptoTestInfo> CryptoTestInfoList = CryptoLib.getSerializedObject(FileName);

                    var moving_average = average_price;

                    //The first object's average trade price is considered multiple times when calculating moving average
                    if (intervalAttr - CryptoTestInfoList.Count >= 0)
                    {
                        for (int i = CryptoTestInfoList.Count - 1; i >= 0; i--)
                        {
                            if (i == 0)
                            {
                                moving_average += (intervalAttr - CryptoTestInfoList.Count) * decimal.Parse(CryptoTestInfoList[i].average_price);
                            }
                            else
                                moving_average += decimal.Parse(CryptoTestInfoList[i].average_price);
                        }

                        moving_average = Decimal.Round(moving_average / intervalAttr, 2);
                        CryptoTestInfoList.Add(new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"), average_price.ToString(),
                            trade_count.ToString(), moving_average.ToString()));

                        new CsvTestHelper(filename_of_csv, new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"),
                            average_price.ToString(), trade_count.ToString(), moving_average.ToString()));

                        CryptoLib.setSerializedObject(FileName, CryptoTestInfoList);
                    }
                    else
                    {
                        //The last [interval size] of elements average trade price are considered when calculating moving average
                        for (int i = CryptoTestInfoList.Count - (intervalAttr - 1); i < CryptoTestInfoList.Count; i++)
                        {
                            moving_average += decimal.Parse(CryptoTestInfoList[i].average_price);
                        }

                        moving_average = Decimal.Round(moving_average / intervalAttr, 2);
                        CryptoTestInfoList.Add(new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"), average_price.ToString(),
                            trade_count.ToString(), moving_average.ToString()));

                        new CsvTestHelper(filename_of_csv, new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"), 
                            average_price.ToString(), trade_count.ToString(), moving_average.ToString()));

                        CryptoLib.setSerializedObject(FileName, CryptoTestInfoList);
                    }
                }
                else
                {
                    //first object in intervals object uses it's own average trade prices to calculate moving average
                    List<CryptoTestInfo> CryptoTestInfoList = new List<CryptoTestInfo>();
                    
                    var moving_average = Decimal.Round((average_price * intervalAttr) / intervalAttr, 2);

                    CryptoTestInfoList.Add(new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"), average_price.ToString(),
                        trade_count.ToString(), moving_average.ToString()));

                    new CsvTestHelper(filename_of_csv, new CryptoTestInfo(start_time_interval.ToString("yy-MM-dd:HH:mm:ss"),
                        average_price.ToString(), trade_count.ToString(), moving_average.ToString()));

                    CryptoLib.setSerializedObject(FileName, CryptoTestInfoList);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error processing interval objects, " + e.Message);
            }

            //Console.WriteLine("Total trades: " + trade_count);
            //Console.WriteLine("Average price: $" + average_price);
            //Console.WriteLine("CSV line written as: " + start_time_interval.ToString("yy-MM-dd:HH:mm:ss") + "," + in_minutes + "," + average_price + "," + trade_count);
            
        }
    }
}
