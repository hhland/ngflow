using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Demo;
using BP.Port;

namespace CCFlow.SDKFlowDemo.BPFramework.DataInput
{
    public partial class Cell3DLeft : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region   Organization Dimension Data Sources .
            BP.Pub.YFs ens1 = new BP.Pub.YFs();
            ens1.RetrieveAll();

            BP.Port.Depts ens2 = new BP.Port.Depts();
            ens2.RetrieveAll();

            BP.Port.Emps ens3 = new BP.Port.Emps();
            ens3.RetrieveAll();
            #endregion   Organization Dimension Data Sources .

            //  Organize the data source .
            EmpCents ensData = new EmpCents();
            ensData.RetrieveAll();

            // Start output .
            this.Pub1.AddTable();
            this.Pub1.AddCaption(" Employee attendance score Month ");

            #region   Output Dimensions 1
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Department ");
            this.Pub1.AddTDTitle(" Staff ");

            foreach (BP.Pub.YF en1 in ens1)
            {
                this.Pub1.AddTDTitle(en1.Name);
            }
            this.Pub1.AddTREnd();
            #endregion   Output Dimensions 1


            #region   Output Body 
            foreach (BP.Port.Dept en2 in ens2)
            {
                int numOfD2 = ens3.GetCountByKey(BP.Port.EmpAttr.FK_Dept, en2.No);

                if (numOfD2 == 0)
                    continue; //  If no data source continue.

                this.Pub1.AddTR();
                this.Pub1.AddTD("rowspan=" + numOfD2 , en2.Name);
                int idx = -1;
                foreach (BP.Port.Emp en3 in ens3)
                {
                    if (en3.FK_Dept != en2.No)
                        continue;

                    idx++;
                    if (idx == 0) //  First record .
                    {
                    }
                    else
                    {
                        this.Pub1.AddTR();
                    }

                    this.Pub1.AddTD(en3.Name);
                    foreach (BP.Pub.YF en1 in ens1)
                    {
                        TextBox tb = new TextBox();
                        tb.CssClass = "TBNum";
                        tb.ID = "TB_" + en1.No + "_" + en3.No;
                        EmpCent enData = ensData.GetEntityByKey(EmpCentAttr.FK_Emp, en3.No, EmpCentAttr.FK_NY, en1.No) as EmpCent;
                        if (enData == null)
                        {
                            tb.Text = "0";
                            this.Pub1.AddTD(tb);
                        }
                        else
                        {
                            tb.Text = enData.Cent.ToString();
                            this.Pub1.AddTD(tb);
                        }
                    }

                    this.Pub1.AddTREnd();
                }
                this.Pub1.AddTREnd();
            }
            #endregion   Output Body 
            this.Pub1.AddTableEndWithHR();

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);

            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            //  Organization Dimension Data Sources .
            BP.Pub.YFs ens1 = new BP.Pub.YFs();
            ens1.RetrieveAll();

            BP.Port.Emps ens3 = new BP.Port.Emps();
            ens3.RetrieveAll();

            // Delete save data before .( Be sure to remove accordance with the conditions .)
            BP.DA.DBAccess.RunSQL("DELETE FROM Demo_EmpCent WHERE 1=1 ");

            // Create a blank entity .
            EmpCent enData = new EmpCent();
            foreach (BP.Port.Emp en3 in ens3)
            {
                foreach (BP.Pub.YF en1 in ens1)
                {
                    float val = float.Parse(this.Pub1.GetTextBoxByID("TB_" + en1.No + "_" + en3.No).Text);
                    enData.MyPK = en1.No + "_" + en3.No;
                    enData.Cent = val;
                    enData.FK_Emp = en3.No;
                    enData.FK_NY = en1.No;
                    enData.Insert();  // As it has been deleted in accordance with the conditions , Here is the direct execution insert.
                }
            }
            this.Response.Write(" Saved successfully .");

            //Button btn = sender as Button;
            //btn.Enabled = false;
            //BP.Sys.PubClass.Alert(" Saved successfully !!");
            // this.Response.Redirect("Cell2D.aspx");
        }
    }
}