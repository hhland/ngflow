<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyFlow_ModifyOldDataSelect.aspx.cs" Inherits="CCFlow.WF.MyFlow_ModifyOldDataSelect" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Old WorkID</title>
    <base target="_self" />

    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function sendFromChild() {
            window.returnValue = $("#txtOldID1").val();
            window.close();
        }


        function sendKeyDown() {
            if (event.keyCode == 13) {
                sendFromChild();
            }
        }
    </script>

    <style type="text/css">
        body
        {
            font-family:Tahoma;
            font-size: 11px;
            }
        
        .tdLeft
		{
		    width:174px;
		    text-align:right;
            background-color: rgb(214, 226, 243);
		    }
		    
		.tdRight
		{
		    width:auto;
		}
		
		.tdRight input
		{
		    width:100%;
		    }
    </style>
</head>
<body>
    <form id="form1" runat="server" onkeydown="sendKeyDown();">
    <div>
        <table style="width:100%">
        <tr>
            <td colspan="2">
                <table id="tbSearch" runat="server" style="width:100%;font-size:11px">
                    
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:right;width:100%">                
                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="90px" 
                    Height="21px" onclick="btnSearch_Click"/>                
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="GVData" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BorderStyle="Groove" BorderWidth="1px" OnRowDataBound="GVData_RowDataBound" PageSize="3"
                    Width="100%">
                    <PagerSettings Mode="NumericFirstLast" Visible="False" />
                    <PagerStyle BackColor="LightSteelBlue" HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Center" BackColor="#006699" Font-Size="11px" ForeColor="White"
                        Font-Bold="false" Height="20px" Font-Names="Tahoma" />
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <RowStyle HorizontalAlign="Left" Wrap="false" Height="25px" Font-Size="11px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="LabVisible" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OID")%>'
                                    Visible="False"></asp:Label><asp:CheckBox ID="CheckSelect" runat="server" onclick="CheckSelect(GVData,this);" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        
                    </Columns>
                    <EmptyDataTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="border-right: black 1px; border-top: black 1px; border-left: black 1px;
                                    border-bottom: black 1px; background-color: whitesmoke;">
                                    No data temporarily in the list
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
            <tr>
                <td style="width:100%">
                    <input id="txtOldID1" type="text" value=""   style="width:100%"/>
                    <asp:TextBox ID="txtOldID" runat="server" Text="180" style="display:none"></asp:TextBox>
                </td>
                <td style="width:90px">
                    <input type="button" value="Determine" onclick="sendFromChild();" />
                </td>
            </tr>
        </table>        
    </div>

    </form>

    <script type="text/javascript">
        function CheckSelect(obj, row) {
            $("#" + obj.id + " input[type='checkbox']").attr("checked", false);
            $($(row).parent().parent()).find("input[type='checkbox']").attr("checked", true);
            $("#txtOldID1").val($(row).parent().parent().find("td:eq(1)").text());
        }

        function CheckSelect_Row(obj, row) {
            $("#" + obj.id + " input[type='checkbox']").attr("checked", false);
            $(row).find("input[type='checkbox']").attr("checked", true);
            $("#txtOldID1").val($(row).find("td:eq(<%=oidIndex %>)").text());
        }
    </script>
</body>
</html>
