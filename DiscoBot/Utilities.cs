using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DiscoBot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Random rnd = new Random();
        public static bool set;
        public static bool exists;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
        }

        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key], parameter);
            }
            return "";
        }

        public static string GetFormattedAlert(string key, object parameter)
        {
            return GetFormattedAlert(key, new object[] {  parameter });
        }

        public static void ValidateList(string key, string quote)
        {
            List<string> existing;
            set = false;
            exists = false;

            if (!DataStorage.pairs.TryGetValue(key, out existing))
            {
                existing = new List<string>();
                DataStorage.pairs[key] = existing;
            }
            if (existing.Count <  Int32.Parse(Program.numberOfMessages))
            {
                foreach (string value in existing)
                {
                    if (quote.ToUpper() == value.ToUpper())
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    set = true;
                    existing.Add(quote);
                    DataStorage.SaveData();
                }
            }
        }

        public static string ReturnRandomString(string key)
        {
            set = false;
            List<string> existing;

            if (DataStorage.pairs.ContainsKey(key))
            {
                set = true;
                existing = DataStorage.pairs[key];
                int r = rnd.Next(existing.Count);
                return existing[r];
            }
            else
            {
                set = false;
                return "";
            }
        }

        public static List<string> ReturnStringList(string key)
        {
            set = false;
            List<string> existing;

            if (DataStorage.pairs.ContainsKey(key))
            {
                set = true;
                existing = DataStorage.pairs[key];
                return existing;
            }
            else
            {
                set = false;
                return null;
            }
        }

        public static void DeleteString(string key, int a)
        {
            set = false;
            List<string> existing;

            if (DataStorage.pairs.ContainsKey(key) && a <= DataStorage.pairs[key].Count)
            {
                set = true;
                existing = DataStorage.pairs[key];
                existing.RemoveAt(a-1);
                DataStorage.pairs[key] = existing;
                DataStorage.SaveData();
            }
            else
            {
                set = false;
            }
        }
    }
}
