using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace PAYFORT
{
    static class JSon
    {

        internal static string[] XMLToArray(string xml_string)
        {
            string json = XmlToJSON(xml_string);

            return JsonToArray(json);
        }
        internal static string JsonToXml(string json)
        {
            string _xml = "";
            json = json.Replace("{", "").Replace("}", "");
            string[] tmp = json.Split(',');
            foreach (string s in tmp)
            {
                string[] ss = s.Replace("https:", "https#").Split(':');
                string key = ss[0].Replace("\"", "");
                string value = ss[1].Replace("\"", "");
                _xml += "<" + key + ">" + value + "</" + key + ">";

            }
            return "<parameters>" + _xml + "</parameters>";

        }
        internal static string[] JsonToArray(string json)
        {
            json = json.Replace("{", "").Replace("}", "");
            json = json.Replace("https:", "https#");
            json = json.Replace("http:", "http#");
            json = json.Replace("localhost:", "localhost#");
            string[] tmp = json.Split(',');
            for(int i=0;i<tmp.Length;i++)
            {
                tmp[i] = tmp[i].Replace("\"", "").Replace(":", "=");
                tmp[i] = tmp[i].Replace("https#", "https:");
                tmp[i] = tmp[i].Replace("http#", "http:");
                tmp[i] = tmp[i].Replace("localhost#", "localhost:");
            }

            return tmp;

        }

        internal static string XmlToJSON(string xml)
        {
            var doc = XDocument.Parse(xml);
           
            var rows = from row in doc.Elements()   select row;

            string[] result = rows.SelectMany(o => o.Elements()).Select(o => (o.Name + "##" + o.Value)).ToArray();

            string json = "";
            foreach (string s in result)
            {
                string[] tmp = s.Split(new string[] { "##" },StringSplitOptions.None);
                string _value = tmp[1];
                string _name = tmp[0];
                if (_value.Length > 0 && _value != "NONE")
                {
                    string comm = ",";
                    if (json == "") { comm = ""; } else { comm = ","; }
                    json = json + comm + "\"" + _name + "\":\"" + _value + "\"";
                }

            }
            return "{" + json + "}";

        }
        internal static ArrayList GetSubStrings(string input, string start, string end)
        {
            ArrayList res = new ArrayList();
            Regex r = new Regex(Regex.Escape(start) + "(.*?)" + Regex.Escape(end));
            MatchCollection matches = r.Matches(input);
            foreach (Match match in matches)
            {
                if(match.Groups[1].Value.ToString().IndexOf("/")==-1)
                res.Add(match.Groups[1].Value.ToString());
            }
            return res; //string.Join("", res.ToArray());

        }
    }
}
