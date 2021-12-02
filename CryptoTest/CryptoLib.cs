using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    // Some helper functions
    public class CryptoLib 
    {
        private static readonly string platformAttr = ConfigurationManager.AppSettings.Get("platform");
        private static readonly int spanAttr = int.Parse(ConfigurationManager.AppSettings.Get("time_span"));
        private static readonly int intervalAttr = int.Parse(ConfigurationManager.AppSettings.Get("number_of_intervals"));

        static CryptoLib() { }
        public static string platform_name
        {
            get => platformAttr.ToString();
        }
        public static string interval_timespan
        {
            get => spanAttr.ToString();
        }
        public static string interval_size
        {
            get => intervalAttr.ToString();
        }
        //Delete serialized object that may have been used in previous execution of program to ensure output integrity
        public static void deleteSerializedObject()
        {
            var FileName = Directory.GetCurrentDirectory() + "CryptoTestInfo.bin";
            if (File.Exists(FileName))
                File.Delete(FileName);
        }

        public static void setSerializedObject(string FileName, List<CryptoTestInfo> CryptoTestInfoList)
        {

            Stream SaveFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, CryptoTestInfoList);
            SaveFileStream.Close();
        }

        public static List<CryptoTestInfo> getSerializedObject(string FileName)
        {
            Stream openFileStream = File.OpenRead(FileName);
            BinaryFormatter deserializer = new BinaryFormatter();
            List<CryptoTestInfo> CryptoTestInfoList = (List<CryptoTestInfo>)deserializer.Deserialize(openFileStream);
            openFileStream.Close();

            return CryptoTestInfoList;
        }

        public static void EnsureDirectoryExists(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
        }
    }
}
