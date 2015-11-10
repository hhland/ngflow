<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.GetTask" Codebehind="GetTask.ascx.cs" %>
	<script language="JavaScript" src="./../Comm/JScript.js"></script>
   <script   type="text/javascript">
       function Tackback(fk_flow, fk_node, toNode, workid) {
           if (window.confirm(' Are you sure you want to perform retrieval operations ?') == false)
               return;
           var url = 'GetTask.aspx?DoType=Tackback&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&ToNode=' + toNode + '&WorkID=' + workid;
           window.location.href = url;
           //  var v = window.showModalDialog(url, 'sd', 'dialogHeight: 400px; dialogWidth: 500px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
           // window.location.href = window.location.href;
       }
    </script>