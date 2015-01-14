using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.DA;
using BP.En;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_MapDefDtlCCForm : BP.Web.WebPage
    {
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_MapDtl
        {
            get
            {
                return this.Request.QueryString["FK_MapDtl"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //MapDtl dtl = new MapDtl();
            //dtl.No = this.FK_MapDtl;

            //if (dtl.RetrieveFromDBSources() == 0)
            //{
            //    dtl.FK_MapData = this.FK_MapData;
            //    dtl.Name = this.FK_MapData;
            //    dtl.Insert();
            //    dtl.IntMapAttrs();
            //}

            //this.Pub1.Add("<div class='easyui-layout' data-options='fit:true'>" + Environment.NewLine);
            //this.Pub1.Add("  <div data-options=\"region:'north',noheader:true,split:false\" style='height:30px;overflow-y:hidden'>" + Environment.NewLine);
            //this.Pub1.Add("    <div style='float:left'>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:EditDtl('" + this.FK_MapData + "','" + dtl.No + "')\" class='easyui-linkbutton' data-options=\"iconCls:'icon-edit'\">" +
            //              dtl.Name + "</a>" + Environment.NewLine);
            //this.Pub1.Add("    </div>" + Environment.NewLine);
            //this.Pub1.Add("    <div style='float:right'>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.AddF('" +
            //              dtl.No + "');\" class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> Insert Column </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.AddFGroup('" + dtl.No + "');\" class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> Insert Column Group </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.CopyF('" + dtl.No + "');\" class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> Copy Column </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.HidAttr('" + dtl.No + "');\" class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> Hide Columns </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.DtlMTR('" + dtl.No + "');\" class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> Multi-header </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href='Action.aspx?FK_MapData=" + this.FK_MapDtl + "' class='easyui-linkbutton' data-options=\"iconCls:'icon-add'\"> From table event </a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:DtlDoUp('" + dtl.No + "')\" class='easyui-linkbutton' data-options=\"iconCls:'icon-up'\"></a>" + Environment.NewLine);
            //this.Pub1.Add("      <a href=\"javascript:DtlDoDown('" + dtl.No + "')\" class='easyui-linkbutton' data-options=\"iconCls:'icon-down'\"></a>" + Environment.NewLine);
            //this.Pub1.Add("    </div>" + Environment.NewLine);
            //this.Pub1.Add("    <div style='clear:both'></div>" + Environment.NewLine);
            //this.Pub1.Add("  </div>" + Environment.NewLine);
            //this.Pub1.Add("  <div data-options=\"region:'center',noheader:true\" style='height:30px'>" + Environment.NewLine);
            //this.Pub1.Add("  </div>" + Environment.NewLine);
            //this.Pub1.Add("</div>");

            //this.Pub1.AddTable("width=100% height='100%'  align=left ");
            //this.Pub1.AddTR(" ID='0_0' ");
            //this.Pub1.Add("<TD colspan=4 class=TRSum  ><div style='text-align:left; float:left'><a href=\"javascript:EditDtl('" + this.FK_MapData + "','" + dtl.No + "')\" >" + dtl.Name + "</a></div><div style='text-align:right; float:right'><a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.AddF('" + dtl.No + "');\"><img src='../Img/Btn/New.gif' border=0/> Insert Column 123</a><a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.AddFGroup('" + dtl.No + "');\"><img src='../Img/Btn/New.gif' border=0/> Insert Column Group </a><a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.CopyF('" + dtl.No + "');\"><img src='../Img/Btn/Copy.gif' border=0/> Copy Column </a><a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.HidAttr('" + dtl.No + "');\"><img src='../Img/Btn/Copy.gif' border=0/> Hide Columns </a><a href=\"javascript:document.getElementById('F" + dtl.No + "').contentWindow.DtlMTR('" + dtl.No + "');\"><img src='../Img/Btn/Copy.gif' border=0/> Multi-header </a> <a href='Action.aspx?FK_MapData=" + this.FK_MapDtl + "' > From table event </a><a href=\"javascript:DtlDoUp('" + dtl.No + "')\" ><img src='../Img/Btn/Up.gif' border=0/></a> <a href=\"javascript:DtlDoDown('" + dtl.No + "')\" ><img src='../Img/Btn/Down.gif' border=0/></a></div></td>");
            //this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.Add("<TD colspan=4 ID='TD" + dtl.No + "' height='50px'   align=left>");

            //string src = "MapDtlDe.aspx?DoType=Edit&FK_MapData=" + this.FK_MapData + "&FK_MapDtl=" + dtl.No;
            //this.Pub1.Add("<iframe ID='F" + dtl.No + "' frameborder=0 style='align:left;padding:0px;border:0px;'  leftMargin='0'  topMargin='0' src='" + src + "' width='100%' height='400'  /></iframe>");

            //this.Pub1.AddTDEnd();
            //this.Pub1.AddTREnd();

            //this.Pub1.AddTableEnd();

        }
    }
}