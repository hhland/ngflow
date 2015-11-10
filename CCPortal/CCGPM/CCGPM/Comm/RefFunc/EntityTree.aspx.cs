using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;

public partial class Comm_RefFunc_EntityTree : System.Web.UI.Page
{
    #region 属性
    string EnsName
    {
        get
        {
            return this.Request.QueryString["EnsName"];
        }
    }
    string No
    {
        get
        {
            string s = this.Request.QueryString["No"];
            if (string.IsNullOrEmpty(s))
                s = "0001";
            return s;
        }
    }
    #endregion

    public Entities HisEns = null;
    public EntityTree HisEn = null;

    public void DoFunc(EntityTree en)
    {

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.HisEns = ClassFactory.GetEns(this.EnsName);
        this.HisEn = HisEns.GetNewEntity as EntityTree;
        if (this.HisEn == null)
            throw new Exception("@ 非 EntityTree 实体.");

        this.HisEns.RetrieveAllFromDBSource();
        if (HisEns.Count == 0)
        {
            this.HisEn.No = "0001";
            this.HisEn.ParentNo = "-1";
            this.HisEn.Name = "根节点";
            this.HisEn.TreeNo = "01";
            this.HisEn.Insert();

            this.HisEn = this.HisEns.GetNewEntity as EntityTree;
            this.HisEn.No = "0002";
            this.HisEn.ParentNo = "0001";
            this.HisEn.Name = "目录1";
            this.HisEn.TreeNo = "0101";
            this.HisEn.Insert();
            HisEns.RetrieveAllFromDBSource();
        }

        this.HisEn.PKVal = this.No;
        this.HisEn.Retrieve();

        string pk = null;
        switch (this.Request.QueryString["DoType"])
        {
            case "AddSameLevelNode":
                pk = this.HisEn.DoCreateSameLevelNode().No;
                this.Response.Redirect("EntityTree.aspx?EnsName=" + this.HisEns.ToString() + "&EnName=" + this.HisEn.ToString() + "&No=" + pk, true);
                break;
            case "AddSubNode":
                pk = this.HisEn.DoCreateSubNode().No;
                this.Response.Redirect("EntityTree.aspx?EnsName=" + this.HisEns.ToString() + "&EnName=" + this.HisEn.ToString() + "&No=" + pk, true);
                return;
            case "DoUp":
                this.HisEn.DoUp();
                break;
            case "DoDown":
                this.HisEn.DoDown();
                break;
            default:
                break;
        }

        this.HisEn.No = this.No;
        this.Title = HisEn.EnDesc;
        this.BindTree();
        this.BindEntities();
    }

    public void BindEntities()
    {
        Map map = this.HisEn.EnMap;
        this.Pub1.AddTable();
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("IDX");
        this.Pub1.AddTDTitle("树");
        foreach (Attr attr in map.Attrs)
        {
            if (attr.Key == BP.En.EntityTreeAttr.No || attr.Key == "Name"
                || attr.IsRefAttr || attr.UIVisible == false)
                continue;
            this.Pub1.AddTDTitle(attr.Desc);
        }
        this.Pub1.AddTREnd();

        EntitiesTree ens = this.HisEn.GetNewEntities as EntitiesTree;
        ens.RetrieveHisChinren(this.HisEn);

        int i = 0;
        foreach (EntityTree en in ens)
        {
            i++;
            this.Pub1.AddTR();

            RadioButton rb = new RadioButton();
            rb.ID = "RB_" + en.No;
            rb.GroupName = "s";
            rb.Text = i.ToString();
            this.Pub1.AddTDBegin("class=Idx");
            this.Pub1.Add(rb.Text);
            this.Pub1.AddTDEnd();

            this.Pub1.AddTDBegin();
            string text = "";
            if (en.No == "0001")
                text = "<img src='/Images/Tree/root.gif' border=0/>" + en.Name;
            else if (en.IsDir)
                text = "<img src='/Images/Tree/Dir.gif' border=0/>" + en.Name;
            else
                text = "<img src='/Images/Tree/File.gif' border=0/>" + en.Name;

            this.Pub1.Add("<a href=\"javascript:ShowEn('UIEn.aspx?EnsName=" + this.EnsName + "&PK=" + en.No + "','sd',500,700)\">" + text + "</a>");
            this.Pub1.AddTDEnd();

            foreach (Attr attr in map.Attrs)
            {
                if (attr.Key == "No" || attr.Key == "Name"
                    || attr.IsRefAttr || attr.UIVisible == false)
                    continue;

                if (attr.IsFKorEnum)
                    this.Pub1.AddTD(en.GetValRefTextByKey(attr.Key));
                else
                    this.Pub1.AddTD(en.GetValStrByKey(attr.Key));
            }
            this.Pub1.AddTREnd();
        }
        this.Pub1.AddTableEnd();
    }
    public void BindTree()
    {
        DataTable dt = this.HisEns.ToDataTableField();
        CreateTreeViewRootNode(dt, true);
    }

