﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.Comm.RefFunc.Dot2Dot_UC"
    CodeBehind="Dot2Dot.ascx.cs" %>
<%@ Register Src="../UC/ToolBar.ascx" TagName="ToolBar" TagPrefix="uc1" %>
<%@ Register Src="../UC/UCSys.ascx" TagName="UCSys" TagPrefix="uc2" %>
<div class="easyui-layout" data-options="fit:true">
    <div id="toolbar" data-options="region:'north',noheader:true,split:false,border:false" style="height: 30px;
        padding: 2px; background-color: #E0ECFF">
        <uc1:ToolBar ID="ToolBar1" runat="server" />
    </div>
    <div id="content" data-options="region:'center',noheader:true,border:false">
        <uc2:UCSys ID="UCSys1" runat="server" />
    </div>
</div>
