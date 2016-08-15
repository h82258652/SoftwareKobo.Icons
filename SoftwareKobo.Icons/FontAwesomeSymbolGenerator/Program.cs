using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FontAwesomeSymbolGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebClient client = new WebClient();
            var html = client.DownloadString("http://fontawesome.io/cheatsheet/");
            HtmlParser parser = new HtmlParser();
            var document = parser.Parse(html);
            var row = document.QuerySelector("div.container > div.row");
            var divs = row.Children.OfType<IHtmlDivElement>();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public enum FontAwesomeSymbol");
            builder.AppendLine("{");
            builder.AppendLine("None = 0x0000,");

            Dictionary<string, int> dic = new Dictionary<string, int>();

            Dictionary<string, int> dd = new Dictionary<string, int>();

            foreach (var htmlDivElement in divs)
            {
                var name = string.Join("", htmlDivElement.ChildNodes.OfType<IText>().Select(temp => temp.Text)).Trim();
                if (name.StartsWith("fa"))
                {
                    name = name.Substring("fa".Length);
                }
                if (name.Contains("-o-"))
                {
                    name = name.Replace("-o-", "-Outline-");
                }
                else if (name.EndsWith("-o"))
                {
                    name = name.Substring(0, name.Length - "-o".Length);
                    name = name + "-Outline";
                }
                var tempList = name.ToCharArray();
                for (int i = 0; i < tempList.Length; i++)
                {
                    var c = tempList[i];
                    if (c == '-')
                    {
                        tempList[i + 1] = char.ToUpper(tempList[i + 1]);
                    }
                }
                name = new string(tempList);
                name = name.Replace("-", "");

                if (htmlDivElement.TextContent.Contains("(alias)"))
                {
                    var span = htmlDivElement.Children.OfType<IHtmlSpanElement>().Last();
                    var value = span.TextContent;
                    value = value.Replace("[&#x", "0x");
                    value = value.Replace(";]", "");
                    var index = Convert.ToInt32(value, 16);

                    dd.Add(name, index);
                }
                else
                {
                    var span = htmlDivElement.Children.OfType<IHtmlSpanElement>().Last();
                    var value = span.TextContent;
                    value = value.Replace("[&#x", "0x");
                    value = value.Replace(";]", "");
                    var index = Convert.ToInt32(value, 16);

                    dic.Add(name, index);
                }

                //builder.AppendLine(name + " = " + value + ",");
            }

            dd = dd.OrderBy(temp => temp.Value).ToDictionary(temp => temp.Key, temp => temp.Value);

            foreach (var temp in dic.OrderBy(temp => temp.Value))
            {
                var name = temp.Key;
                var value = "0x" + temp.Value.ToString("x");
                builder.AppendLine(name + " = " + value + ",");

                var ttf = dd.Where(temp2 => temp2.Value == temp.Value);
                foreach (var keyValuePair in ttf)
                {
                    builder.AppendLine(keyValuePair.Key + "=" + name + ",");
                }
            }

            builder.AppendLine("}");

            File.WriteAllText(@"symbol.txt", builder.ToString());
            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }
}