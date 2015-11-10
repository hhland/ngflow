using System;
using BP.Sys;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;

public partial class Comm_Sys_GloVar : BP.Web.WebPageAdmin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = "全局变量";
        this.UCSys1.AddTable();
        this.UCSys1.AddCaptionLeft("表单全局变量");
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle("序");
        this.UCSys1.AddTDTitle("变量");
        this.UCSys1.AddTDTitle("变量名称");
        this.UCSys1.AddTDTitle("变量值");
        this.UCSys1.AddTDTitle("操作");
        this.UCSys1.AddTREnd();

        GloVars ens = new GloVars();
        ens.RetrieveAll();
        int i = 0;
        foreach (GloVar en in ens)
        {
            i++;
            this.UCSys1.AddTR();
            this.UCSys1.AddTDIdx(i);
            this.UCSys1.AddTDTitle("变量");
            this.UCSys1.AddTDTitle("变量名称");
            this.UCSys1.AddTDTitle("变量值");
            this.UCSys1.AddTDTitle("操作");
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }
}