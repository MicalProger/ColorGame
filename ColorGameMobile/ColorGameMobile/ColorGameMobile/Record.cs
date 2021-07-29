using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GameColorDesktop
{
    class Record
    {
        static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Records.json");
        public int Attemps { get; set; }
        public TimeSpan Time { get; set; }

        public static List<Record> Records = File.Exists(path) ? 
            JsonConvert.DeserializeObject<List<Record>>(File.ReadAllText(path))
            : new List<Record>();

        public static void SaveRecords() => File.WriteAllText(path, JsonConvert.SerializeObject(Records));
    }
}
