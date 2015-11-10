<%@ Page Title="ccflow Form Designer " Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.WF_MapDef_MapDefDtlCCForm" Codebehind="MapDefDtlFreeFrm.aspx.cs" %>
<%@ Import Namespace="BP.Sys" %>
 <%@ Register src="../Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <base target="_self" />
<script language="javascript">
	function HelpGroup()
	{
	   var msg=' Field Grouping : Is to put together a similar field , Allowing users to operate more friendly .\t\n Such as : We designed a basic taxpayer information collection node .';
	   msg+=' Basic information when registering taxpayers , We can put basic information , Travel Information , Real Estate Information , Investor information packet .\t\n \t\n Packet format is :@ From the field name 1= Group Name 1@ From the field name 2= Group Name 2 ,\t\n Such as : Node information set ,@NodeID= Basic Information @LitData= Assessment Information .';
       alert( msg);
	}
	function DoGroupF( enName)
	{
	    var b=window.showModalDialog( 'GroupTitle.aspx?EnName='+enName , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
	}
	function Insert(mypk,IDX)
    {
        var url='Do.aspx?DoType=AddF&MyPK='+mypk+'&IDX=' +IDX ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
	function AddF(mypk) {

        var url='Do.aspx?DoType=AddF&MyPK='+mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 

        window.location.href = window.location.href;
    }
    function AddTable(mypk)
    {
        var url='EditCells.aspx?MyPK='+mypk;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function MapExt(mypk) {
        var url = 'MapExt.aspx?FK_MapData=' + mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 800px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function CopyFieldFromNode(mypk)
    {
        var url='CopyFieldFromNode.aspx?DoType=AddF&FK_Node='+mypk;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 700px; dialogWidth: 900px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function HidAttr(mypk) {
        alert(mypk);
        var url = 'HidAttr.aspx?FK_MapData=' + mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 700px; dialogWidth: 900px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function GroupFieldNew(mypk)
    {
        var url='GroupField.aspx?RefNo='+mypk+"&RefOID=0&DoType=FunList";
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 200px; dialogWidth: 600px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function GroupField(mypk, OID )
    {
        var url='GroupField.aspx?RefNo='+mypk+"&RefOID="+OID ;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 200px; dialogWidth: 600px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function GroupFieldDel(mypk,refoid)
    {
        var url='GroupField.aspx?RefNo='+mypk+'&DoType=DelIt&RefOID='+refoid ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
     
    function Edit(mypk,refno, ftype)
    {
        var url='EditF.aspx?DoType=Edit&MyPK='+mypk+'&RefNo='+refno +'&FType=' + ftype;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 960px; dialogWidth:1024px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function EditEnum(mypk,refno)
    {
        var url='EditEnum.aspx?DoType=Edit&MyPK='+mypk+'&RefNo='+refno;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
     function EditTable(mypk,refno)
    {
        var url='EditTable.aspx?DoType=Edit&MyPK='+mypk+'&RefNo='+refno;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    
	function Up(mypk,refoid,idx)
    {
        var url='Do.aspx?DoType=Up&MyPK='+mypk+'&RefNo='+refoid+'&ToIdx='+idx;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        //window.location.href ='MapDef.aspx?PK='+mypk+'&IsOpen=1';
        window.location.href = window.location.href ;
    }
    function Down(mypk,refoid,idx)
    {
        var url='Do.aspx?DoType=Down&MyPK='+mypk+'&RefNo='+refoid +'&ToIdx='+idx;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    function GFDoUp(refoid)
    {
        var url='Do.aspx?DoType=GFDoUp&RefOID='+refoid ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href ;
    }
    function GFDoDown(refoid)
    {
        var url='Do.aspx?DoType=GFDoDown&RefOID='+refoid ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }

    function FrameDoUp(MyPK) {
        var url = 'Do.aspx?DoType=FrameDoUp&MyPK=' + MyPK;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function FrameDoDown(MyPK) {
        var url = 'Do.aspx?DoType=FrameDoDown&MyPK=' + MyPK;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }

    function DtlDoUp(MyPK)
    {
        var url='Do.aspx?DoType=DtlDoUp&MyPK='+MyPK ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href ;
    }
    function DtlDoDown(MyPK)
    {
        var url='Do.aspx?DoType=DtlDoDown&MyPK='+MyPK;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }

    function M2MDoUp(MyPK) {
        var url = 'Do.aspx?DoType=M2MDoUp&MyPK=' + MyPK;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function M2MDoDown(MyPK) {
        var url = 'Do.aspx?DoType=M2MDoDown&MyPK=' + MyPK;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }

    function Del(mypk,refoid)
    {
        if (window.confirm(' Are you sure you want to delete it ?') ==false)
            return ;
    
        var url='Do.aspx?DoType=Del&MyPK='+mypk+'&RefOID='+refoid;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
	function Esc()
    {
        if (event.keyCode == 27)     
        window.close();
       return true;
    }

    function GroupBarClick(rowIdx) {
        var alt = document.getElementById('Img' + rowIdx).alert;
        var sta = 'block';
        if (alt == 'Max') {
            sta = 'block';
            alt = 'Min';
        } else {
            sta = 'none';
            alt = 'Max';
        }
        document.getElementById('Img' + rowIdx).src = './Img/' + alt + '.gif';
        document.getElementById('Img' + rowIdx).alert = alt;
        var i = 0
        for (i = 0; i <= 40; i++) {
            if (document.getElementById(rowIdx + '_' + i) == null)
                continue;
            if (sta == 'block') {
                document.getElementById(rowIdx + '_' + i).style.display = '';
            } else {
                document.getElementById(rowIdx + '_' + i).style.display = sta;
            }
        }
    }
    var isInser = "";
    function CopyFieldFromNode(mypk) {
        var url = 'CopyFieldFromNode.aspx?FK_Node=' + mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 700px; dialogWidth: 900px;center: yes; help: no');
        window.location.href = window.location.href;
    }

  function EditDtl(mypk, dtlKey) {
      var url = 'MapDtl.aspx?DoType=Edit&FK_MapData=' + mypk + '&FK_MapDtl=' + dtlKey;
      var b = window.showModalDialog(url, 'ass', 'dialogHeight: 600px; dialogWidth: 700px;center: yes; help:no;resizable:yes');
     // var b = window.showModalDialog(url, 'ass', 'dialogHeight: 700px; dialogWidth: 800px;center: yes; help:no;resizable:yes');
      window.location.href = window.location.href;
  }

  function EditM2M(mypk, dtlKey) {
      var url = 'MapM2M.aspx?DoType=Edit&FK_MapData=' + mypk + '&NoOfObj=' + dtlKey;
      var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
      window.location.href = window.location.href;
  }
  
  function MapDtl( mypk  )
  {
      var url='MapDtl.aspx?DoType=DtlList&FK_MapData=' + mypk   ;
        var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no'); 
        window.location.href = window.location.href;
    }
    
    ///  Multiple choice .
    function MapM2M(mypk) {
        var url = 'MapM2M.aspx?DoType=List&FK_MapData=' + mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function EditFrame(mypk, dtlKey) {
        var url = 'MapFrame.aspx?DoType=Edit&FK_MapData=' + mypk + '&FK_MapFrame=' + dtlKey;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function MapFrame(mypk) {
        var url = 'MapFrame.aspx?DoType=DtlList&FK_MapData=' + mypk;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
        window.location.href = window.location.href;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Pub ID="Pub1" runat="server"  />
    <%
        
         %>
         <div  style='width:100%;text-align:right;'>
                <asp:DropDownList ID="drpFormGroup" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnSaveFormDesigner" runat="server" 
                    Text="Save in form designer" onclick="btnSaveFormDesigner_Click" OnClientClick="return confirm('Are you sure to save this Dtl to Form Designer?');"/>
            </div>
    <div class='easyui-layout' data-options='fit:true'>
        <div data-options="region:'north',noheader:true,split:false,border:false" style='height:30px;overflow-y:hidden'>
            <div style='float:left'>
                <a href="javascript:EditDtl('<%=this.FK_MapData %>','<%=dtl.No %>')" class='easyui-linkbutton' data-options="iconCls:'icon-edit',plain:true"><%=dtl.Name %></a>
            </div>
            <div style='float:right'>
                <a href="javascript:document.getElementById('F<%=dtl.No %>').contentWindow.AddF('<%=dtl.No %>');" class='easyui-linkbutton' data-options="iconCls:'icon-new',plain:true"> Insert Column </a>
                <a href="javascript:document.getElementById('F<%=dtl.No %>').contentWindow.AddFGroup('<%=dtl.No %>');" class='easyui-linkbutton' data-options="iconCls:'icon-new',plain:true"> Insert Column Group </a>
                <a href="javascript:document.getElementById('F<%=dtl.No %>').contentWindow.CopyF('<%=dtl.No %>');" class='easyui-linkbutton' data-options="iconCls:'icon-add',plain:true"> Copy Column </a>
                <a href="javascript:document.getElementById('F<%=dtl.No %>').contentWindow.HidAttr('<%=dtl.No %>');" class='easyui-linkbutton' data-options="iconCls:'icon-add',plain:true"> Hide Columns </a>
                <a href="javascript:document.getElementById('F<%=dtl.No %>').contentWindow.DtlMTR('<%=dtl.No %>');" class='easyui-linkbutton' data-options="iconCls:'icon-add',plain:true"> Multi-header </a>
                <a href='Action.aspx?FK_MapData=<%=this.FK_MapDtl %>' class='easyui-linkbutton' data-options="iconCls:'icon-add',plain:true"> From table event </a>
                <a href="javascript:DtlDoUp('<%=dtl.No %>')" class='easyui-linkbutton' data-options="iconCls:'icon-up',plain:true"></a>
                <a href="javascript:DtlDoDown('<%=dtl.No %>')" class='easyui-linkbutton' data-options="iconCls:'icon-down',plain:true"></a>
            </div>
            <div style='clear:both'></div>
        </div>
        <div data-options="region:'center',noheader:true">
            <iframe ID='F<%=dtl.No %>' frameborder='0' scrolling="auto" style='width:100%;height:100%' src='MapDtlDe.aspx?DoType=Edit&FK_MapData=<%=this.FK_MapData %>&FK_MapDtl=<%=dtl.No %>'></iframe>
        </div>
    </div>
</asp:Content>

