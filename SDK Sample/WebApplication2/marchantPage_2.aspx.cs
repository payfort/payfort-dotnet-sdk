using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PAYFORT;
namespace WebApplication2
{
    public partial class test_redirect : System.Web.UI.Page
    {
        //first declare a public Parameters to use them in aspx and code behind
        public string api_url{get;set;}
        public string merchant_referance { get; set; }
        public string signature { get; set; }
        public string return_url { get; set; }
        public string access_code { get; set; }
        public string language { get; set; }
        public string command_service { get; set; }
        public string merchant_identifier { get; set; }


        //---------------------------------------------------------------------
        private void setConfig()
        {
            PAYFORT.PayFortConfig.access_code = System.Configuration.ConfigurationManager.AppSettings["access_code"].ToString();
            PAYFORT.PayFortConfig.language = System.Configuration.ConfigurationManager.AppSettings["language"].ToString();
            PAYFORT.PayFortConfig.merchant_identifier = System.Configuration.ConfigurationManager.AppSettings["merchant_identifier"].ToString();
            PAYFORT.PayFortConfig.SHA_RequestPhase = System.Configuration.ConfigurationManager.AppSettings["sha_request"].ToString();
            PAYFORT.PayFortConfig.SHA_ResponsePhase = System.Configuration.ConfigurationManager.AppSettings["sha_response"].ToString();
            int sand = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sandbox"].ToString());
            PAYFORT.PayFortConfig.SandBoxMode = Convert.ToBoolean(sand);


            access_code = PAYFORT.PayFortConfig.access_code;
            language = PAYFORT.PayFortConfig.language;
            command_service = "TOKENIZATION";
            merchant_identifier = PAYFORT.PayFortConfig.merchant_identifier;

            if (System.Configuration.ConfigurationManager.AppSettings["sha_type"].ToString() == "SHA-256")
            {
                PAYFORT.PayFortConfig.sha_type = PAYFORT.Security.SHA_Type.SHA_256;
            }
            else
            {
                PAYFORT.PayFortConfig.sha_type = PAYFORT.Security.SHA_Type.SHA_512;

            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {

            setConfig();

            api_url = PAYFORT.Command.GetAPIURL( Command.IntegrationTypes.Redirect,true);



            merchant_referance = Guid.NewGuid().ToString();


            return_url = "http://localhost:65379/FortResponse.aspx";


            ArrayList PayfortParameters = new ArrayList
            {
                "service_command=TOKENIZATION",
                "merchant_reference="+merchant_referance,
                "return_url="+return_url
            };
            signature= PAYFORT.Security.GenerateSignature(PayfortParameters);
            





        }
    }
}