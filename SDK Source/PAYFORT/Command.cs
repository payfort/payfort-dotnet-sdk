using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml;

namespace PAYFORT
{
    public class Command
    {
        public enum IntegrationTypes { Redirect,Host_to_Host}
        private static  string ConvertToJSON(string[] record)
        {
            
            string json = "";
            foreach (string s in record)
            {
                string[] tmp = s.Split('=');
                string _value = tmp[1];
                string _name = tmp[0];
                if (_value.Length > 0 && _value != "NONE")
                { string comm = ",";
                    if (json == "") { comm = ""; } else { comm = ","; }
                    json = json + comm + "\""+_name+ "\":\"" + _value + "\"";
                }
                
            }
            return "{" + json +"}";
        }
        private static string[] ConvertFromJSON(string txt)
        {







            ArrayList res = new ArrayList();
            txt = txt.Replace("{", "").Replace("}", "");
            txt = txt.Replace("https:", "https#");
            string[] tmp = txt.Split(new string[] { "," }, StringSplitOptions.None);
            foreach (string x in tmp)
            {
                try
                {
                    string[] d = x.Split(':');
                    string name = d[0].Replace("\"", "").Replace(" ", "");
                    string value = d[1].Replace("\"", "");
                    if (name == "3ds_url")
                    {
                        value = value.Replace("https#", "https:");
                    }
                    res.Add(name + "=" + value);
                }
                catch { }


            }
            return res.ToArray(typeof(string)) as string[];
        }
        private static Dictionary<string, string> ConvertFromJSON_dic(string txt)
        {




            Dictionary<string, string> dic = new Dictionary<string, string>();


           
            txt = txt.Replace("{", "").Replace("}", "");
            txt = txt.Replace("https:", "https#");
            string[] tmp = txt.Split(new string[] { "," }, StringSplitOptions.None);
            foreach (string x in tmp)
            {
                try
                {
                    string[] d = x.Split(':');
                    string name = d[0].Replace("\"", "").Replace(" ", "");
                    string value = d[1].Replace("\"", "");
                    if (name == "3ds_url")
                    {
                        value = value.Replace("https#", "https:");
                    }
                    dic.Add(name , value);
                }
                catch { }


            }
            return dic;
        }
        private static string send_request(string json)
        {

            if(json.IndexOf("signature")==-1)
            {
                string signature ="\"signature\":\""+Security.GenerateSignature(json)+"\"";
                json = json.Replace("{","").Replace("}", "");
                json= "{"+ json + "," + signature + "}";
            }


            string str_returned = "";
            string api_url = GetAPIURL(IntegrationTypes.Host_to_Host, PayFortConfig.SandBoxMode);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(api_url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
               
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                str_returned = result;
            }
            return str_returned;
        }
        public static string SendRequestToPayFORT(string json)
        {
            if(json.IndexOf("access_code")==-1)
            {
                json = json.Replace("}", "");
                json += "\"access_code\"=\""+PayFortConfig.access_code+"\"," +   "}";

            }
            if (json.IndexOf("merchant_identifier") == -1)
            {
                json = json.Replace("}", "");
                json += "\"merchant_identifier\"=\"" + PayFortConfig.merchant_identifier + "\"," + "}";

            }
            if (json.IndexOf("language") == -1)
            {
                json = json.Replace("}", "");
                json += "\"language\"=\"" + PayFortConfig.language + "\"," + "}";
            }


            string result = send_request(json);
            
           
            return result;


        }
        public static string[]  SendRequestToPayFORT(string[] _record)
        {

            _record = PayFortConfig.AddConfigToParameters(_record);
            string json= ConvertToJSON(_record);
            string result = send_request(json);
            return ConvertFromJSON(result);


        }
        public static ArrayList SendRequestToPayFORT(ArrayList _record)
        {
            _record = PayFortConfig.AddConfigToParameters(_record);
            string json = ConvertToJSON(_record.ToArray(typeof(string)) as string[]);
            string result = send_request(json);
            ArrayList myArrayList = new ArrayList();
            myArrayList.AddRange(ConvertFromJSON(result));
            return myArrayList;


        }
        public static Dictionary<string, string>  SendRequestToPayFORT(Dictionary<string, string> dictionary)
        {
            ArrayList tmp = new ArrayList();
            foreach (var item in dictionary)
            {
                tmp.Add(item.Key.ToString() + "=" + item.Value.ToString());
            }
            string[] FORTRequestParameters = tmp.ToArray(typeof(string)) as string[];
            FORTRequestParameters = PayFortConfig.AddConfigToParameters(FORTRequestParameters);



            string json = ConvertToJSON(FORTRequestParameters);
            string result = send_request(json);
           
          
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = ConvertFromJSON_dic(result);

           return dic; 


           


        }
        public static string[] GetPAYFORTResponse(NameValueCollection request_queryString_keys)
        {
            ArrayList pars = new ArrayList();
            foreach (String key in request_queryString_keys)
            {
                //if (key != "signature")
               // {
                    pars.Add(key + "=" + request_queryString_keys[key]);
                //}
            }
            string[] parameters = pars.ToArray(typeof(string)) as string[];
            return parameters;
        }
        public static string GetAPIURL( IntegrationTypes _intType, Boolean SandBox = false)
        {

            if (_intType == IntegrationTypes.Redirect)
            {
                if (SandBox == true)
                {
                    return "https://sbcheckout.payfort.com/FortAPI/paymentPage";
                }
                else
                {
                    return "https://checkout.payfort.com/FortAPI/paymentPage";
                }



            }
            else {
                    if (SandBox == true)
                    {
                        return "https://sbpaymentservices.payfort.com/FortAPI/paymentApi";
                    }
                    else
                    {
                        return "https://paymentservices.payfort.com/FortAPI/paymentApi";
                    }
            }
        }
        internal static string get_value(string[] _record, string key)
        {

            foreach (string s in _record)
            {
                if (s.IndexOf(key) != -1)
                {
                    int ind = s.IndexOf('=');
                    string str = s.Substring(ind + 1);
                    return str;
                }
            }
            return "";

        }
        public static string GetParameterValue(ArrayList _record,string key)
        {
            return get_value(_record.ToArray(typeof(string)) as string[], key);
        }
        public static string GetParameterValue(string[] _record, string key)
        {
            return get_value(_record, key);
        }
        public static string GetParameterValue(string json, string key)
        {
            string[] array = JSon.JsonToArray(json);
            return get_value(array, key);
        }
        public static string GetParameterValue(Dictionary<string, string> dictionary, string key)
        {
            string value = "";
            if (dictionary.ContainsKey(key))
            {
                value = dictionary[key];
            }
            else {
                value = "";
            }

            return value;
        }
        
    }
}