    #region 绑定tree.
    //创建TreeView的节点  
    private TreeNode CreateTreeViewNode(DataRow rows, bool isAddUrl)
    {
        //创建节点对象  
        TreeNode node = new TreeNode();
        node.Text = rows["Name"].ToString();      //节点对象的显示文本  
        node.Value = rows["No"].ToString();       //节点对象的非显示性值  

        //是否为节点对象添加连接  
        if (isAddUrl == true)
        {
            //节点指定连接URL  
            node.NavigateUrl = "EntityTree.aspx?EnsName=" + this.EnsName + "&EnName=" + this.HisEn.ToString() + "&No=" + rows["No"].ToString();
            //node.NavigateUrl = "~/Comm/Search.aspx?EnsName=BP.HR.Emps&FK_Dept=" + rows["ID"].ToString();
            //指示该节点点击后新打开的URL显示的目标窗口或框架  
            //此处指定显示于框架"luluFrame"中  
            node.Target = "_self";
        }
        else
        {
            //节点添加复选框  
            node.ShowCheckBox = true;
            //设置选择节点的引发事件  
            node.SelectAction = TreeNodeSelectAction.Expand;
        }

        if (node.Value == this.Request.QueryString["No"])
            node.Selected = true;

        //返回封装好的节点对象  
        return node;
    }

    //创建TreeView的根节点  
    private void CreateTreeViewRootNode(DataTable dt, bool isAddUrl)
    {
        //创建表格的行，该行是满足dt表中ParentID='-1'条件的行，为所有版面的父版面  
        DataRow[] rows = dt.Select("ParentNo='-1'");
        if (rows.Length <= 0)
            return;

        //创建根节点  
        TreeNode root = CreateTreeViewNode(rows[0], isAddUrl);

        //清空要绑定的树控件  
        this.TreeView1.Nodes.Clear();

        //添加根节点到树控件  
        this.TreeView1.Nodes.Add(root);

        //创建根节点下的其他子节点  
        CreateTreeViewChildNode(root, dt, isAddUrl);
    }

    //创建TreeView根节点下的其他子节点  
    private void CreateTreeViewChildNode(TreeNode parentNode, DataTable dt, bool isAddUrl)
    {

        //创建表格的行，该行是满足dt表中以该ParentID为父版面的所有子版面  
        DataRow[] rows = dt.Select("ParentNo='" + parentNode.Value + "'");
        //遍历表格行  
        foreach (DataRow row in rows)
        {
            if (row["No"].ToString() == row["ParentNo"].ToString())
                continue;

            //创建子节点
            TreeNode node = CreateTreeViewNode(row, isAddUrl);

            //父节点添加子节点  
            parentNode.ChildNodes.Add(node);

            //递归调用，创建其他子节点  
            CreateTreeViewChildNode(node, dt, isAddUrl);
        }
    }
    #endregion
}
