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

public partial class Comm_GradeEns : WebPage
{
    #region 属性
    /// <summary>
    /// 分组的键
    /// </summary>
    public string GroupKey
    {
        get
        {
            return ViewState["GK"] as string;
        }
        set
        {
            ViewState["GK"] = value;
        }
    }
    public new string EnsName
    {
        get
        {
            return "BP.GE.DKDirs";
        }
    }
    #endregion 属性


    protected void Page_Load(object sender, EventArgs e)
    {
        #region 按钮
        Button btn = new Button();
        btn = new Button();
        btn.ID = "Btn_Del";
        btn.CssClass = "Btn";
        btn.Text = "删除";
        btn.Attributes["onclick"] = " return confirm('您确认吗？');";
        btn.Click += new EventHandler(btn_Del_Click);
        this.UCSys1.Add(btn);

        btn.ID = "Btn_Save";
        btn.Text = " 保 存 ";
        btn.Click += new EventHandler(btn_Save_Click);
        this.UCSys1.Add(btn);

        btn = new Button();
        btn.ID = "Btn_Insert";
        btn.Text = "增加下级";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_Insert_Click);
        this.UCSys1.Add(btn);

        btn = new Button();
        btn.ID = "Btn_InsertUp";
        btn.Text = "增加同级";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_InsertSameLevel_Click);
        this.UCSys1.Add(btn);
        #endregion 按钮

        #region 获取数据源
        Entities ens = ClassFactory.GetEns(this.EnsName);
        Entity en = ens.GetNewEntity;
        this.Label1.Text = this.GenerCaption(en.EnDesc + en.EnMap.TitleExt);
        QueryObject qo = new QueryObject(ens);
        if (this.GroupKey != null)
        {
            switch (this.GroupKey)
            {
                case "FK_Emp":
                    qo.AddWhere("FK_Emp", WebUser.No);
                    break;
                case "FK_Dept":
                    qo.AddWhere("FK_Dept", WebUser.FK_Dept);
                    break;
                default:
                    throw new Exception("没有判断的GroupKey");
            }
        }
        qo.addOrderBy("GradeNo");
        qo.DoQuery();
        this.BindTree(ens);
        #endregion 获取数据源

        Map map = en.EnMap;
        Attrs attrs = map.Attrs;

        this.UCSys1.AddTable();
        this.UCSys1.AddTR();
        this.UCSys1.AddTDTitle();
        this.UCSys1.AddTDTitle();
        foreach (Attr attr in map.Attrs)
        {
            if (attr.Key == "FK_Emp" || attr.Key == "FK_Dept")
                this.GroupKey = attr.Key;
            if (attr.UIVisible == false)
                continue;
            this.UCSys1.AddTDTitle(attr.Desc);
        }
        this.UCSys1.AddTREnd();

        if (this.RefPK != null)
        {
            en.PKVal = this.RefPK;
            en.RetrieveFromDBSources();
        }

        DDL ddl = new DDL();
        CheckBox cb = new CheckBox();
        foreach (Entity dtl in ens)
        {
            if (this.RefPK != null)
            {
                string pNo = en.GetValStringByKey("GradeNo");
                string dtlNo = dtl.GetValStringByKey("GradeNo");
                if (dtlNo.Length < pNo.Length)
                    continue;

                if (dtlNo.Substring(0, pNo.Length) != pNo)
                    continue;

                if (dtlNo.Length > pNo.Length)
                    continue;
            }

            this.UCSys1.AddTR();
            CheckBox cbC = new CheckBox();
            cbC.ID = "CB_" + dtl.PKVal;
            this.UCSys1.AddTD(cbC);
            this.UCSys1.AddTDBegin();
            this.UCSys1.Add("<a href=\"javascript:DoUp('" + this.EnsName + "','" + dtl.PKVal + "','" + this.RefPK + "');\" ><img src='../Images/Btn/Up.gif' border=0/></a>");
            this.UCSys1.Add("<a href=\"javascript:DoDown('" + this.EnsName + "','" + dtl.PKVal + "','" + this.RefPK + "');\" ><img src='../Images/Btn/Down.gif' border=0/></a>");
            //  this.UCSys1.Add("<a href=\"javascript:DoDel('" + this.EnsName + "','" + dtl.PKVal + "','" + this.RefPK + "');\" ><img src='../Images/Btn/Delete.gif' border=0/></a>");
            //    this.UCSys1.Add("<a href=\"javascript:DoInsert('" + this.EnsName + "','" + dtl.PKVal + "','" + this.RefPK + "');\" ><img src='../Images/Btn/Insert.gif' border=0/></a>");
            this.UCSys1.AddTDEnd();

            foreach (Attr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;

                string val = dtl.GetValByKey(attr.Key).ToString();
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        TB tb = new TB();
                        this.UCSys1.AddTD(tb);
                        tb.LoadMapAttr(attr);
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

                        if (attr.IsNum && attr.MyFieldType == FieldType.Normal)
                        {
                            if (tb.Enabled)
                            {
                                // OnKeyPress="javascript:return VirtyNum(this);"
                                // tb.Attributes["OnKeyDown"] = "javascript:return VirtyNum(this);";
                                // tb.Attributes["onkeyup"] += "javascript:C" + dtl.OID + "();C" + attr.Key + "();";
                                tb.Attributes["class"] = "TBNum";
                            }
                            else
                            {
                                //  tb.Attributes["onpropertychange"] += "C" + attr.Key + "();";
                                tb.Attributes["class"] = "TBNumReadonly";
                            }
                        }
                        break;
                    case UIContralType.DDL:
                        ddl = new DDL();
                        ddl.LoadMapAttr(attr);
                        ddl.ID = "DDL_" + attr.Key + "_" + dtl.PKVal;
                        this.UCSys1.AddTD(ddl);
                        ddl.SetSelectItem(val);
                        break;
                    case UIContralType.CheckBok:
                        cb = new CheckBox();
                        cb.ID = "CB_" + attr.Key + "_" + dtl.PKVal;
                        cb.Text = attr.Desc;
                        if (val == "1")
                            cb.Checked = true;
                        else
                            cb.Checked = false;
                        this.UCSys1.AddTD(cb);
                        break;
                    default:
                        break;
                }
            }
            this.UCSys1.AddTREnd();
        }
        this.UCSys1.AddTableEnd();

        btn = new Button();
        btn.ID = "Btn_Del";
        btn.Text = "批量删除";
        btn.CssClass = "Btn";
        btn.Attributes["onclick"] = "return window.confirm('您确定要删除吗？');";
        btn.Click += new EventHandler(btn_Del_Click);
        this.UCSys1.Add(btn);
    }

    #region 绑定树
    /// <summary>
    /// 绑定树
    /// </summary>
    /// <param name="ens"></param>
    public void BindTree(Entities ens)
    {
        if (ens.Count == 0)
        {
            Entity en = ens.GetNewEntity;
            en.SetValByKey("GradeNo", "01");
            en.SetValByKey("Name", "新建节点");
            en.Insert();
            ens.AddEntity(en);
        }

        for (int i = 0; i < ens.Count; i++)
        {
            Entity dtl = ens[i];
            string no = dtl.GetValStringByKey("GradeNo");
            if (no.Length != 2)
                continue;

            TreeNode tn = new TreeNode();
            tn.Text = dtl.GetValStringByKey("Name");
            tn.Value = dtl.PKVal.ToString();
            tn.NavigateUrl = "GradeEns.aspx?EnsName=" + this.EnsName + "&RefPK=" + dtl.PKVal;
            tn.Target = "_self";

            AddNode(ens, dtl, tn);
            this.TreeView1.Nodes.Add(tn);
        }
    }
    /// <summary>
    /// 增加节点
    /// </summary>
    /// <param name="ens"></param>
    /// <param name="pdtl"></param>
    /// <param name="ptn"></param>
    public void AddNode(Entities ens, Entity pdtl, TreeNode ptn)
    {
        string pNo = pdtl.GetValStringByKey("GradeNo");
        ArrayList al = new ArrayList();
        foreach (Entity en in ens)
        {
            string gradeNo = en.GetValStringByKey("GradeNo");
            if (gradeNo.Length != pNo.Length + 2)
                continue;
            if (gradeNo.Substring(0, pNo.Length) != pNo)
                continue;
            al.Add(en);
        }
        foreach (Entity dtl in al)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dtl.GetValStringByKey("Name");
            tn.Value = dtl.PKVal.ToString();
            tn.NavigateUrl = "GradeEns.aspx?EnsName=" + this.EnsName + "&RefPK=" + dtl.PKVal;
            tn.Target = "_self";

            AddNode(ens, dtl, tn);
            ptn.ChildNodes.Add(tn);
        }
    }
    #endregion 绑定树


    #region 事件
    void btn_Del_Click(object sender, EventArgs e)
    {
        #region 获取数据.
        Entities ens = ClassFactory.GetEns(this.EnsName);
        QueryObject qo = new QueryObject(ens);
        if (this.GroupKey != null)
        {
            switch (this.GroupKey)
            {
                case "FK_Emp":
                    qo.AddWhere("FK_Emp", WebUser.No);
                    break;
                case "FK_Dept":
                    qo.AddWhere("FK_Dept", WebUser.FK_Dept);
                    break;
                default:
                    throw new Exception("没有判断的GroupKey");
            }
        }
        qo.addOrderBy("GradeNo");
        qo.DoQuery();
        #endregion 获取数据.


        Entity en = ens.GetNewEntity;
        if (this.RefPK != null)
        {
            en.PKVal = this.RefPK;
            en.Retrieve();
        }


        #region 获取数据.
        foreach (Entity dtl in ens)
        {
            if (this.RefPK != null)
            {
                string pNo = en.GetValStringByKey("GradeNo");
                string dtlNo = dtl.GetValStringByKey("GradeNo");
                if (dtlNo.Length < pNo.Length)
                    continue;

                if (dtlNo.Substring(0, pNo.Length) != pNo)
                    continue;
            }

            CheckBox cb = this.UCSys1.GetCBByID("CB_" + dtl.PKVal);
            if (cb.Checked == false)
                continue;

            dtl.Delete();
        }
        #endregion 获取数据.

        this.Response.Redirect(this.Request.RawUrl, true);


    }
    void btn_Insert_Click(object sender, EventArgs e)
    {
        Entity en = ClassFactory.GetEns(this.EnsName).GetNewEntity;
        string pNo = "";
        if (this.RefPK != null)
        {
            en.PKVal = this.RefPK;
            en.Retrieve();
            pNo = en.GetValStringByKey("GradeNo");
        }

        int len = pNo.Length + 2;

        string sql = "";
        sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND LEN(GradeNo)=" + len;
        if (en.EnMap.Attrs.Contains("FK_Emp"))
            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND fk_emp='" + WebUser.No + "' AND LEN(GradeNo)=" + len;
        if (en.EnMap.Attrs.Contains("FK_Dept"))
            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND fk_dept='" + WebUser.FK_Dept + "' AND LEN(GradeNo)=" + len;

        int i = BP.DA.DBAccess.RunSQLReturnValInt(sql) + 1;
        Entity enNew = ClassFactory.GetEns(this.EnsName).GetNewEntity;
        string myno = i.ToString();
        if (myno.Length == 1)
            myno = "0" + myno;

        string no = pNo + myno;
        enNew.SetValByKey("GradeNo", no);
        enNew.SetValByKey("Name", "子节点" + i.ToString());

        enNew.SetValByKey("FK_Emp", WebUser.No);
        enNew.SetValByKey("FK_Dept", WebUser.FK_Dept);

        enNew.Insert();
        this.Response.Redirect(this.Request.RawUrl, true);
    }

    void btn_InsertSameLevel_Click(object sender, EventArgs e)
    {
        Entity en = ClassFactory.GetEns(this.EnsName).GetNewEntity;
        string pNo = "";
        if (this.RefPK != null)
        {
            en.PKVal = this.RefPK;
            en.Retrieve();
            pNo = en.GetValStringByKey("GradeNo");
            pNo = pNo.Substring(0, pNo.Length);
        }

        int len = pNo.Length;

        string sql = "";
        sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND LEN(GradeNo)=" + len;
        if (en.EnMap.Attrs.Contains("FK_Emp"))
            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND fk_emp='" + WebUser.No + "' AND LEN(GradeNo)=" + len;
        if (en.EnMap.Attrs.Contains("FK_Dept"))
            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE GradeNo like '" + pNo + "%' AND fk_dept='" + WebUser.FK_Dept + "' AND LEN(GradeNo)=" + len;

        int i = BP.DA.DBAccess.RunSQLReturnValInt(sql) + 1;
        Entity enNew = ClassFactory.GetEns(this.EnsName).GetNewEntity;
        string myno = i.ToString();
        if (myno.Length == 1)
            myno = "0" + myno;

        string no = pNo + myno;
        enNew.SetValByKey("GradeNo", no);
        enNew.SetValByKey("Name", "子节点" + i.ToString());

        enNew.SetValByKey("FK_Emp", WebUser.No);
        enNew.SetValByKey("FK_Dept", WebUser.FK_Dept);

        enNew.Insert();

        this.Response.Redirect(this.Request.RawUrl, true);
    }
  
    void btn_Save_Click(object sender, EventArgs e)
    {
        #region 获取数据.
        Entities ens = ClassFactory.GetEns(this.EnsName);
        QueryObject qo = new QueryObject(ens);
        if (this.GroupKey != null)
        {
            switch (this.GroupKey)
            {
                case "FK_Emp":
                    qo.AddWhere("FK_Emp", WebUser.No);
                    break;
                case "FK_Dept":
                    qo.AddWhere("FK_Dept", WebUser.FK_Dept);
                    break;
                default:
                    throw new Exception("没有判断的GroupKey");
            }
        }
        qo.addOrderBy("GradeNo");
        qo.DoQuery();
        #endregion 获取数据.


        Entity en = ens.GetNewEntity;
        if (this.RefPK != null)
        {
            en.PKVal = this.RefPK;
            en.Retrieve();
        }


        #region 获取数据.
        foreach (Entity dtl in ens)
        {
            if (this.RefPK != null)
            {
                string pNo = en.GetValStringByKey("GradeNo");
                string dtlNo = dtl.GetValStringByKey("GradeNo");
                if (dtlNo.Length < pNo.Length)
                    continue;

                if (dtlNo.Substring(0, pNo.Length) != pNo)
                    continue;
            }

            this.UCSys1.Copy(dtl, dtl.PKVal.ToString());

            dtl.SetValByKey("FK_Emp", WebUser.No);
            dtl.SetValByKey("FK_Dept", WebUser.FK_Dept);
            dtl.Update();
        }
        #endregion 获取数据.

        this.Response.Redirect(this.Request.RawUrl, true);
    }
    #endregion

}
