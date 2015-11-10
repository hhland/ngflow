using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.GPM;

public partial class SSO_InfoPush : BP.Web.UC.UCBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.AddTable("width='75%' border='0' align='left' cellspacing='0'");
        this.AddTR();
        string Tools = "";
        foreach (BP.GPM.InfoPush pl in InfoPushs)
        {
            Tools += "&nbsp;|&nbsp;<a class='menuLink' href='" + pl.Url + "' target='_blank'>" + pl.Name + "(" + GetNum(pl) + ")</a>";
        }
        //this.Add("<strong class='font_01'>&nbsp;&nbsp;<a href='Index.aspx'>首页</a> | 新邮件(13) | 系统消息(10) | 待办工作(0) | 未读新闻(20) | 未读公告(0)| 在途工作(4)</strong>");
        this.Add("<td><strong class='font_01'>&nbsp;&nbsp;<a class='menuLink' href='Default.aspx'>首页</a> " + Tools + "</strong></td>");
        
        this.AddTREnd();
        this.AddTableEnd();

        this.AddTable("width='25%' border='0' cellspacing='0'");
        this.AddTR();
        this.Add("<td align='right'><strong class='font_01'>");
        this.Add("<a class='menuLink' href='Loginin.aspx'>重登录</a> | ");
        this.Add("<a class='menuLink' href='STemSettingPage.aspx'>密码设置 </a> | ");
        //this.Add("<a class='menuLink' href='STemSettingPage.aspx'>密码修改 </a> | ");
        this.Add("<a class='menuLink' href='InfoBarSetting.aspx?DoType=Setting'>信息块设置 </a>");
        this.Add("&nbsp;&nbsp;</strong></td>");
        this.AddTREnd();
        this.AddTableEnd();
    }
    public InfoPushs InfoPushs
    {
        get
        {
            BP.GPM.InfoPushs pls = new InfoPushs();
            pls.RetrieveAll("idx");            
            return pls;
        }
    }
    public int GetNum(InfoPush pa)
    {
        string sql = pa.GetSQL;
        BP.DA.Paras p = new BP.DA.Paras("user", BP.Web.WebUser.No);
        p.SQL = sql;
        
        int num = BP.DA.DBAccess.RunSQLReturnValInt(p);
        return num;
    }
}