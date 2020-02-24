using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections;
namespace PAYFORT
{
   public class Security
    {
        public enum SHA_Type { SHA_512, SHA_256 }
        private static string GetHashSHA(string text, SHA_Type _type)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string hashString = string.Empty;
            byte[] hash = null;
            if (_type == SHA_Type.SHA_512)
            {
                SHA512Managed hashstring_512 = new SHA512Managed();
                hash = hashstring_512.ComputeHash(bytes);
            }
            else
            {
                SHA256Managed hashstring_256 = new SHA256Managed();
                hash = hashstring_256.ComputeHash(bytes);
            }
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
        internal static string generate_signature(string[] FORTRequestParameters)
        {


            FORTRequestParameters = PayFortConfig.AddConfigToParameters(FORTRequestParameters);

            string signature = "";
            for (int i = 0; i < FORTRequestParameters.Length; i++)
            {
                string par = FORTRequestParameters[i];
                if    (   par.IndexOf("signature")!=-1 ||    par.IndexOf(  "card_security_code") != -1 || par.IndexOf("card_number") != -1 || par.IndexOf("expiry_date") != -1 || par.IndexOf("card_holder_name") != -1 )
                {
                    FORTRequestParameters[i] = "";
                }
            }
            FORTRequestParameters = FORTRequestParameters.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Array.Sort(FORTRequestParameters, StringComparer.InvariantCulture);
            string _pars = PayFortConfig.SHA_RequestPhase + string.Join("", FORTRequestParameters) + PayFortConfig.SHA_RequestPhase;
            string Hashed_SHA = GetHashSHA(_pars, PayFortConfig.sha_type);
            signature = Hashed_SHA;
            return signature;

        }
        public static string GenerateSignature(string[] FORTRequestParameters)
        {
            return generate_signature(FORTRequestParameters);
        }
        public static string GenerateSignature(ArrayList _record)
        {
            string[] FORTRequestParameters = _record.ToArray(typeof(string)) as string[];
            return generate_signature(FORTRequestParameters);

        }
        public static string GenerateSignature(string json)
        {
            string[] FORTRequestParameters = JSon.JsonToArray(json);
            return generate_signature(FORTRequestParameters);

        }
        public static string GenerateSignature(Dictionary<string, string> dictionary)
        {
            ArrayList tmp = new ArrayList();
            foreach (var item in dictionary)
            {
                tmp.Add(item.Key.ToString() + "=" + item.Value.ToString());
            }
            string[] FORTRequestParameters = tmp.ToArray(typeof(string)) as string[];
            return generate_signature(FORTRequestParameters);
        }
        internal static Boolean validate_signature(string[] FORTRequestParameters)
        {

           string returned_signature= Command.GetParameterValue(FORTRequestParameters, "signature");
            FORTRequestParameters = PayFortConfig.AddConfigToParameters(FORTRequestParameters);
            string signature = "";

            for (int i = 0; i < FORTRequestParameters.Length; i++)
            {
                if(FORTRequestParameters[i].IndexOf("signature")!=-1)
                {
                    FORTRequestParameters[i] = "";

                }
            }


            FORTRequestParameters = FORTRequestParameters.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Array.Sort(FORTRequestParameters, StringComparer.InvariantCulture);
            string _pars = PayFortConfig.SHA_ResponsePhase + string.Join("", FORTRequestParameters) + PayFortConfig.SHA_ResponsePhase;
            string Hashed_SHA = GetHashSHA(_pars, PayFortConfig.sha_type);
            signature = Hashed_SHA;
            if (signature == returned_signature)
            { return true; }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateSignature (string[] FORTRequestParameters)
        {
            return validate_signature(FORTRequestParameters);
        }
        public static Boolean ValidateSignature(ArrayList _record)
        {
            string[] FORTRequestParameters = _record.ToArray(typeof(string)) as string[];
            return validate_signature(FORTRequestParameters);
        }
        public static Boolean ValidateSignature(string json)
        {
            string[] FORTRequestParameters = JSon.JsonToArray(json);
            return validate_signature(FORTRequestParameters);
        }
        public static Boolean ValidateSignature(Dictionary<string, string> dictionary)
        {
            ArrayList tmp = new ArrayList();
            foreach (var item in dictionary)
            {
                tmp.Add(item.Key.ToString() + "=" + item.Value.ToString());
            }
            string[] FORTRequestParameters = tmp.ToArray(typeof(string)) as string[];
            return validate_signature(FORTRequestParameters);
        }
    }
}
