using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class FortResponse : System.Web.UI.Page
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
            working();
        }
        private  void working()
        {
            //First you get Payfort Response from Redirect request
            // This function is get the response came from "redirect request" you just made.
            // basicly, its reading the QyeryStrings from httpsRequest and organize them is string[]
            //This is for testing purpose, in real live websites, you should get Payfort response from your "Notification URL" not "returnURL".
            string[] payfort_res = PAYFORT.Command.GetPAYFORTResponse(Request.QueryString);
            //Validate Payfort Reponse
            //you can pass string[],ArrayList or JSON to this function 
            Boolean valid_request = PAYFORT.Security.ValidateSignature(payfort_res);
            //get response_code from Payfort 
            //This function is search for a key inside an object and return its value.
            string response_code = PAYFORT.Command.GetParameterValue(payfort_res, "response_code");
            //check if the response is valid from Payfort
            if (valid_request)
            {
                // check if the Tokenization Request success
                if (response_code == "18000")
                {
                    //create an object with new parameters to send HOST to HOST request to Payfort using SDK method "SendRequestToPayFort"       
                    // ** you can pass JSON,Dictionary,string[] or ArrayList to SDK method "SendRequestToPayFort" 
                    // **incase you pass string[] or ArrayList, the format of the string MUST me "Key=Value" format
                    // ** the following parameters you may or not pass them to "SendRequestToPayfort", in case you
                    //    dont want to send them, the SDK will add them to request parameters using the "PayfortConfig" object you created early:
                    //    {access_code,language,merchant_identifier}
                    //    - Please notice that incase you pass the above parameters, the SDK will  use them instead of values of "PayFortConfig" object.

                    //Please be aware of what Parameters you should pass, different situation with different mandatory parameters.
                    //Forexample: - "customer_ip" is an optional parameter, But in case you sending Host-to-Host request it will be mandatory.
                    //            - when setting payment_option=MADA , you must passed set the command to "Purchase"

                  //  Request.UserHostAddress  ==<< set it too

                    ArrayList trans = new ArrayList
                    {
                        "command=PURCHASE",
                        "merchant_reference="+Request.QueryString["merchant_reference"].ToString(),
                        "amount=500000",
                        "currency=SAR",
                        "customer_email=diaa@gmail.com",
                        "token_name="+Request.QueryString["token_name"].ToString(),
                        "return_url=http://localhost:65379/Response_from_3dSecure.aspx",
                        "customer_ip=192.178.1.10"
                    };
                    //create Object to receive Payfort Response from HOST-to-HOST request
                    // ** you can receive Payfort Response in several types: string[],ArrayList,JSON or Dictionary
                    // ** You can generate Signature for your request,then pass it along with other parameters to SDK method "SendRequestToPayfort",
                    //    in case you didnt passed it, the SDK will generate Signature and send it tp Payfort with other parameters.
                    ArrayList payfort_response = new ArrayList();
                    payfort_response =PAYFORT.Command.SendRequestToPayFORT(trans);

                    //Validate Payfort Reponse
                    Boolean valid_response =PAYFORT.Security.ValidateSignature(payfort_response);
                    if (valid_response)
                    {

                        //check if Payfort is asking for 3-D secure or NOT.
                        // if Payfort requests 3-D secure, the following  will send back with PayFort response:
                        //  response_code =20064
                        //  status =20
                        //  3ds_url=-- the URL that you should redirect your customer to it --
                        string status =PAYFORT.Command.GetParameterValue(payfort_response, "status");
                        string res_code = PAYFORT.Command.GetParameterValue(payfort_response, "response_code");
                        string returned_3dsURL = PAYFORT.Command.GetParameterValue(payfort_response, "3ds_url");
                        if (res_code == "20064" && status == "20")
                        {
                           // redirect the customer to the 3-D secure URL 
                            Response.Redirect(returned_3dsURL.ToString());
                        }
                        else
                        {
                            //success transaction. no 3ds redirect requierd
                            Response.Write(PAYFORT.Command.GetParameterValue(payfort_response, "response_message"));
                           
                        }
                    }
                    else {
                        // the response came from payfort is invalid
                        Response.Write("invalid response from payfort");
                    }
                }
                else {
                    //the response came from payfort is valid, but has some error.[ex: missing parameters]
                    Response.Write("Error:" + Request.QueryString["response_message"].ToString());
                }
            }
            else {
                // the response came from payfort is invalid
                Response.Write("invalid response from payfort");
            }
        }
    }
}