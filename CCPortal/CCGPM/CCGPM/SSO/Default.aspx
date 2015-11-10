<%@ Page Title="" Language="C#" MasterPageFile="~/SSO/MasterPage.master" AutoEventWireup="true"
    Inherits="SSO_Default" CodeBehind="Default.aspx.cs" %>

<%@ Register Src="AppBar.ascx" TagName="AppBar" TagPrefix="uc1" %>
<%@ Register Src="InfoBar.ascx" TagName="InfoBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Style/main.css" rel="stylesheet" type="text/css" />
    <link href="./../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="Style/default.css" rel="stylesheet" type="text/css" />
    <script src="../Js/zDialog/zDialog.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <table width="746" border="0" cellspacing="0">
        <tr>
            <td>
                <table width="745" border="0" align="left" cellpadding="0" cellspacing="0" class="border2">
                    <tr>
                        <td height="25" bgcolor="#FFF7D6">
                            <span class="font_01">&nbsp;<img alt="" src="Img/icon.gif" width="11" height="11" /></span>
                            应用系统 &nbsp;&nbsp;<span style="color:red; font-weight:lighter;">（提示：你已经登录CCPort, 你可以直接进入以下系统，而不用登录！）</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="">
                            <div style="overflow: auto; height: auto; text-align:center; ">
                                <uc1:AppBar ID="InfoPush1" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="5">
            </td>
        </tr>
        <tr>
            <td>
                <table width="745" border="0" align="left" cellpadding="0" cellspacing="0" class="border4">
                    <tr>
                        <td height="25" bgcolor="#D4FDE7">
                            <span class="font_01">&nbsp;<img alt="" src="Img/about_b.gif" align="middle" width="16"
                                height="16" /></span> 信息块
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 745px; height: auto;padding:3px 0px 0px 3px;">
                                <uc2:InfoBar ID="InfoBar1" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
