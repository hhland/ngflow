using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using BP.Port;
using BP.GPM;

public partial class SSO_InfoBar : BP.Web.UC.UCBase
{
    public string FK_Bar
    {
        get
        {
            return this.Request.QueryString["FK_Bar"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.DoType)
        {
            case "Setting":
                //this.InfoPush();
                this.BindSetting();
                break;
            case "Up":
                BarEmp be = new BarEmp();
                be.MyPK= WebUser.No+"_"+this.FK_Bar ;
                be.Retrieve();
                be.DoUp();
                this.BindSetting();
                break;
            case "Down":
                BarEmp be1 = new BarEmp();
                be1.MyPK = WebUser.No + "_" + this.FK_Bar;
                be1.Retrieve();
                be1.DoDown();
                this.BindSetting();
                break;
            case "Hid":
            case "Show":
                BarEmp be2 = new BarEmp();
                be2.MyPK = WebUser.No + "_" + this.FK_Bar;
                be2.Retrieve();
                be2.DoHidShow();
                this.BindSetting();
                break;
            case "Disktop":
            case "":
            default:
                //this.InfoPush();
                this.Disktop();
                break;
        }
    }

    public void Disktop()
    {
        /*获取到该人员可以显示的infobar. 使用矩阵输出。*/
        #region 开始 [  信息发布 ] 的矩阵输出
        Bars ens = new Bars();
        ens.RetrieveAll();

        int cols = BP.Sys.GloVars.GetValByKeyInt("ColsOfSSO", 3);
        BarEmps bes = new BarEmps();
        bes.InitMyBars();

        bes.Retrieve(BarEmpAttr.FK_Emp, WebUser.No, BarEmpAttr.Idx);

        this.Add("<table width='100%'>");
        int idx = -1;
        bool is1 = false;
        foreach (BarEmp be in bes)
        {
            Bar en = ens.GetEntityByKey(BarAttr.No, be.FK_Bar) as Bar;
            if (en == null)
            {
                be.Delete();
                continue;
            }
            else
            {
                if (be.IsShow == false)
                    continue;
            }

            idx++;
            if (idx == 0)
                is1 = this.AddTR(is1);

            #region 输出bar信息.
            this.AddTDBegin();
            this.Add("<table class='info_table' cellpadding='0' cellspacing='0'>");
            this.Add("<tr class='info_img'><td height='25' ><span style='float:left;'><img src='Img/icon_keyword.gif' width='16' height='16' /></span>");
            this.Add("<span style='float:left;'>");
            this.Add(en.Title);
            this.Add("</span>");
            this.Add("<span style='float:right;'>");
            this.Add("<a href='" + en.MoreUrl+ "'>");
            this.Add("<img src='Img/more.gif' width='29' height='11' border='0' /></a>&nbsp;&nbsp;");
            this.Add("</span><span style='clear:both;'></span>");
            this.Add("</td></tr>");
            this.Add("<tr>");
            this.Add("<td height='150' style='overflow:hidden;width:210px; padding-left:10px;padding-right:10px; vertical-align:top;'>");
            this.Add(en.Doc);
            this.Add("</td>");
            this.Add("</tr>");
            this.Add("</table>");
            //this.Add(@"<h5 style='background:url(Img/back.png) repeat-x;'><span style='float:left' height='38px'><b>"
            //    + en.Title
            //    + "</b></span> <span style='float:right; font-size:12px;'><a  style='text-decoration: none' href='"
            //    + en.MoreUrl + "'>"
            //    + en.MoreLab + "</a></span></h5>");




            this.AddTDEnd();
            #endregion 输出信息.

            if (idx == cols - 1)
            {
                idx = -1;
                this.AddTREnd();
            }
        }

        while (idx != -1)
        {
            idx++;
            if (idx == cols - 1)
            {
                idx = -1;
                this.AddTD();
                this.AddTREnd();
            }
            else
            {
                this.AddTD();
            }
        }
        this.AddTableEnd();

        #endregion 结束  [  信息发布 ]  矩阵输出

    }
    /// <summary>
    /// 桌面
    /// </summary>
    public void DisktopV2()
    {
        /*获取到该人员可以显示的infobar. 使用矩阵输出。*/
        Bars ens = new Bars();
        ens.RetrieveAll();

        int cols = BP.Sys.GloVars.GetValByKeyInt("ColsOfSSO", 3);
        BarEmps bes = new BarEmps();
        bes.InitMyBars();

        bes.Retrieve(BarEmpAttr.FK_Emp, WebUser.No, BarEmpAttr.Idx);
        string js = "<javascript>";
        js += " \t\n CDrag.database.json = [";

        foreach (BarEmp be in bes)
        {
            Bar en = ens.GetEntityByKey(BarAttr.No, be.FK_Bar) as Bar;
            if (en == null)
            {
                be.Delete();
                continue;
            }
            else
            {
                if (be.IsShow == false)
                    continue;
            }
        }

        //  string title="";
        //  js+="\t\n { id: \"m_1_1\", title: \""+be.tit+"\", className: "News", src: "News" },";
        //  js+= @"<h4 style='background:url(Img/back.png) repeat-x;'><span style='float:left' height='38px'><b>" 
        //      + en.Title 
        //      + "</b></span> <span style='float:right; font-size:12px;'><a  style='text-decoration: none' href='" 
        //      + en.MoreUrl + "'>" 
        //      + en.MoreLab + "</a></span></h4>");
        //  this.Add(en.Doc);
        //js += " ]; <javascript>";
    }

    #region 设置消息显示的顺序与位置
    /// <summary>
    /// 设置消息显示的顺序与位置
    /// </summary>
    public void BindSetting()
    {
        //this.AddTable();
        this.Add("<table class='InfoBarT'>");
        //this.AddTR();
        this.Add("<tr id='title'>");
        this.AddTDTitle("序号");
        this.AddTDTitle("编号");
        this.AddTDTitle("名称");
        this.AddTDTitle("操作");
        this.AddTDTitle("操作");
        this.AddTDTitle("操作");
        this.AddTREnd();

        Bars ens = new Bars();
        ens.RetrieveAll();

        BarEmps pss = new BarEmps();
        pss.Retrieve(BarEmpAttr.FK_Emp, WebUser.No, BarEmpAttr.Idx);

        int idx = 0;
        foreach (BarEmp be in pss)
        {
            Bar en = ens.GetEntityByKey(BarEmpAttr.No, be.FK_Bar) as Bar;
            idx++;
            this.AddTR("style='border: 1px solid #C2D5E3; text-align:center;color: #336699;'");
            this.AddTDIdx(idx);
            this.AddTD(en.No);
            this.AddTD(en.Name);

            this.AddTD("<a href='" + this.PageID + ".aspx?DoType=Up&FK_Bar=" + en.No + "' >上移</a>");
            this.AddTD("<a href='" + this.PageID + ".aspx?DoType=Down&FK_Bar=" + en.No + "' >下移</a>");
            if (en.IsDel == true)
            {
                if (be.IsShow)
                    this.AddTD("<a href='" + this.PageID + ".aspx?DoType=Hid&FK_Bar=" + en.No + "' >隐藏</a>");
                else
                    this.AddTD("<a href='" + this.PageID + ".aspx?DoType=Show&FK_Bar=" + en.No + "' >显示</a>");
            }
            else
            {
                this.AddTD();
            }
            this.AddTREnd();
        }
        this.AddTableEnd();
    }
    #endregion
}