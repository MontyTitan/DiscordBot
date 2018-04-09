using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DiscoBot
{
    class DataStorage
    {
        //private static Dictionary<string, string> pairs = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> pairs = new Dictionary<string, List<string>>();

        /*public static void AddPairToStorage(string key, string value)
        {
            pairs.Add(key, value);
            SaveData();
        }*/

        public static void AddPairToStorage(string key, List<string> values)
        {
            pairs.Add(key, values);
            SaveData();
        }

        public static int GetPairsCount()
        {
            return pairs.Count;
        }

        static DataStorage()
        {
            //Load Data
            if (!ValidateStorageFile("DataStorage.json")) return;
            string json = File.ReadAllText("DataStorage.json");
            //pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            pairs = JsonConvert.DeserializeObject<Dictionary<string,List<string>>>(json);
        }

        public static void SaveData()
        {
            //Save Data
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText("DataStorage.json", json);
        }

        private static bool ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }
    }
}
