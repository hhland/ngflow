<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.WF_DtlOpt" Codebehind="DtlOpt.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Comm/Style/Tabs.css" rel="stylesheet" type="text/css" />
    <script language=javascript>
        function selectAll() {
            var arrObj = document.all;
            if (document.forms[0].checkedAll.checked) {
                for (var i = 0; i < arrObj.length; i++) {
                    if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
                        arrObj[i].checked = true;
                    }
                }
            } else {
                for (var i = 0; i < arrObj.length; i++) {
                    if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox')
                        arrObj[i].checked = false;
                }
            }
        }

        function checkType() 
        {
            // Get the value of the uploaded file    
            var fileName = document.getElementById("fup").value;

            // Return String Location of the last occurrence of the string object neutron .   
            var seat = fileName.lastIndexOf(".");

            // Back to String Objects in the specified location and converted to lowercase substring .   
            var extension = fileName.substring(seat).toLowerCase();

            // Judge allowed to upload a file format    
            //if(extension!=".jpg"&&extension!=".jpeg"&&extension!=".gif"&&extension!=".png"&&extension!=".bmp"){   
            //alert(" Does not support "+extension+" Upload file !");   
            //return false;   
            //}else{   
            //return true;   
            //}   

            var allowed = [".jpg", ".gif", ".png", ".bmp", ".jpeg"];
            for (var i = 0; i < allowed.length; i++) {
                if (!(allowed[i] != extension)) {
                    return true;
                }
            }
            alert(" Does not support " + extension + " Format ");
            return false;
        }  
    </script>

    <base target=_self />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Pub ID="Top" runat="server" />
    <uc1:Pub ID="Pub1" runat="server" />
</asp:Content>

