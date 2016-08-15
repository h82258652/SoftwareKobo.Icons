using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MaterialSymbolGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebClient client = new WebClient();
            var json = client.DownloadString("https://design.google.com/icons/data/grid.json");

            //var json = File.ReadAllText("data.json");
            var root = JsonConvert.DeserializeObject<Root>(json);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public enum MaterialSymbol");
            builder.AppendLine("{");
            builder.AppendLine("None = 0x0000,");

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var iconInfo in root.icons)
            {
                var name = iconInfo.ligature;
                var arr = name.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    var c = arr[i];
                    if (c == '_')
                    {
                        arr[i + 1] = char.ToUpper(arr[i + 1]);
                    }
                }
                arr[0] = char.ToUpper(arr[0]);
                name = new string(arr);
                name = name.Replace("_", "");

                name = name.Replace("FormatTextdirectionLToR", "FormatTextdirectionLeftToRight");
                name = name.Replace("FormatTextdirectionRToL", "FormatTextdirectionRightToLeft");

                dict.Add(name, iconInfo.codepoint);
            }
            dict = dict.OrderBy(temp => temp.Value).ToDictionary(temp => temp.Key, temp => temp.Value);

            foreach (var k in dict)
            {
                var name = k.Key;
                var value = k.Value;
                builder.AppendLine(name + " = " + "0x" + value + ",");
            }

            builder.AppendLine("}");

            File.WriteAllText("symbol.txt", builder.ToString());
            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }

    public class Root
    {
        public IconInfo[] icons
        {
            get; set;
        }
    }

    public class IconInfo
    {
        public string ligature
        {
            get; set;
        }

        public string codepoint
        {
            get; set;
        }
    }
}