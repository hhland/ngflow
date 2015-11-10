<%@ Page Title="5.  Set Report Permissions " Language="C#" MasterPageFile="RptGuide.Master" AutoEventWireup="true"
    CodeBehind="S6_Power.aspx.cs" Inherits="CCFlow.WF.MapDef.Rpt.S6_Power" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/EasyUIUtility.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OpenDailog(dlgTitle, rpt, rptNo) {
            var url = "../../Comm/RefFunc/Dot2DotSingle.aspx?EnsName=BP.WF.Rpt.MapRpts&EnName=BP.WF.Rpt.MapRpt&AttrKey=BP.WF.Rpt." + rpt + "&No=" + rptNo;
            OpenEasyUiDialog(url, 'eudlgframe', dlgTitle, 520, 321, null, false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<fieldset>
<legend> View Access Control </legend>
[ ] Anyone can view , Process data .
[ ] Has the following  { Post / Department }  You can view .
    Post 
    Department .
[ ] You can view the following designated person , Configure a sql.    
</fieldset>

----  Data permissions department  ------------
   【】 Data can be viewed in all sectors .
   【】 Data can be viewed in this sector .
    【】  Can be viewed in this sector , The sector sub-sector level data .
    【】 You can only view I participated in the process data .
    【】 You can view the specified department processes data .


    <ul class="navlist">
        <li>
            <div>
                <a href="javascript:OpenDailog('1.  Permissions post ', 'RptStations','<%=this.RptNo%>')"><span class="nav">
                    1.  Permissions post </span></a></div>
        </li>
        <li>
            <div>
                <a href="javascript:OpenDailog('2.  Department Permissions ', 'RptDepts','<%=this.RptNo%>')"><span class="nav">
                    2.  Department Permissions </span></a></div>
        </li>
        <li>
            <div>
                <a href="javascript:OpenDailog('3.  Staff privileges ', 'RptEmps','<%=this.RptNo%>')"><span class="nav">
                    3.  Staff privileges </span></a></div>
        </li>
    </ul>




    <cc1:LinkBtn ID="Btn_Cancel1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-undo'"
        Text=" Cancel " OnClick="Btn_Cancel_Click" />


</asp:Content>
