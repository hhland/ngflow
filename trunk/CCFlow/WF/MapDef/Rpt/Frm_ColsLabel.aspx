<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true"
    CodeBehind="Frm_ColsLabel.aspx.cs" Inherits="CCFlow.WF.MapDef.Rpt.Frm_ColsLabel" %>

<%@ Register Src="../Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #top
        {
            width: 100%;
            height: 24px;
            text-align: center;
        }
        #top *
        {
            position: relative;
        }
        .toolBar
        {
            width: 100%;
            float: left;
            z-index: 1000;
            overflow: visible;
            position: fixed;
            left: 0px;
            top: 0px;
            _position: absolute;
            _top: expression_r(documentElement.scrollTop);
            background: #909090;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function Esc() {

        }

        var rowsIdex = 0;
        var currentCell = 1;
        var ietype = 0;

        function NextRowCell() {
            var rows = document.getElementById("myTable").rows;
            var valueTr = rows[rowsIdex];
            if (valueTr) {
                var valueTd = valueTr.cells[currentCell];
                if (valueTd) {
                    var input = valueTd.getElementsByTagName('input');
                    if (input && input.length > 0) {
                        input[0].focus();
                        input[0].select();
                    }
                }
            }
//            alert(rowsIdex+"_"+currentCell);
            if (rowsIdex == rows.length - 1) rowsIdex = 0;
            if (rowsIdex <= 0) rowsIdex = 0;
            if (currentCell <= 1) currentCell = 4;
            if (currentCell >= 5) currentCell = 1;
        }

        function OnKeyDown(evt) {
            key = (ietype) ? e.which : event.keyCode
            // Arrow keys 
            //            alert(key);
            if (key == 37) moveLeft();
            if (key == 38) moveTop();
            if (key == 39) moveRight();
            if (key == 40) moveDown();

        }

        function moveLeft() {
            currentCell--;
            NextRowCell();
        }
        function moveTop() {
            rowsIdex--;
            NextRowCell();
        }
        function moveRight() {
            currentCell++;
            NextRowCell();
        }
        function moveDown() {
            rowsIdex++;
            NextRowCell();
        }
        function BindKeyDownEvent() {
            ietype = (document.layers) ? 1 : 0; // Determine the type of browser 
            var content = document.getElementById("divContent");
            //            content.onkeydown = OnKeyDown; // Setting key events 
            document.onkeydown = OnKeyDown; // Setting key events 
        }
        window.onload = BindKeyDownEvent;

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="toolBar">
        <div id="top">
            <asp:Button ID="Btn_Save" runat="server" Text=" Save " OnClick="Btn_Save_Click" />
            <asp:Button ID="Btn_Return" runat="server" Text=" Return " OnClick="Btn_Return_Click" />
            <asp:Button ID="Btn_Close" runat="server" Text=" Shut down " OnClick="Btn_Close_Click" Visible="false" />
        </div>
    </div>
    <div style="width: 100%; height: auto; display: table; margin-top: 20px;" id="divContent">
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
</asp:Content>
