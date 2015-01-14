//added by liuxc,2014-12-1
// This file can be used to store EasyUI The public JS Method , Gave suggestions JS Methods with comments ,Demo As follows 

function OpenEasyUiDialog(url, iframeId, dlgTitle, dlgWidth, dlgHeight, dlgIcon, showBtns, okBtnFunc, okBtnFuncArgs, dockObj) {
    ///<summary> Use EasyUiDialog Open a page </summary>
    ///<param name="url" type="String"> Page link </param>
    ///<param name="iframeId" type="String"> Nesting url Page iframe的id,在okBtnFunc中, By document.getElementById('eudlgframe').contentWindow Get this page , The method then call the page , Such as access to the selected value </param>
    ///<param name="dlgTitle" type="String">Dialog Title </param>
    ///<param name="dlgWidth" type="int">Dialog Width </param>
    ///<param name="dlgHeight" type="int">Dialog Height </param>
    ///<param name="dlgIcon" type="String">Dialog Icon , Must be a style class</param>
    ///<param name="showBtns" type="Boolean">Dialog Is displayed below [ Determine ][ Cancel ] Push button , If the display , The back okBtnFunc Parameters to fill </param>
    ///<param name="okBtnFunc" type="Function"> Click [ Determine ] Method button calls </param>
    ///<param name="okBtnFuncArgs" type="Object">okBtnFunc Parameters used in the method </param>
    ///<param name="dockObj" type="Object">Dialog Bound dom Object , With this dom Dimensional change object has changed ,如:document.getElementById('mainDiv')</param>

    var dlg = $('#eudlg');
    var isTheFirst;

    if (dlg.length == 0) {
        isTheFirst = true;
        var divDom = document.createElement('div');
        divDom.setAttribute('id', 'eudlg');
        document.body.appendChild(divDom);
        dlg = $('#eudlg');
        dlg.append("<iframe frameborder='0' src='' scrolling='auto' id='" + iframeId + "' style='width:100%;height:100%'></iframe>");
    }

    // Positioning the outer container handling size change events 
    if (dockObj != null && dockObj != undefined) {
        var dobj = $(dockObj);

        dlgWidth = dobj.innerWidth() - 20;
        dlgHeight = dobj.innerHeight() - 20;

        if (isTheFirst) {
            $(window).resize(function () {
                var obj = $(this);

                $('#eudlg').dialog('resize', {
                    width: obj.innerWidth() - 20,
                    height: obj.innerHeight() - 40
                });
            });
        }
    }

    dlg.dialog({
        title: dlgTitle,
        width: dlgWidth,
        height: dlgHeight,
        iconCls: dlgIcon,
        resizable: true,
        modal: true
    });

    if (showBtns) {
        dlg.dialog({
            buttons: [{
                text: ' Determine ',
                iconCls: 'icon-save',
                handler: function () {
                    okBtnFunc(okBtnFuncArgs)
                    dlg.dialog('close');
                    $('#' + iframeId).attr('src', '');
                }
            }, {
                text: ' Cancel ',
                iconCls: 'icon-cancel',
                handler: function () {
                    dlg.dialog('close');
                    $('#' + iframeId).attr('src', '');
                }
            }]
        });
    }
    else {
        dlg.dialog({
            buttons: null,
            onClose: function () {
                dlg.find("iframe").attr('src', '');
            }
        });
    }

    dlg.dialog('open');
    $('#' + iframeId).attr('src', url);
}