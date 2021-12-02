using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CryptoTest
{
    class CsvTestHelper
    {
        public CsvTestHelper(string file_name, CryptoTestInfo cryptoInfoObject)
        {
            var path = Directory.GetCurrentDirectory() + "\\csv\\" + file_name + ".csv";
            var record = new List<CryptoTestInfo>
            {
                cryptoInfoObject
            };
            try
            {
                CryptoLib.EnsureDirectoryExists(Directory.GetCurrentDirectory() + "\\csv\\");
                if (File.Exists(path))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        // Don't write the header again.
                        HasHeaderRecord = false,
                    };
                    using (var writer = File.AppendText(path))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        csv.WriteRecords(record);
                    }
                }
                else
                {
                    using (var writer = new StreamWriter(path))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(record);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing CSV file, " + e.Message);
            }

        }

    }
}
