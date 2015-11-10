using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.IO;
using BP.GPM;
using BP.Web;

namespace GMP2.SSO
{
    public partial class AppBar : BP.Web.UC.UCBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Disktop();
        }

        public void Disktop()
        {
            /*获取到该人员可以显示的app. 使用矩阵输出。*/
            #region 开始 [ 应用系统 ] 的矩阵输出

            Apps apps = new Apps();
            apps.RetrieveAll();

            string sql = "SELECT a.No,a.Name,a.FK_App,b.Idx FROM V_GPM_EmpMenu a,GPM_App b WHERE a.FK_App=b.No and FK_Emp='" + WebUser.No + "' and MenuType=2 order by CAST(b.Idx as int)";

            DataTable dt_App = BP.DA.DBAccess.RunSQLReturnTable(sql);

            int cols = BP.Sys.GloVars.GetValByKeyInt("ColsOfApp", 2);

            //EmpApps empApps = new EmpApps();
            //empApps.Retrieve(EmpAppAttr.FK_Emp, WebUser.No);


            this.Add("<table border='0' cellspacing='20' cellpadding='0' style='margin:0 auto;width:650px;'>");
            int idx = -1;
            foreach (DataRow row in dt_App.Rows)
            {
                BP.GPM.App en = apps.GetEntityByKey(BP.GPM.AppAttr.No, row["FK_App"]) as BP.GPM.App;
                string url = en == null ? null : en.Url;
                if (string.IsNullOrEmpty(url))
                    continue;

                string imgPath = Server.MapPath("Img");
                imgPath = imgPath + "/" + en.No + ".png";
                if (File.Exists(imgPath))
                {
                    imgPath = "img/" + en.No + ".png";
                }
                else
                {
                    imgPath = "img/laptop.png";
                }

                if (en.SSOType == "1" || en.SSOType == "2")
                    url = "SubForm.aspx?AppNo=" + en.No + "&UserNo=" + WebUser.No + "";

                idx++;
                if (idx == 0)
                    this.AddTR();

                #region 输出bar信息.
                this.AddTDBegin("style='height:100px;width:120px;'");
                string ctrlWay = "_blank";

                this.Add("<table width='100px' border='0' cellspacing='0' cellpadding='0'>");
                this.Add("<tr>");
                this.Add("<td align='center'>");
                this.Add("<div id='appDiv'>");
                this.Add("<a target='" + ctrlWay + "' href='" + url + "'>");
                this.Add("<img alt='' style='width:100px;height:100px;' src='" + imgPath + "' border='0' /></a>");
                this.Add("</div>");
                this.Add("</td>");
                this.Add("</tr>");
                this.Add("<tr>");
                this.Add("<td height='30px' width='120px' align='center'>");
                this.Add("<a target='" + ctrlWay + "' href='" + url + "'>");
                this.Add("<b>");
                this.Add(en.Name);
                this.Add("</b>");
                this.Add("</a>");
                this.Add("</td>");
                this.Add("</tr>");
                this.Add("</table>");

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

            #endregion 结束  [  应用程序 ]  矩阵输出

        }
    }
}