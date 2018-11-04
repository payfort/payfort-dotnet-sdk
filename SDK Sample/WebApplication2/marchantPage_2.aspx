<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="marchantPage_2.aspx.cs" Inherits="WebApplication2.test_redirect" %>
<html>

    <body>
          <form action="<%=api_url %>" method ='post' name='frm' id="frm">
          <input type='hidden' name='signature' value="<%=signature %>"  />
          <input type='hidden' name='service_command' value='<%=command_service %>' />
          <input type='hidden' name='access_code' value='<%=access_code %>' /> 
          <input type='hidden' name='merchant_identifier' value='<%=merchant_identifier %>' /> 
          <input type='hidden' name='merchant_reference' value="<%=merchant_referance %>" /> 
          <input type='hidden' name='language' value='<%=language %>' />
          <input type='hidden' name='return_url' value="<%=return_url %>" />
             ExpiryDate:<input type='text' name='expiry_date' value='2105' /> <br/>
             Card Number:<input type='text' name='card_number' value='4557012345678902' /> <br/>
             C.S.C: <input type='text' name='card_security_code' value='123' /> <br/>
             Holder name: <input type='text' name='card_holder_name' value='diaa hananny' /> <br/>
          <a href= '#' onclick ='document.getElementById("frm").submit();' >
          <img src = 'http://www.clker.com/cliparts/C/z/u/B/R/w/purchase-md.png' width = '70px' /></a>
          </form>
    </body>


</html>
