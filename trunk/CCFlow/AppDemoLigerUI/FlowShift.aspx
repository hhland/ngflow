<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowShift.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.FlowShift" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Job transfer </title>
     <script src="/WF/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
   
    <link href="/WF/Scripts/Jquery-plug/jquery-autocomplete/jquery.autocomplete.css"
        rel="stylesheet" type="text/css" />
    <script src="/WF/Scripts/Jquery-plug/jquery-autocomplete/jquery.autocomplete.pack.js"
        type="text/javascript"></script>
    <script src="/WF/Scripts/Jquery-plug/jquery-autocomplete/jquery.autocomplete.js"
        type="text/javascript"></script>
    <script src="/WF/Scripts/Jquery-plug/jquery-autocomplete/jquery.autocomplete.min.js"
        type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
      
            initComp();
        });


        function initComp() {
            $('#<%=TB_Emp.ClientID%>').autocomplete("Base/DataServices.ashx?action=getEmp&key=" + encodeURI($('#<%= TB_Emp.ClientID%>').val()), {
                max: 10,    // The number of entries in the list 
                minChars: 0,    // The minimum character is automatically filled in to complete before activating 
                width: 200,     // Tip width , Overflow hidden 
                scrollHeight: 300,   // Highly suggestive of , Overflow scroll bars 
                matchContains: true,    // Contain a match , Is data Parameters in the data , As long as the data is contained in the text box on the show 
                autoFill: false,    // Automatic filling 
                parse: function (data) {
                    return $.map(eval(data), function (row) {
                        return {
                            data: row,
                            value: row.Name,
                            result: row.Name+"("+row.No+")"
                        }
                    });
                },
                formatItem: function (row, i, max) {
                   
                    return row.Name+"("+row.No+")";
                },
                formatMatch: function (row, i, max) {
                    return row.Name + "(" + row.No + ")";
                },
                formatResult: function (row) {
                    return row.Name + "(" + row.No + ")";
                }
            }).result(function (event, row, formatted) {
                $('#<%= TB_Emp.ClientID%>').val(row.No);
            });
        }
    </script>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset>
            <legend> Select this work should work transferred staff </legend> Please enter the surrender of persons :
            <%--<select id="cmb_Emp" class="easyui-combogrid" style="width: 215px" />--%>
             <asp:TextBox ID="TB_Emp" runat="server" Width="200px"></asp:TextBox>
           <%-- <asp:TextBox ID="TB_Emp" Style="visibility: hidden" runat="server" Width="159px">
            </asp:TextBox>--%>
            <br />
             The reason :
            <br />
            <asp:TextBox ID="TB_Note" TextMode="MultiLine" runat="server" Width="336px" Height="91px"></asp:TextBox>
            <br/>
             Explanation : Enter only one staff . &nbsp;<hr>
            <asp:Button ID="Btn_OK" runat="server" Text=" Determining transfer " OnClick="Btn_OK_Click" />
            <asp:Button ID="Btn_Cancel" runat="server" Text=" Cancel and close " OnClick="Btn_Cancel_Click" />
        </fieldset>
    </div>
    </form>
</body>
</html>
