using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PAYFORT
{
    public static class PayFortConfig
    {
        public static string access_code { get; set; }
        public static string merchant_identifier { get; set; }
        public static string language { get; set; }
        public static  Security.SHA_Type sha_type { get; set; }
        public static string SHA_RequestPhase { get; set; }
        public static string SHA_ResponsePhase { get; set; }
        public static Boolean SandBoxMode { get; set; }
        public static Dictionary<string, string>  AddConfigToParameters(Dictionary<string, string> PayFortParameters)
        {
            ArrayList tmp = new ArrayList();
            foreach (var item in PayFortParameters)
            {
                tmp.Add(item.Key.ToString() + "=" + item.Value.ToString());
            }
            string[] ar = tmp.ToArray(typeof(string)) as string[];
            string[] result= add_config_to_parameters(ar);
            ArrayList ar2 = new ArrayList();
            ar2.AddRange(result);
            Dictionary<string, string> dd = new Dictionary<string, string>();
            foreach(string s in ar2)
            {
                string[] x = s.Split('=');
                string key = x[0];
                string value =x[1];
                dd.Add(key, value);
            }
            return dd;

        }
        public static string[] AddConfigToParameters(string[] PayFortParameters)
        {
            return add_config_to_parameters(PayFortParameters);
        }
        public static ArrayList AddConfigToParameters(ArrayList PayFortParameters)
        {
            string[] tmp= add_config_to_parameters(PayFortParameters.ToArray(typeof(string)) as string[]);
            ArrayList ar = new ArrayList();
            ar.AddRange(tmp);
            return ar;
        }
        internal static string[] add_config_to_parameters(string[] pars)
        {
            ArrayList ar = new ArrayList();
            ar.AddRange(pars);
            if( find(pars, "access_code") ==false)
            {
                ar.Add("access_code="+access_code);
            }
            if (find(pars, "merchant_identifier") == false)
            {
                ar.Add("merchant_identifier="+ merchant_identifier);
            }
            if (find(pars, "language") == false)
            {
                ar.Add("language="+ language);
            }
            return ar.ToArray(typeof(string)) as string[];
        }
        internal static Boolean find(string[] ar,string key) {

            string tmp = string.Join("", ar);
            if (tmp.IndexOf(key) == -1) { return false; }
            
            return true;


        }



    }
}
