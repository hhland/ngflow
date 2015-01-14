<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="uc1" TagName="UCEn" Src="UC/UCEn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Page language="c#" Inherits="CCFlow.Web.Comm.GroupEnsNum" Codebehind="Group.aspx.cs" %>
<%@ Register src="UC/ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
<%@ Register src="UC/UCGraphics.ascx" tagname="UCGraphics" tagprefix="uc3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>  Thank you for choosing CCFlow</title>
		<META content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<META content="C#" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<META http-equiv="Page-Enter" content="revealTrans(duration=0.5, transition=8)" >
		<META content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="JScript.js"></script>
		<base target=_self />
        <script type="text/javascript">
             //   Event .
            function DDL_mvals_OnChange(ctrl, ensName, attrKey) {

                var idx_Old = ctrl.selectedIndex;

                if (ctrl.options[ctrl.selectedIndex].value != 'mvals')
                    return;
                if (attrKey == null)
                    return;

                //alert(ensName);
                //alert(attrKey);

                var url = 'SelectMVals.aspx?EnsName=' + ensName + '&AttrKey=' + attrKey;
                var val = window.showModalDialog(url, 'dg', 'dialogHeight: 450px; dialogWidth: 450px; center: yes; help: no');

                if (val == '' || val == null) {
                    // if (idx_Old==ctrl.options.cont
                    ctrl.selectedIndex = 0;
                    //    ctrl.options[0].selected = true;
                }
            }
        </script>
	</HEAD>
	<body  onkeypress=Esc() leftMargin=0 background=BJ1.gif topMargin=0>
		<form id="Form1" method="post" runat="server">
			<TABLE height="100%" style="background:none;"  cellPadding=0 width="100%" align=left border=0>
					<caption ><asp:label id="Label1" runat="server">Label</asp:label>
				<TR>
					<TD colSpan="2" class=toolbar >
                        <uc2:ToolBar ID="ToolBar1" runat="server" />
                    </TD>
				</TR>
				<TR vAlign="top" height="100%">
					<TD vAlign="top" noWrap width="18%"  >
						<TABLE width="100%" cellspacing="1" style="border:1px #abcbe6 solid;">
							<TR>
								<TD border="0" align=center class="GroupTitle" ><b> Display content </b></TD>
							</TR>
							<tr>
								<TD  style="font-size:12px;"><asp:checkboxlist id="CheckBoxList1" runat="server" AutoPostBack=true ></asp:checkboxlist></TD>
							</TR>
							<tr >
								<TD nowarp=true align=left  style="font-size:12px;"><hr><uc1:ucsys id="UCSys2" runat="server"></uc1:ucsys></TD>
							</TR>
							<TR>
								<TD class="GroupTitle" >
								<asp:CheckBox ID="CB_IsShowPict" runat="server" Text=" Display graphics " AutoPostBack=true Font-Bold="True" />
								</TD>
							</TR>
							<TR>
								<TD>
								<table width='100%'>
								<tr style="font-size:12px;">
								<TD>  Height :</TD>
								  <TD class="TD"><cc1:tb id="TB_H" runat="server" ShowType="Num" Width="80px">400</cc1:tb></TD>
								 </tr>
								<tr style="font-size:12px;">
								<TD> Width :</TD>
									<TD class="TD" >
									<cc1:tb id="TB_W" runat="server" ShowType="Num" Width="80px">600</cc1:tb>
									</TD>
								</TR>
								</table>
                            </TD>
							</TR>
						</TABLE>
					</TD>
					<TD valign="top" ><cc1:bptabstrip id="BPTabStrip1" Visible=true runat="server"  
							BorderStyle="None" BorderWidth="2px" TargetID="BPMultiPage1" Height="25px"   
							TabDefaultStyle="font-size:12px;background:white;padding:3px;text-align:center;text-decoration:none;"   
  TabHoverStyle="color:red;"
  TabSelectedStyle="background:white;border-bottom:none"
							>
							<iewc:Tab Text="  Form " ID="ShowTable" DefaultImageUrl="../Img/Pub/Table.gif" ></iewc:Tab>
							<iewc:TabSeparator></iewc:TabSeparator>
							<iewc:Tab Text="  Histogram "  ID="ShowZZT" DefaultImageUrl="../Img/Pub/Histogram.ico"></iewc:Tab>
							<iewc:TabSeparator></iewc:TabSeparator>
							<iewc:Tab Text="  Pie "  ID="ShowPie" DefaultImageUrl="../Img/Pub/Pie.ico"></iewc:Tab>
							<iewc:TabSeparator></iewc:TabSeparator>
							<iewc:Tab Text="  Line chart " ID="ShowZXT" DefaultImageUrl="../Img/Pub/ZX.ico"></iewc:Tab>
							<iewc:TabSeparator></iewc:TabSeparator>
						</cc1:bptabstrip>
						<cc1:bpmultipage id="BPMultiPage1" runat="server" Width="95%" Height="100%" BorderColor=GhostWhite>
							<IEWC:PAGEVIEW id="P0">
								<uc1:ucsys id="UCSys1" runat="server"></uc1:ucsys>
								<table class=Table1 border=0 class=Table>
								<tr>
								<td class=TD>
								<uc1:UCSys ID="UCSys3" runat="server" />
								</td>
								</tr>
							</table>
							</IEWC:PAGEVIEW>
							<IEWC:PAGEVIEW id="P1" BorderColor=White>
								<cc1:BPImage id="Img1" BorderWidth=0 runat="server"></cc1:BPImage>
							</IEWC:PAGEVIEW>
							<IEWC:PAGEVIEW id="P2" BorderColor=White >
								<cc1:BPImage  id="Img2"  BorderWidth=0 runat="server"></cc1:BPImage>
							</IEWC:PAGEVIEW>
							<IEWC:PAGEVIEW id="P3" BorderColor=White>
								<cc1:BPImage id="Img3"  BorderWidth=0 runat="server"></cc1:BPImage>
							</IEWC:PAGEVIEW>
						</cc1:bpmultipage> </TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
