<%@ Page Title="" Language="C#" MasterPageFile="~/SSO/MasterPage.master" AutoEventWireup="true" Inherits="SSO_Home" Codebehind="Home.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Style/main.css" rel="stylesheet" type="text/css" />
    <link href="Style/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {

            var listleft = $("div[id = 'column_left']");
            var listright = $("div[id = 'column_right']");

            $(".column").sortable({
                connectWith: ".column",
                revert: true, //缓冲效果 
                cursor: 'move'
            });

            $(".column").bind("sortcreate", function (event, ui) {
                //alert("sortcreate");
            });
            $(".column").bind("sortstart", function (event, ui) {
                //alert("sortstart");
            });
            $(".column").bind("sort", function (event, ui) {
                //alert("sort");
            });
            $(".column").bind("sortchange", function (event, ui) {
                //alert("sortchange");
            });
            $(".column").bind("sortbeforestop", function (event, ui) {
                //alert("sortbeforestop");
            });
            $(".column").bind("sortstop", function (event, ui) {

                var new_order_left = []; //左栏布局
                var new_order_right = []; //右栏布局

                listleft.children(".portlet").each(function () {
                    new_order_left.push(this.title);
                });
                listright.children(".portlet").each(function () {
                    new_order_right.push(this.title);
                });

                var newleftid = new_order_left.join(',');
                var newrightid = new_order_right.join(',');

                var moduleOrder = newleftid + ":" + newrightid;

                $.ajax({
                    type: "POST",
                    url: "ashx/SetCustomerSetting.ashx",
                    data: "moduleOrder=" + moduleOrder
                });

            });


            $(".column").bind("sortupdate", function (event, ui) {
                //alert("sortupdate");
            });

            $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
			.find(".portlet-header")
				.addClass("ui-widget-header ui-corner-all")
				.prepend("<span class='ui-icon ui-icon-minusthick'></span>")
				.end()
			.find(".portlet-content");

            $(".portlet-header .ui-icon").click(function () {
                $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
                $(this).parents(".portlet:first").find(".portlet-content").toggle();
            });

            $(".column").disableSelection();

            $.ajax({
                type: "POST",
                url: "ashx/GetCustomerSetting.ashx",
                success: function (list) {

                    if (list == "") {
                        list = "1,2,3,4:5,6,7,8";
                    }
                    var listnode = $(".column").find(".portlet");
                    listleft.children(".portlet").detach();
                    listright.children(".portlet").detach();
                    var leftNo = list.split(':')[0].split(',');
                    var rightNo = list.split(':')[1].split(',');
                    //alert(leftNo + rightNo);

                    $.each(leftNo, function (li, lobj) {
                        $.each(listnode, function (i, obj) {
                            if (lobj == obj.title)
                                listleft.append(obj);
                        });
                    });

                    $.each(rightNo, function (ri, robj) {
                        $.each(listnode, function (i, obj) {
                            if (robj == obj.title)
                                listright.append(obj);
                        });
                    });
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="InfoPush">
        <ul>
            <%foreach (BP.GPM.InfoPush pl in InfoPushs)
              {%>
            <li><a href="<%=pl.Url %>">
                <img alt="" src="<%=pl.ICON%>" width="16px" height="16px" border="0" />
                <%=pl.Name%>(<%=GetNum(pl)%>)</a>&nbsp;</li>
            <% } %>
        </ul>
    </div>
    <div>
    
    </div>
</asp:Content>
