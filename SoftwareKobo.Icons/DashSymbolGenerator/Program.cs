using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DashSymbolGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public enum DashSymbol");
            builder.AppendLine("{");
            builder.AppendLine("None = 0x0000,");

            WebClient client = new WebClient();
            var html = client.DownloadString("https://developer.wordpress.org/resource/dashicons/");

            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);

            var list = document.QuerySelector("#iconlist");

            var divs = list.Children.OfType<IHtmlDivElement>();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var htmlDivElement in divs)
            {
                var name = htmlDivElement.GetAttribute("class");
                if (name.StartsWith("dashicons "))
                {
                    name = name.Substring("dashicons ".Length);
                }
                if (name.StartsWith("dashicons-"))
                {
                    name = name.Substring("dashicons-".Length);
                }
                var arr = name.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    var c = arr[i];
                    if (c == '-')
                    {
                        arr[i + 1] = Char.ToUpper(arr[i + 1]);
                    }
                }
                arr[0] = char.ToUpper(arr[0]);
                name = new string(arr);
                name = name.Replace("-", "");

                var value = htmlDivElement.GetAttribute("alt");

                dic.Add(name, value);
            }

            dic = dic.OrderBy(temp => temp.Value).ToDictionary(temp => temp.Key, temp => temp.Value);

            Dictionary<string, string> other = new Dictionary<string, string>();

            Regex regex = new Regex(@"<!--(<div .*?></div>) Duplicate -->");
            var matches = regex.Matches(html);
            foreach (Match match in matches)
            {
                var divSource = match.Groups[1].Value;
                HtmlParser parser2 = new HtmlParser();
                var d = parser2.Parse(divSource);
                var htmlDivElement = d.All.OfType<IHtmlDivElement>().First();

                var name = htmlDivElement.GetAttribute("class");
                if (name.StartsWith("dashicons "))
                {
                    name = name.Substring("dashicons ".Length);
                }
                if (name.StartsWith("dashicons-"))
                {
                    name = name.Substring("dashicons-".Length);
                }
                var arr = name.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    var c = arr[i];
                    if (c == '-')
                    {
                        arr[i + 1] = Char.ToUpper(arr[i + 1]);
                    }
                }
                arr[0] = char.ToUpper(arr[0]);
                name = new string(arr);
                name = name.Replace("-", "");

                var value = htmlDivElement.GetAttribute("alt");

                other.Add(name, value);
            }

            foreach (var keyValue in dic)
            {
                var name = keyValue.Key;
                var value = keyValue.Value;
                builder.AppendLine(name + " = " + "0x" + value + ",");

                var lll = other.Where(temp2 => temp2.Value == value);
                foreach (var keyValuePair in lll)
                {
                    builder.AppendLine(keyValuePair.Key + "=" + name + ",");
                }
            }

            builder.AppendLine("}");
            File.WriteAllText("symbol.txt", builder.ToString());

            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }
}