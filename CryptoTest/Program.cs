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
        private static List<string> PLATFORM_NAME = new List<string>(){ "BINC"};
        static async Task Main(string[] args)
        {
            int spanAttr = int.Parse(ConfigurationManager.AppSettings.Get("time_span"));
            int intervalAttr = int.Parse(ConfigurationManager.AppSettings.Get("number_of_intervals"));
            string symbol;
            DateTime starting_date_time;
            DateTime ending_date_time;
            int duration; //in minutes            

            // Step 1: test for null.
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("The time span for intervals is " + spanAttr + " minute(s).");
                Console.WriteLine("The number of intervals is " + intervalAttr + ", edit 'CryptoTest.exe.config' to change these values.");
                Console.WriteLine("Usage: symbol yy-MM-dd HH:mm:ss duration(in minutes) ");
                Console.WriteLine("Example: BTCUSDT 21-05-05 12:00:00 60");
            }
            else
            {
                try
                {                    
                    // Step 2: print length, and loop over all arguments.
                    symbol = args[0].ToUpper();
                    //Use of Convert.ToDateTime()                    
                    starting_date_time = DateTime.ParseExact(args[1] + " " + args[2], "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    duration = int.Parse(args[3]);
                    ending_date_time = starting_date_time.AddMinutes(duration);

                    Console.WriteLine("The symbol is: " + symbol);
                    Console.WriteLine("Starting date and time is " + starting_date_time.ToString("yy-MM-dd:HH:mm:ss"));
                    Console.WriteLine("Ending date and time is " + ending_date_time.ToString("yy-MM-dd:HH:mm:ss"));
                    Console.WriteLine("Duration is " + duration + " minute(s)");
                                        
                    //CryptoTestHelper cth2 = new CryptoTestBinance();

                    DateTime start_time_interval = starting_date_time;
                    DateTime end_time_interval = start_time_interval.AddMinutes(spanAttr);

                    deleteSerializedObject();

                    while (end_time_interval.CompareTo(ending_date_time) <= 0)
                    {
                        string filename_of_csv = PLATFORM_NAME[0] + "-" + symbol + "-" + starting_date_time.ToString("yyyyMMdd_HHmm") + "_" + ending_date_time.ToString("yyyyMMdd_HHmm");
                        await new CryptoTestBinance().get_aggTrades(symbol, intervalAttr, spanAttr, start_time_interval, end_time_interval, ending_date_time, filename_of_csv);
                        start_time_interval = end_time_interval;
                        end_time_interval = end_time_interval.AddMinutes(spanAttr);
                    }                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Illegal arguments given to program, " + e.Message);
                    Console.WriteLine("\nThe time span for intervals is " + spanAttr + " minute(s).");
                    Console.WriteLine("The number of intervals is " + intervalAttr + ", edit 'CryptoTest.exe.config' to change these values.");
                    Console.WriteLine("Usage: symbol yy-MM-dd HH:mm:ss duration(in minutes) ");
                    Console.WriteLine("Example: BTCUSDT 21-05-05 12:00:00 60");
                }                
            }
        }

        //Delete serialized object that may have been used in previous execution of program to ensure output integrity
        static void deleteSerializedObject()
        {
            var FileName = Directory.GetCurrentDirectory() + "CryptoTestInfo.bin";
            if (File.Exists(FileName))
                File.Delete(FileName);

        }
    }
}
