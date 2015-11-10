using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.En;

public partial class Comm_RefFunc_UIEn : BP.Web.WebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string enName = this.Request.QueryString["EnName"];
        if (enName == null || enName == "")
            enName = this.Request.QueryString["EnsName"];

        if (enName.Contains(".") == false)
        {
            this.Response.Redirect("SysMapEn.aspx?EnsName=" + enName + "&PK=" + this.Request["PK"], true);
            return;
        }
        else
        {
            if (this.IsPostBack == false)
            {
                BP.En.Map map = null;

                string str = this.Request.QueryString["EnName"];
                if (string.IsNullOrEmpty(str) == false)
                {
                    //如果没有相关功能.
                    BP.En.Entity en = ClassFactory.GetEn(enName);
                    map = en.EnMap;
                    int myNum = map.HisRefMethods.Count + map.Dtls.Count + map.AttrsOfOneVSM.Count;
                    if (myNum == 0)
                        this.Response.Redirect("UIEnOnly.aspx?12=2" + this.RequestParas, true);
                }
                else
                {
                    //如果没有相关功能.
                    BP.En.Entity en = ClassFactory.GetEns(enName).GetNewEntity;
                    map = en.EnMap;
                    int myNum = map.HisRefMethods.Count + map.Dtls.Count + map.AttrsOfOneVSM.Count;
                    if (myNum == 0)
                        this.Response.Redirect("UIEnOnly.aspx?12=2" + this.RequestParas, true);
                }
            }
        }
    }
}