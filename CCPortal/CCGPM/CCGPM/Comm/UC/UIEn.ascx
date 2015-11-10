<%@ Control Language="C#" AutoEventWireup="true" Inherits="Comm_UC_UIEn" Codebehind="UIEn.ascx.cs" %>
	<%@ Register src="UCEn.ascx" tagname="UCEn" tagprefix="uc1" %>
<%@ Register src="ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
<div  class="ToolBar" style="padding-bottom:6px;padding-top:2px;" >
<uc2:ToolBar ID="ToolBar1" runat="server" />
</div>
<div >
<div style="margin-top:1px; padding-left:4px">
<uc1:UCEn ID="UCEn1" runat="server" />
</div>
</div>
