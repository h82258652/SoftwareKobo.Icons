using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;

namespace IonicSymbolGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public enum IonicSymbol");
            builder.AppendLine("{");
            builder.AppendLine("    None = 0x0000,");

            var webClient = new WebClient();
            var html = webClient.DownloadString("http://ionicons.com/");

            var css = webClient.DownloadString("http://ionicons.com/css/ionicons.css?v=2.0.1");
            var cssLines = css.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);

            var icons = document.QuerySelector("#icons");

            HashSet<string> originList = new HashSet<string>();
            foreach (var iconsChild in icons.Children.OfType<IHtmlListItemElement>())
            {
                var attribute = iconsChild.GetAttribute("class");
                originList.Add(attribute);
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var att in originList)
            {
                var css1 = cssLines.FirstOrDefault(temp => temp.StartsWith("." + att + ":before"));
                if (css1 != null)
                {
                    var index1 = css1.IndexOf("\\", StringComparison.OrdinalIgnoreCase);
                    css1 = css1.Substring(index1 + 1);
                    var index2 = css1.IndexOf("\"", StringComparison.OrdinalIgnoreCase);
                    css1 = css1.Substring(0, index2);

                    var arr = att.ToArray();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var c = arr[i];
                        if (c == '-')
                        {
                            arr[i + 1] = char.ToUpper(arr[i + 1]);
                        }
                    }
                    arr[0] = char.ToUpper(arr[0]);

                    var att2 = new string(arr);
                    att2 = att2.Replace("-", "");

                    att2 = att2.Substring(3);

                    dict[att2] = css1;
                }
                else
                {
                    Debugger.Break();
                }
            }

            var dict2 = dict.OrderBy(temp => temp.Value).ToDictionary(temp => temp.Key, temp => temp.Value);

            foreach (var keyValue in dict2)
            {
                var n = keyValue.Key;
                var vv = keyValue.Value;

                builder.AppendLine("    " + n + " = " + "0x" + vv + ",");
            }

            builder.AppendLine("}");
            File.WriteAllText("symbol.txt", builder.ToString());

            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }
}