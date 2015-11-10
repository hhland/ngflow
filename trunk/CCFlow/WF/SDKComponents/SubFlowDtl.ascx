<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubFlowDtl.ascx.cs" Inherits="CCFlow.WF.SDKComponents.SubFlowDtl" %>
<script type="text/javascript">
    function Del(fk_flow, workid) {
        if (window.confirm(' Are you sure you want to delete it ?') == false)
            return;
    }
</script>
<div id="DelMsg" />