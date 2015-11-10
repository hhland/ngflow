using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Web;
using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web.Controls;


public partial class Comm_EnsSimple : WebPage
{
    public new string EnsName = "BP.GE.InfoSorts";
    protected void Page_Load(object sender, EventArgs e)
    {
        Entities ens = ClassFactory.GetEns(EnsName);
        ens.RetrieveAll();
        Entity myen = ens.GetNewEntity;


        this.Label1.Text = this.GenerCaption(myen.EnDesc + myen.EnMap.TitleExt);

        Map map = myen.EnMap;


        this.UCSys1.AddTable();
        this.UCSys1.AddTR();
        Attrs attrs = map.Attrs;
        this.UCSys1.AddTable();
        this.UCSys1.AddTR();
        CheckBox cb = new CheckBox();
        string str1 = "<INPUT id='checkedAll' onclick='selectAll()' type='checkbox' name='checkedAll'>";
        this.UCSys1.AddTDTitle(str1);
        foreach (Attr attr in attrs)
        {
            if (attr.UIVisible == false)
                continue;
            this.UCSys1.AddTDTitle(attr.Desc);
        }
        this.UCSys1.AddTREnd();

        myen.PKVal = "0";
        ens.AddEntity(myen);

        DDL ddl = new DDL();

        int i = 0;
        bool is1 = false;
        foreach (Entity dtl in ens)
        {
            i++;
            if (dtl.PKVal == "0")
            {
                this.UCSys1.AddTRSum();
                this.UCSys1.AddTDIdx("<b>*</b>");
            }
            else
            {
                is1 = this.UCSys1.AddTR(is1, "ondblclick=\"WinOpen( 'UIEn.aspx?EnsName=" + this.EnsName + "&PK=" + dtl.PKVal + "', 'cd' )\"");
                cb = new CheckBox();
                cb.ID = "IDX_" + dtl.PKVal;
                cb.Text = i.ToString();
                this.UCSys1.AddTDIdx(cb);
            }

            foreach (Attr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                if (attr.Key == "OID")
                    continue;

                string val = dtl.GetValByKey(attr.Key).ToString();
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        TB tb = new TB();
                        tb.LoadMapAttr(attr);
                        this.UCSys1.AddTD(tb);
                        tb.ID = "TB_" + attr.Key + "_" + dtl.PKVal;
                        switch (attr.MyDataType)
                        {
                            case DataType.AppMoney:
                            case DataType.AppRate:
                                tb.TextExtMoney = decimal.Parse(val);
                                break;
                            default:
                                tb.Text = val;
                                break;
                        }

                        if (attr.IsNum && attr.IsFKorEnum == false)
                        {
                            if (tb.Enabled)
                            {
                                tb.Attributes["class"] = "TBNum";
                            }
                            else
                            {
                                //   tb.Attributes["onpropertychange"] += "C" + attr.Key + "();";
                                tb.Attributes["class"] = "TBNumReadonly";
                            }
                        }
                        break;
                    case UIContralType.DDL:
                        if (attr.UIIsReadonly)
                        {
                            ddl = new DDL();
                            ddl.LoadMapAttr(attr);
                            ddl.ID = "DDL_" + attr.Key + "_" + dtl.PKVal;
                            this.UCSys1.AddTD(ddl);
                            ddl.SetSelectItem(val);
                        }
                        else
                        {
                            this.UCSys1.AddTD(dtl.GetValRefTextByKey(attr.Key));
                        }
                        break;
                    case UIContralType.CheckBok:
                        cb = new CheckBox();
                        cb.ID = "CB_" + attr.Key + "_" + dtl.PKVal;
                        if (val == "1")
                            cb.Checked = true;
                        else
                            cb.Checked = false;
                        this.UCSys1.AddTDCenter(cb);
                        break;
                    default:
                        break;
                }
            }
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();
    }
}

