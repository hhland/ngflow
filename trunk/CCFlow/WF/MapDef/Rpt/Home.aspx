<%@ Page Title=" Report Design " Language="C#" MasterPageFile="RptGuide.master" AutoEventWireup="true"
    Inherits="WF_MapDef_Rpt_Home" CodeBehind="Home.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <base target="_self" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%
        string rptNo = "ND" + int.Parse(this.FK_Flow) + "MyRpt"; // this.Request.QueryString["RptNo"];

        var rpt = new BP.Sys.MapData();
        rpt.No = rptNo;
        rpt.RetrieveFromDBSources();

        if (string.IsNullOrWhiteSpace(rpt.Name))
        {
    %>
    <h4>
         Failure Report Definition , First run <a href="../../Admin/DoType.aspx?RefNo=<%=this.FK_Flow%>&DoType=FlowCheck&Lang=CH"> Process inspection </a>, Then open the report design wizard .</h4>
    <%
}
           else
           {%>
    <h4>
         Report Design Wizard , Help you complete a personalized report definition :</h4>
    <ul class="navlist">
        <li>
            <div>
                <a href="S1_Edit.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=rptNo%>">
                    <span class="nav">1.  Basic Information </span></a></div>
        </li>
        <li>
            <div>
                <a href="S2_ColsChose.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=rptNo%>">
                    <span class="nav">2.  Set the report display columns </span></a></div>
        </li>
        <li>
            <div>
                <a href="S3_ColsLabel.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=rptNo%>">
                    <span class="nav">3.  Set the report displays the column order </span></a></div>
        </li>
        <li>
            <div>
                <a href="S5_SearchCond.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=rptNo%>">
                    <span class="nav">4.  Setting the report query conditions </span></a></div>
        </li>
        <li>
            <div>
                <a href="S6_Power.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=rptNo%>">
                    <span class="nav">5.  Set Report Permissions </span></a></div>
        </li>
    </ul>    
    <%
}%>
</asp:Content>
