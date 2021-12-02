using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

namespace CryptoTest
{
    public class Program
    {
        private static readonly int spanAttr = int.Parse(CryptoLib.interval_timespan);
        private static readonly int intervalAttr = int.Parse(CryptoLib.interval_size);
        private static string PLATFORM_NAME = CryptoLib.platform_name;
        static async Task Main(string[] args)
        {   
            DateTime starting_date_time;
            DateTime ending_date_time;
            int duration; //in minutes            

            // Step 1: test for null.
            if (args == null || args.Length < 1)
            {
                PrintInformation();
            }
            else
            {
                try
                {                    
                    // Step 2: print length, and loop over all arguments.
                    //symbol = args[0].ToUpper();
                    //Use of Convert.ToDateTime()                    
                    starting_date_time = DateTime.ParseExact(args[0] + " " + args[1], "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    duration = int.Parse(args[2]);
                    ending_date_time = starting_date_time.AddMinutes(duration);

                    Console.WriteLine("Starting date and time is " + starting_date_time.ToString("yy-MM-dd:HH:mm:ss"));
                    Console.WriteLine("Ending date and time is " + ending_date_time.ToString("yy-MM-dd:HH:mm:ss"));
                    Console.WriteLine("Duration is " + duration + " minute(s)");
                    Console.WriteLine("The symbol(s) and platform must be provided in the app.config file");

                    //CryptoTestHelper cth2 = new CryptoTestBinance();

                    await new CryptoTestBinance().buildInterval(starting_date_time, ending_date_time);


                    /*while (end_time_interval.CompareTo(ending_date_time) <= 0)
                    {
                        string filename_of_csv = PLATFORM_NAME + "-" + symbol + "-" + starting_date_time.ToString("yyyyMMdd_HHmm") + "_" + ending_date_time.ToString("yyyyMMdd_HHmm");
                        await new CryptoTestBinance().get_aggTrades(symbol, start_time_interval, end_time_interval, ending_date_time, filename_of_csv);
                        start_time_interval = end_time_interval;
                        end_time_interval = end_time_interval.AddMinutes(spanAttr);
                    }        */
                }
                catch (Exception e)
                {
                    Console.WriteLine("Illegal arguments given to program, " + e.Message);
                    PrintInformation();
                }                
            }
        }

        public static void PrintInformation()
        {
            Console.WriteLine("\nThe time span for intervals is " + spanAttr + " minute(s).");
            Console.WriteLine("The number of intervals is " + intervalAttr + ", edit 'CryptoTest.exe.config' to change these values.");
            Console.WriteLine("Usage: yy-MM-dd HH:mm:ss duration(in minutes) ");
            Console.WriteLine("Example: 21-05-05 12:00:00 60");
            Console.WriteLine("The symbol and platform must be provided in the 'CryptoTest.exe.config' file");
        }
    }
}
