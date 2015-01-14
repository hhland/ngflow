using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class RenameFieldsName : System.Web.UI.Page
    {
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("IDX");

            this.Pub1.AddTDTitle(" Fields Chinese name - Original ");
            this.Pub1.AddTDTitle(" Fields Chinese name ");

            this.Pub1.AddTDTitle(" English name field - Original ");
            this.Pub1.AddTDTitle(" English name field ");

            this.Pub1.AddTDTitle(" Minimum length ");
            this.Pub1.AddTDTitle(" The maximum length ");
            //add by myflow- Dalian  2014-08-01
            this.Pub1.AddTDTitle(" Sequence ");
            //end
            this.Pub1.AddTREnd();

            MapAttrs attrs = new MapAttrs(this.FK_MapData);
            int idx = 0;
            foreach (MapAttr  attr in attrs)
            {
                if (attr.IsPK)
                    continue;

                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);

                // Description .
                this.Pub1.AddTD(attr.Name);
                TextBox tb = new TextBox();
                tb.ID = "TB_Name_" + attr.KeyOfEn;
                tb.Text = attr.Name;
                this.Pub1.AddTD(tb);

                //  Field .
                this.Pub1.AddTD(attr.KeyOfEn);
                  tb = new TextBox();
                tb.ID = "TB_"+attr.KeyOfEn;
                tb.Text = attr.KeyOfEn;
                this.Pub1.AddTD(tb);


                // Minimum length .
                tb = new TextBox();
                tb.ID = "TB_MinLen_" + attr.KeyOfEn;
                tb.Columns = 3;
                tb.Text = attr.MinLen.ToString();
                this.Pub1.AddTD(tb);

                // The maximum length .
                tb = new TextBox();
                tb.ID = "TB_MaxLen_" + attr.KeyOfEn;
                tb.Text = attr.MaxLen.ToString();
                tb.Columns = 3;
                this.Pub1.AddTD(tb);
                //add by myflow- Dalian  2014-08-01
                // Sequence .
                tb = new TextBox();
                tb.ID = "TB_IDX_" + attr.KeyOfEn;
                tb.Text = attr.IDX.ToString();
                tb.Columns = 3;
                this.Pub1.AddTD(tb);
                //end

                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            MapAttrs attrs = new MapAttrs(this.FK_MapData);
            int idx = 0;
            string err = "";
            string info = "";
            foreach (MapAttr attr in attrs)
            {
                if (attr.IsPK || attr.KeyOfEn=="Title")
                    continue;

                try
                {
                    TextBox tb = this.Pub1.GetTextBoxByID("TB_" + attr.KeyOfEn);
                    string filed = tb.Text.Trim();

                    tb = this.Pub1.GetTextBoxByID("TB_Name_" + attr.KeyOfEn);
                    string name = tb.Text.Trim();

                    int minLen = int.Parse(this.Pub1.GetTextBoxByID("TB_MinLen_" + attr.KeyOfEn).Text);
                    int maxLen = int.Parse(this.Pub1.GetTextBoxByID("TB_MaxLen_" + attr.KeyOfEn).Text);
                    idx = int.Parse(this.Pub1.GetTextBoxByID("TB_IDX_" + attr.KeyOfEn).Text);
                    if (attr.KeyOfEn != filed)
                    {
                        attr.Delete();

                        attr.KeyOfEn = filed;
                        attr.Name = name;
                        attr.MaxLen = maxLen;
                        attr.MinLen = minLen;
                        //add by myflow- Dalian  2014-08-01
                        attr.IDX = idx;
                        //end
                        attr.MyPK = attr.FK_MapData + "_" + filed;
                        attr.Insert();

                        info += "@ Field :" + attr.KeyOfEn + "," + attr.Name + " Rename success .";
                        continue;
                    }

                    bool isChange = false;
                    if (attr.Name != name)
                        isChange = true;

                    if (attr.MinLen != minLen)
                        isChange = true;

                    if (attr.MaxLen != maxLen)
                        isChange = true;
                    if (attr.IDX != idx)
                        isChange = true;
                    if (isChange == false)
                        continue;

                    attr.MaxLen = maxLen;
                    attr.MinLen = minLen;
                    attr.Name = name;
                    //add by myflow- Dalian  2014-08-01
                    attr.IDX = idx;
                    //end
                    attr.Update();
                    info += "@ Field :" + attr.KeyOfEn + "," + attr.Name + " Change Success .";

                }
                catch (Exception ex)
                {
                    err += "@ Field :" + attr.KeyOfEn + "," + attr.Name + "; Failed to save :" + ex.Message;
                }
            }

            if (string.IsNullOrEmpty(info) == false)
                this.Pub2.AddFieldSet(" Information saved successfully ", info);

            if (string.IsNullOrEmpty(err) == false)
                this.Pub2.AddFieldSet(" Failed to save information ", err);
            return;
        }
    }
}