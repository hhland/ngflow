using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using BP.GPM;

namespace GMP2.SSO
{
    public partial class Menu : BP.Web.UC.UCBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DesktopContent();
        }
        /// <summary>
        /// 创建页面
        /// </summary>
        public void DesktopContent()
        {
            Link link = new Link();
            DataTable dtlink = link.RunSQLReturnTable("SELECT  GPM_Link.*,GPM_LinkSort.Name AS SortName FROM GPM_Link,GPM_LinkSort WHERE GPM_LinkSort.No = GPM_Link.FK_Sort ORDER BY GPM_LinkSort.No");

            string FK_Sort = "";
            string strtd = "";
            if (dtlink != null && dtlink.Rows.Count > 0)
            {
                int icount = 0;
                foreach (DataRow dr in dtlink.Rows)
                {
                    if (FK_Sort == dr["FK_Sort"].ToString())//类别相同
                    {
                        //继续添加表格
                        this.Add("<tr><td align='center'><img src='Img/let.jpg' width='5' height='5' /></td><td height='20'>");
                        strtd = "<a    target='_blank'  href='" + dr["Url"].ToString() + "'>" + dr["Name"].ToString() + "</a>";
                        this.Add(strtd);
                        this.Add("</td></tr>");
                    }
                    else
                    {
                        //添加table
                        if (FK_Sort != null && FK_Sort != "" && icount < dtlink.Rows.Count)
                        {
                            this.Add("</table></td></tr></table>");
                            this.Add("<table width='255' border='0' align='center' cellspacing='0'><tr> <td height='5'> </td> </tr></table>");
                        }

                        this.Add("<table width='255' border='0' cellpadding='0' cellspacing='0' class='border'>");
                        this.Add("<tr><td height='25' bgcolor='#D6E8FF'>");
                        this.Add("<span class='font_01'>&nbsp;<img src='Img/up.gif' width='11' height='11' /></span>");
                        this.Add(dr["SortName"].ToString());
                        this.Add("</td></tr><tr><td>");
                        this.Add("<table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                        this.Add("<tr> <td width='11%'></td> <td width='89%' height='5'></td> </tr>");
                        this.Add("<tr> <td align='center'><img src='Img/let.jpg' width='5' height='5' /></td>");
                        this.Add("<td width='100%' height='20'>");
                        strtd = "<a  target='_blank'  href='" + dr["Url"].ToString() + "'>" + dr["Name"].ToString() + "</a></td></tr>";
                        this.Add(strtd);

                    }
                    FK_Sort = dr["FK_Sort"].ToString();
                    icount++;
                }
                this.Add("</table></td></tr></table>");
            }
            else
            {
                this.Add("<table width='255' border='0' cellpadding='0' cellspacing='0' class='border'>");
                this.Add("<tr>");
                this.Add("<td height='25' bgcolor='#D6E8FF'>");
                this.Add("<span class='font_01'>&nbsp;<img src='Img/up.gif' width='11' height='11' /></span>");
                this.Add("超链接");
                this.Add("</td>");
                this.Add("</tr>");
                this.Add("<tr>");
                this.Add("<td>");
                this.Add("<table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                this.Add("<tr>");
                this.Add("<td width='11%'>");
                this.Add("</td>");
                this.Add("<td width='89%' height='5'>");
                this.Add("</td>");
                this.Add("</tr>");

                this.Add("<tr>");
                this.Add("<td align='center'>");
                this.Add("<img src='Img/let.jpg' width='5' height='5' />");
                this.Add("</td>");
                this.Add("<td width='100%' height='20'>");
                this.Add("<a target='_blank' href='http://www.ccflow.org'>济南驰骋信息技术有限公司</a>");
                this.Add("</td>");
                this.Add("</tr>");

                this.Add("<tr>");
                this.Add("<td height='5' colspan='2'>");
                this.Add("</td>");
                this.Add("</tr>");

                this.Add("</table></td>");
                this.Add("</tr>");
                this.Add("</table>");
            }
        }
    }
}