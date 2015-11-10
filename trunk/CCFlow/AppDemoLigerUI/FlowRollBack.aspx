<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowRollBack.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.FlowRollBack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Rollback Process </title>
        <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
     <div>
        <fieldset>
            <legend> Rollback Process  </legend> Roll back to a node :<asp:DropDownList ID="DDL_SkipToNode" runat="server">
            </asp:DropDownList>
            <br>
             Rollback reason :
            <br>
            <asp:TextBox ID="TB_Note" TextMode="MultiLine" Rows="5" runat="server" Width="308px"></asp:TextBox>
            <br>
            <asp:Button ID="Btn_OK" runat="server" Text=" OK rollback " OnClick="Btn_OK_Click" />
            <asp:Button ID="Btn_Cancel" runat="server" Text=" Cancel " OnClick="Btn_Cancel_Click" />
        </fieldset>
    </div>
    </form>
</body>
</html>
