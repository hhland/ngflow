<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="CCFlow.WF.Comm.kindeditor.plugins.map.map" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <style>
		html { height: 100% }
		body { height: 100%; margin: 0; padding: 0; background-color: #FFF }
		#map_canvas { height: 100% }
	</style>

    <%
        object _culture = Session["culture"];
        string culture = _culture == null ? "en-us" : _culture.ToString();
         %>
    <script src="/WF/Scripts/jquery-1.4.1.min.js" ></script>
	<script src="http://maps.googleapis.com/maps/api/js?sensor=false&language=<%=culture %>"></script>
	<script>
	    var map, geocoder;
	    var lang = {};
	    lang["address"] = "<%=GetGlobalResourceLabel("Address.Text") %>";
        lang["comfirm"] = "<%=GetGlobalResourceButton("Comfirm.Text") %>";
        lang["cancel"] = "<%=GetGlobalResourceButton("Cancel.Text") %>";
        lang["search"] = "<%=GetGlobalResourceButton("Search.Text") %>";
        lang["title"] = "<%=GetGlobalResourceTitle("GoogleMap.Text") %>";

        function initLang(){
           var dialog=$(parent.document).find(".ke-dialog-content");
           dialog.find("[name=searchBtn]").val(lang["search"]);
           var footer=dialog.find(".ke-dialog-footer");
           footer.find("input:eq(0)").val(lang["comfirm"]);
           footer.find("input:eq(1)").val(lang["cancel"]);
           var header=dialog.find(".ke-dialog-header");
           header.html(header.html().replace("Google地图",lang["title"]));
           
           dialog.find("span#address").text(lang["address"]);

           dialog.find("#kindeditor_plugin_map_address").keypress(function(event){
               if(event.keyCode==13){
                  search($(this).val());
               }
            });


        }

	    function initialize() {
	        var latlng = new google.maps.LatLng(31.230393, 121.473704);
	        var options = {
	            zoom: 11,
	            center: latlng,
	            disableDefaultUI: true,
	            panControl: true,
	            zoomControl: true,
	            mapTypeControl: true,
	            scaleControl: true,
	            streetViewControl: false,
	            overviewMapControl: true,
	            mapTypeId: google.maps.MapTypeId.ROADMAP
	        };
	        map = new google.maps.Map(document.getElementById("map_canvas"), options);
	        geocoder = new google.maps.Geocoder();
	        geocoder.geocode({ latLng: latlng }, function (results, status) {
	            if (status == google.maps.GeocoderStatus.OK) {
	                if (results[3]) {
	                    parent.document.getElementById("kindeditor_plugin_map_address").value = results[3].formatted_address;
	                }
	            }
	        });

            
            
	    }
	    function search(address) {
	        if (!map) return;
	        geocoder.geocode({ address: address }, function (results, status) {
	            if (status == google.maps.GeocoderStatus.OK) {
	                map.setZoom(11);
	                map.setCenter(results[0].geometry.location);
	                var marker = new google.maps.Marker({
	                    map: map,
	                    position: results[0].geometry.location
	                });
	            } else {
	                alert("Invalid address: " + address);
	            }
	        });
	    }

        initLang();

        $(document).ready(function(){
          initialize();
          
        });
	</script>
</head>
<body  >
    <form id="form1" runat="server">
   
    </form>
    <div id="map_canvas" style="width:100%; height:100%"></div>
</body>
</html>
