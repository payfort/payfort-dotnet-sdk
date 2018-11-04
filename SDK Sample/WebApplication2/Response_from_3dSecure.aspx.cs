using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class Response_from_3dSecure : System.Web.UI.Page
    {
        private void setConfig()
        {
            PAYFORT.PayFortConfig.access_code = System.Configuration.ConfigurationManager.AppSettings["access_code"].ToString();
            PAYFORT.PayFortConfig.language = System.Configuration.ConfigurationManager.AppSettings["language"].ToString();
            PAYFORT.PayFortConfig.merchant_identifier = System.Configuration.ConfigurationManager.AppSettings["merchant_identifier"].ToString();
            PAYFORT.PayFortConfig.SHA_RequestPhase = System.Configuration.ConfigurationManager.AppSettings["sha_request"].ToString();
            PAYFORT.PayFortConfig.SHA_ResponsePhase = System.Configuration.ConfigurationManager.AppSettings["sha_response"].ToString();
            int sand = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sandbox"].ToString());
            PAYFORT.PayFortConfig.SandBoxMode = Convert.ToBoolean(sand);
            if (System.Configuration.ConfigurationManager.AppSettings["sha_type"].ToString() == "SHA-256")
            {
                PAYFORT.PayFortConfig.sha_type = PAYFORT.Security.SHA_Type.SHA_256;
            }
            else
            {
                PAYFORT.PayFortConfig.sha_type = PAYFORT.Security.SHA_Type.SHA_512;

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            setConfig();
            // getting Payfort Response from 3-D secure URL
            //validate the Response
            Boolean valid_request = PAYFORT.Security.ValidateSignature(PAYFORT.Command.GetPAYFORTResponse(Request.QueryString));
            if (valid_request)
            {

                    Response.Write(Request.QueryString["response_message"].ToString());
            }
            else
            {
                Response.Write("invalid response from PAYFORT");
            }



        }
    }
}