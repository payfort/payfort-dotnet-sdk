# PayFort .NET SDK

This version is beta
This SDK provides various API functions to manage PayFort requests, responses and some helpful functions that developers can use in order to decrease  the time  of integration process.

This SDK is currently support **Marchant Page 2.0**, All Payfort services  will be available as soon as possible.
For any inquery or Questions, Please feel free to contact us



## How to use
* Download the DLL file and add it as a web reference on your project. 
* Make sure that PAYFORT.SDK.dll file inside  your  **Bin** folder.  
* 
# SDK Methods
PayFort .NET SDK has several useful functions to give developers an easy way to interact directly with Payfort. Below a list of all PayFort .NET SDK functions:

##### GenerateSignature
> This function is used to generate signature string for Payfort requests.

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| PayfortParameters | string[], ArrayList, Dictionary or JSON. |
| Returned Value | String |


Sample 

 ```csharp
string[] pars={"access_code=XX","language=en","merchant_reference=blabla"};
string Signature=PAYFORT.Security.GenerateSignature(pars);
```
##### ValidateSignature
> This function is used to validate returned signature from Payfort.

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| PayfortParameters | string[], ArrayList, Dictionary or JSON. |
| Returned Value | Boolean |

Sample 

```csharp
string[] pars={"access_code=XX","language=en","merchant_reference=blabla"};
Boolean isValid=PAYFORT.Security.ValidateSignature(pars);
```
##### SendRequestToPayFORT
> This function is used to send host-to-host request to Payfort.

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| PayfortParameters | string[], ArrayList, Dictionary or JSON. |
| Returned Value | string[], ArrayList, Dictionary or JSON. |

Sample 

```csharp
string[] pars={"access_code=XX","language=en","merchant_reference=blabla"};
string[] response=PAYFORT.Command.SendRequestToPayFORT(pars);
```
##### GetPAYFORTResponse
> This function is used to collect the response parameters of “redirect request”
 From PAYFORT.

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| NameValueCollection | httpRrequestCollection. |
| Returned Value | string[] |

Sample 

```csharp
string[] response=PAYFORT.Command.GetPAYFORTResponse(Request.QueryString);
```
##### GetAPIURL
> This function is used to retrieve Payfort API URL depend on several Conditions

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| IntegrationTypes | "Redirect" or "Host_to_Host" |
| SandBox | Boolean. default is "false" |
| Returned Value | String URL |

Sample 

```csharp
string _url=PAYFORT.Command.GetAPIURL(PAYFORT.Command.IntegrationTypes.Host_to_Host,true);
```
##### GetParameterValue
> This function is used to retrieve value of a key from different source Objects 

 Please read section "in common" and "PayfortConfig" for more details.

|Parameters | Description |
| --- | --- |
| Source | string[], ArrayList, Dictionary or JSON. |
| key | String |
| Returned Value | String |

Sample 
```csharp
ArrayList ar=new ArrayList{"x=a","y=b"};
string _value=PAYFORT.Command.GetParameterValue(ar,"x");
```
# In Common
* When you use string[] or ArrayList in any Payfort .NET SDK function either as an input or output parameter, Please be aware that the parameters **MUST be in "key=value"** format.   

example: 
```csharp
ArrayList ar=new ArrayList();
ar.Add("access_code=XXYYZZ");  //This is right  
ar.Add("access_code");  //This is wrong  
string[] arr=new string[] {"access_code=Z","signature=x"}; //This is right
string[] arr=new string[] {"access_code","z"}; //This is wrong
```

* Funtion **SendRequestToPayFORT**: This function will check if "signature" passed along with other parameters,
in case didnt pass then it will generate one and passed to payfort alonf with other passed parameters.

# PayfortConfig
Instead of passing {access_code, language, merchant_identifier, SHARequestPhase, SHAResponsePhase, SandBoxMode and SHA hash type} in every time you use this SDK functions, You can store them using class **PayfortConfig**:
```csharp
PAYFORT.PayFortConfig.access_code ="abc";
PAYFORT.PayFortConfig.language ="en"; 
PAYFORT.PayFortConfig.merchant_identifier ="xyz";
PAYFORT.PayFortConfig.SHA_RequestPhase ="x"; 
PAYFORT.PayFortConfig.SHA_ResponsePhase ="y"; 
PAYFORT.PayFortConfig.SandBoxMode =true; 
PAYFORT.PayFortConfig.sha_type = PAYFORT.Security.SHA_Type.SHA_256;
```
Every function in this SDK will do a check if the above parameters are passed to it or not, incase you didnt pass them then the function will retrieve the required ones from shared class **"PayfortConfig"**.

when you pass any parameteres mention above to a function, the function will use the passed parameter even when you already set its value in PayfortConfig class
