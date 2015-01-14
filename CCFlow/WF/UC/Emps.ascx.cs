using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Port;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class Emps : BP.Web.UC.UCBase3
    {
        public void GenerAllImg()
        {
            BP.WF.Port.WFEmps empWFs = new BP.WF.Port.WFEmps();
            empWFs.RetrieveAll();

            foreach (BP.WF.Port.WFEmp emp in empWFs)
            {
                if (System.IO.File.Exists(BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.No + ".JPG")
                    || System.IO.File.Exists(BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.Name + ".JPG"))
                {
                    continue;
                }

                try
                {
                    string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\T.JPG";
                    string pathMe = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.No + ".JPG";
                    File.Copy(BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\Templete.JPG",
                        path, true);


                    string fontName = " Times New Roman ";
                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    Font font = new Font(fontName, 15);
                    Graphics g = Graphics.FromImage(img);
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);// Text 
                    g.DrawString(emp.Name, font, drawBrush, 3, 3);

                    try
                    {
                        File.Delete(pathMe);
                    }
                    catch
                    {
                    }
                    img.Save(pathMe);
                    img.Dispose();
                    g.Dispose();

                    File.Copy(pathMe,
                    BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.Name + ".JPG", true);
                }
                catch
                {

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Empleyes";
            if (WebUser.IsWap)
            {
                this.BindWap();
                return;
            }

            string sql = "SELECT a.No,a.Name, b.Name as DeptName FROM Port_Emp a, Port_Dept b WHERE a.FK_Dept=b.No ORDER BY a.FK_Dept ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            BP.WF.Port.WFEmps emps = new BP.WF.Port.WFEmps();
            if (this.DoType != null)
                emps.RetrieveAllFromDBSource();
            else
                emps.RetrieveAllFromDBSource();

            this.AddTable("width=100% align=left");
            this.AddCaption(" Member ");
            this.AddTR();
            this.AddTDTitle("IDX");
            this.AddTDTitle(" Department ");
            this.AddTDTitle(" Staff ");
            this.AddTDTitle("Tel");
            this.AddTDTitle("Email");
            this.AddTDTitle(" Post "); // <a href=Emps.aspx?DoType=1> Refresh </a> ");
            this.AddTDTitle(" Signature ");
            if (WebUser.No == "admin")
                this.AddTDTitle(" Order ");

            if (this.DoType != null)
            {
                BP.WF.Port.WFEmp.DTSData();
                this.GenerAllImg();
            }
            this.AddTREnd();

            string keys = DateTime.Now.ToString("MMddhhmmss");
            string deptName = null;
            int idx = 0;

            EmpStations ess = new EmpStations();
            ess.RetrieveAll();

            foreach (DataRow dr in dt.Rows)
            {
                string fk_emp = dr["No"].ToString();
                if (fk_emp == "admin")
                    continue;

                idx++;
                if (dr["DeptName"].ToString() != deptName)
                {
                    deptName = dr["DeptName"].ToString();
                    this.AddTRSum();
                    this.AddTDIdx(idx);
                    this.AddTD(deptName);
                }
                else
                {
                    this.AddTR();
                    this.AddTDIdx(idx);
                    this.AddTD();
                }

                this.AddTD(fk_emp + "-" + dr["Name"]);

                BP.WF.Port.WFEmp emp = emps.GetEntityByKey(fk_emp) as BP.WF.Port.WFEmp;
                if (emp != null)
                {
                    //this.AddTD(emp.TelHtml);
                    //this.AddTD(emp.EmailHtml);

                    this.AddTD();
                    this.AddTD();

                    string stas = "";
                    foreach (EmpStation es in ess)
                    {
                        if (es.FK_Emp != emp.No)
                            continue;
                        stas += es.FK_StationT + ",";
                    }
                    this.AddTD(stas);
                }
                else
                {
                    this.AddTD("");
                    this.AddTD("");
                    this.AddTD("");
                    //break;
                }

                this.AddTD("<img src='../DataUser/Siganture/" + fk_emp + ".jpg' border=1 onerror=\"this.src='../DataUser/Siganture/UnName.jpg'\"/>");
                if (WebUser.No == "admin")
                {
                    this.AddTD("<a href=\"javascript:DoUp('" + emp.No + "','" + keys + "')\" ><img src='Img/Btn/Up.gif' border=0 /></a>-<a href=\"javascript:DoDown('" + emp.No + "','" + keys + "')\" ><img src='Img/Btn/Down.gif' border=0 /></a>");
                }
                this.AddTREnd();
            }
            this.AddTableEnd();
        }

        public void BindWap()
        {
            this.AddTable("align=left width=100%");
            this.AddTR();
            this.AddTD("colspan=4 align=left class=FDesc", "<a href='Home.aspx'><img src='Img/Home.gif' border=0/>Home</a> -  Member ");
            this.AddTREnd();

            BP.Port.Depts depts = new BP.Port.Depts();
            depts.RetrieveAllFromDBSource();

            BP.WF.Port.WFEmps emps = new BP.WF.Port.WFEmps();
            emps.RetrieveAllFromDBSource();

            int idx = 0;
            foreach (BP.Port.Dept dept in depts)
            {
                this.AddTRSum();
                this.AddTD("colspan=4", dept.Name);
                this.AddTREnd();
                foreach (BP.WF.Port.WFEmp emp in emps)
                {
                    if (emp.FK_Dept != dept.No)
                        continue;

                    idx++;
                    this.AddTR();
                    this.AddTD(idx);
                    this.AddTD(emp.Name);
                    this.AddTD(emp.Tel);
                    this.AddTD(emp.Stas);
                    this.AddTREnd();
                }
            }
            this.AddTableEnd();
        }
    }

}