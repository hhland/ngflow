using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BP.Controls;
using BP.Frm;
using BP.SL;
using Liquid;
using OrganizationalStructure;
using Silverlight;
using WF.WS;

namespace BP
{
    /// <summary>
    ///  Designer home page 
    /// </summary>
    public partial class MainPage 
    {
        #region  Variable 
     

        private TreeNode nodeFlow ,
            nodeForm ,
            nodeGpm = new TreeNode() ;
        private List<ToolbarButton> ToolBarButtonList = new List<ToolbarButton>();
        private const string ToolBarEnableIsFlowSensitived = "EnableIsFlowSensitived";

        public static readonly string CustomerId = "CCFlow";

        private WSDesignerSoapClient _service = Glo.GetDesignerServiceInstance();
        public WSDesignerSoapClient _Service
        {
            get { return _service; }
            set { _service = value; }
        }

        public Container SelectedContainer
        {
            get
            {
                var c = (Container)tbDesigner.SelectedContent;
                return c;
            }
        }

        public bool IsRefresh { get; set; }

        #endregion

        static MainPage instance;
        public static MainPage Instance
        {
            get
            {
                if (null == instance)
                    instance = new MainPage();
                return MainPage.instance;
            }
        }
      
        private MainPage()
        {
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("Designer", this);
            this.LayoutRoot.SizeChanged += this.LayoutRoot_SizeChanged;
            Application.Current.Host.Content.Resized += Content_Resized;
            Application.Current.Host.Content.FullScreenChanged += Content_FullScreenChanged ;
            this.Loaded += MainPage_Loaded;

            this.getOsModel();
            this.setCustomerAttribute();
            this.initFlowTempleteLoadCompeletedEventHandler();

            this.tvwFlow.NodeDoubleClick += (object sender, TreeEventArgs e) =>
            {
                #region  Process Designer 
                TreeNode node = sender as TreeNode;
                if (null != node && !node.IsSort) 
                {
                    OpenFlow(node.Name, node.Title);
                }
                #endregion
            } ;
            this.tvwForm.NodeDoubleClick += (object sender, TreeEventArgs e) =>
            {
                #region  Process Form Designer 
                TreeNode node = sender as TreeNode;
                if (null != node && !node.IsSort) 
                {
                    string title = " Form ID: {0}  Storage Table :{1}  Name :{2}";
                    title = string.Format(title, node.Name, node.Name, node.Title);
                    if (Glo.UrlOrForm)
                    {
                        string url = "/WF/Admin/XAP/DoPort.aspx?DoType=MapDefFreeModel&FK_MapData=" + node.Name;
                        OpenBPForm(BPFormType.FormFlow, title, url);
                    }
                    else
                    {
                        OpenBPForm(BPFormType.FormFlow, title, node.Name, node.Name);
                    }
                }
                #endregion
            } ;

            this.tvwOrg.NodeDoubleClick += (tvwOrg_NodeDoubleClick);
            //this.tvwOrg.MouseRightButtonUp += element_MouseRightButtonUp;

            MouseButtonEventHandler rbd = (object sender, MouseButtonEventArgs e) =>
            { e.Handled = true; };
            this.MouseRightButtonDown += rbd;
            this.tvwSysMenu.MouseRightButtonDown += rbd;
            this.tvwOrg.MouseRightButtonDown += rbd;
            this.tvwFlow.MouseRightButtonDown += rbd;
            this.tvwForm.MouseRightButtonDown += rbd;


            this.MouseRightButtonUp += element_MouseRightButtonUp;
            this.tvwSysMenu.MouseRightButtonUp += element_MouseRightButtonUp;
            this.tvwFlow.MouseRightButtonUp += element_MouseRightButtonUp;
            this.tvwForm.MouseRightButtonUp += element_MouseRightButtonUp;

            this.tbcLeft.SelectionChanged += TabControlTree_SelectionChanged;

        }

        //  Configure User Attributes b
        void setCustomerAttribute()
        {
            var designerService = Glo.GetDesignerServiceInstance();
            this.Visibility = System.Windows.Visibility.Collapsed;
            designerService.DoAsync("GetSettings", "CustomerNo", true);  //  Picture .
            designerService.DoCompleted += (object senders, DoCompletedEventArgs ee) =>
            {
                Exception exc = null;
                bool toBeContinued = true;
                if (null != ee.Error)
                {
                    exc = ee.Error;
                    toBeContinued = false;
                }

                if(toBeContinued)
                    try
                    {
                        var id = ee.Result;
                        if (id == null || id == "")
                            id = "CCFlow";

                        imageLogo.Source = new BitmapImage(new Uri(string.Format("./Images/Icons/{0}/Icon.png", id), UriKind.Relative));
                        var brush = new ImageBrush(); // Definition picture brush 
                        brush.ImageSource = new BitmapImage(new Uri(string.Format("./Images/Icons/{0}/Welcome.png", id), UriKind.Relative));
                        tbDesigner.Background = brush;
                    }
                    catch (Exception ex)
                    {
                        exc = ex;
                        toBeContinued = false;
                    }


                if (!toBeContinued)
                {
                    Glo.ShowException(exc);
                }
                this.Visibility = System.Windows.Visibility.Visible;

            };
        }
        public void LoginCompleted()
        {
            try
            {
                this.SetSelectedTool("Wait");
                //装 toolbar.
                this.LoadToolbar();

                //  Process tree .
                this.BindFlowAndFlowSort();

                this.BindFormTree();

                this.checkOrgRoot();
           
                // InitDesignerXml
                var designerService = Glo.GetDesignerServiceInstance();
                designerService.DoTypeAsync("InitDesignerXml", null, null, null, null, null);
                designerService.DoTypeCompleted += designer_DoTypeCompleted;

            }
            catch (Exception ex)
            {
                Glo.ShowException(ex, " Login failed system ");
            }
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            bool isLogOn= 
                System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("UserNo")
                && System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("SID");

            if (!isLogOn)
            {
                 Glo.Login();
            }
            else 
            {
                string userNo = System.Windows.Browser.HtmlPage.Document.QueryString["UserNo"];
                string passWord = System.Windows.Browser.HtmlPage.Document.QueryString["SID"];

                isLogOn = !String.IsNullOrEmpty(userNo) && !string.IsNullOrEmpty(passWord);
                if( !isLogOn)
                    Glo.Login();

                var ws = Glo.GetDesignerServiceInstance();
                ws.RunSQLReturnTableAsync("SELECT SID FROM Port_Emp WHERE No='" + userNo + "'");
                ws.RunSQLReturnTableCompleted 
                    += (object senders, RunSQLReturnTableCompletedEventArgs ee)=>
                    {
                        #region
                        if (null != ee.Error)
                        {
                            Glo.ShowException(ee.Error, " Login Error ");
                            return;
                        }
                        try
                        {
                            DataSet ds = new DataSet();
                            try
                            {
                                ds.FromXml(ee.Result);
                            }
                            catch (Exception ex)
                            {
                                Glo.ShowException(ex, " Login Error ");
                                return;
                            }

                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count != 1)
                                throw new Exception("@ No inquiry into the user login SID Information .");

                            string sid = System.Windows.Browser.HtmlPage.Document.QueryString["SID"];
                            string s = dt.Rows[0][0].ToString();
                            if (s.Equals(sid) == false)
                            {
                                throw new Exception("@ User name or password is incorrect .");
                            }
                        }
                        catch (Exception ex)
                        {
                            Glo.ShowException(ex, " Login Error ");
                            Glo.Login();
                        }
                        #endregion
                    };
            }
        }

        #region Flow 

        /// <summary>
        ///  Binding workflow tree 
        /// </summary>
        internal void BindFlowAndFlowSort()
        {
            this.SetSelectedTool("Wait");

            _service = Glo.GetDesignerServiceInstance();
            _Service.DoAsync("GetFlows", string.Empty, true);
            _Service.DoCompleted += BindFlowTree_GetFlowsCompleted;
        }
        /// <summary>
        ///  Get a workflow type of event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BindFlowTree_GetFlowsCompleted(object sender, DoCompletedEventArgs e)
        {
            _Service.DoCompleted -= BindFlowTree_GetFlowsCompleted;
            bool toBeContinued = true;
            Exception exc = null;

            if (e.Error != null)
            {
                exc = e.Error;
                toBeContinued = false;
            }

            if (toBeContinued)
                try
                {

                    DataSet ds = new DataSet();
                    ds.FromXml(e.Result);

                    nodeFlow = new TreeNode();
                    tvwFlow.Clear();

                    // Binding FlowSort  Root directory 
                    TreeNode rootNode = new TreeNode();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string parentNo = dr["ParentNo"].ToString();
                        if (string.IsNullOrEmpty(parentNo))
                            parentNo = "0";

                        if (parentNo != "0")
                            continue;

                        rootNode.Title = dr["Name"].ToString();
                        rootNode.ID = dr["No"].ToString();
                        rootNode.IsSort = true;
                        rootNode.isRoot = true;
                        rootNode.Icon = "../Images/MenuItem/FlowSort.png";
                        nodeFlow.Nodes.Add(rootNode);
                        break;
                    }
                    if (string.IsNullOrEmpty(rootNode.ID))
                    {
                        throw new Exception("@ Binding process tree error , Not found ParentNo=0 With the directory :");
                    }

                    // The second-level directory 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string parentNo = dr["ParentNo"].ToString();
                        if (parentNo != rootNode.ID.ToString())
                            continue;

                        TreeNode subNode = new TreeNode();
                        subNode.Title = dr["Name"].ToString();
                        subNode.ID = dr["No"].ToString();
                        subNode.IsSort = true;
                        subNode.Icon = "../Images/MenuItem/FlowSort.png";
                        nodeFlow.Nodes.Add(subNode);
                        rootNode.Nodes.Add(subNode);

                        AddSubTreeNode(subNode, ds.Tables[0], TreeType.Flow);
                    }
                    tvwFlow.Nodes.Add(rootNode);
                    rootNode.IsExpanded = true;
                    rootNode.Expand();

                    #region   Binding Process 
                    foreach (DataRow d in ds.Tables[1].Rows)
                    {
                        string tmp_FK_FlowSort = d["FK_FlowSort"];
                        var node = new TreeNode();
                        node.Title = d["Name"].ToString();
                        node.ID = tmp_FK_FlowSort;
                        node.Name = d["No"].ToString();
                        node.Icon = "../Images/MenuItem/EditTable4.png";
                        node.IsSort = false;
                        if (SelectedContainer != null)
                        {
                            if (SelectedContainer.FlowID == node.Name)
                            {
                                var te = this.tbDesigner.SelectedItem as TabItemEx;
                                te.Title = node.Title;
                                Canvas cs = te.Header as Canvas;
                                TextBlock tbx = cs.Children[1] as TextBlock;
                                tbx.Text = node.Title;
                            }
                        }
                        foreach (TreeNode ne in nodeFlow.Nodes)
                        {
                            if (ne.ID == tmp_FK_FlowSort)
                            {
                                ne.Nodes.Add(node);
                                ne.IsSort = true;
                            }
                        }
                    }
                    #endregion

                    //  Upon completion of the binding , Expand final FlowSort
                    foreach (TreeNode node in tvwFlow.Nodes)
                    {
                        if (node.ID == Glo.FK_FlowSort)
                        {
                            node.IsExpanded = true;
                            node.Expand();
                            Glo.FK_FlowSort = string.Empty;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    exc = ex;
                    toBeContinued = false;
                }

            if (exc != null)
            {
                Glo.ShowException(exc, " An error occurred while binding process tree ");
            }
            this.SetSelectedTool("Arrow");

        }
     
        /// <summary>
        ///  Delete workflow category 
        /// </summary>
        /// <param name="flowsortid"> Workflow category ID</param>
        public void DeleteFlowSort(string flowsortid)
        {
            if (HtmlPage.Window.Confirm(" You sure you want to delete it ?"))
            {
                _Service.DoAsync("DelFlowSort", flowsortid, true);
                Glo.Loading(true);
                _Service.DoCompleted += Server_DoCompletedToRefreshSortTree;
            }
        }

        string flowNoToDel = string.Empty;
        /// <summary>
        ///  Delete workflow and update process tree menu and sketchpad 
        /// </summary>
        public void DeleteFlow(string flowid)
        {
            if (HtmlPage.Window.Confirm(" Are you sure you want to delete the number of 【" + flowid + "】 Workflow right ?"))
            {
                Glo.Loading(true);
                _Service.DoCompleted += Server_DoCompletedToRefreshSortTree;
                _Service.DoAsync("DelFlow", flowid, true);
                flowNoToDel = flowid;

            }
        }
        void Server_DoCompletedToRefreshSortTree(object sender, DoCompletedEventArgs e)
        {
            Glo.Loading(false);
            _Service.DoCompleted -= Server_DoCompletedToRefreshSortTree;

            if (null != e.Error)
            {
                Glo.ShowException(e.Error, " Process delete error ");
                return;
            }
            if (e.Result != null)
            {
                MessageBox.Show(e.Result, "Err", MessageBoxButton.OK);
                return;
            }

            foreach (TabItem t in tbDesigner.Items)
            {
                var ct = t.Content as Container;
                if (ct != null && tvwFlow.Selected != null && ct.FlowID == tvwFlow.Selected.Name)
                {
                    tbDesigner.Items.Remove(t);
                    break;
                }
            }

            //TreeNode node = this.findNode(tvwFlow.Nodes[0] as TreeNode, this.flowNameToDel);
            //if (null != node && null != node.ParentNode)
            //    node.ParentNode.Nodes.Remove(node);
            //this.InvalidateArrange();

            this.flowNoToDel = string.Empty;
            this.BindFlowAndFlowSort();

        }

        /// <summary>
        ///  New Workflow 
        /// </summary>
        public void NewFlow(string flowSortId, string flowName, int dataSaveModel, string pTable, string flowCode)
        {
            Glo.Loading(true);
            _Service.DoAsync("NewFlow", flowSortId + "," + flowName + "," + dataSaveModel + "," + pTable + "," + flowCode, true);
            _Service.DoCompleted += _service_DoCompleted;
        }
        private void _service_DoCompleted(object sender, DoCompletedEventArgs e)
        {
            bool toBeContinued = true;
            Exception exc = null;
            _Service.DoCompleted -= _service_DoCompleted;

            if (null != e.Error)
            {
                exc = e.Error;
                toBeContinued = false;
            }

            if (toBeContinued && e.Result.IndexOf(";") < 0)
            {
                MessageBox.Show(e.Result, " Error 3", MessageBoxButton.OK);
                toBeContinued = false;
            }

            if (toBeContinued)
            {
                try
                {
                    string[] flow = e.Result.Split(';');

                    this.BindFlowAndFlowSort();
                    OpenFlow(flow[0], flow[1]);
                }
                catch(Exception ex)
                {
                    exc = ex;
                }
            }

            if( exc != null)
            {
                Glo.ShowException(exc, " New Process Error :" + exc.Message);
            }
            Glo.Loading(false);
        }

        public void NewFlowHandler(int tabIdx)
        {
            Glo.FK_FlowSort = tvwFlow.Selected.ID;
            var newFlow = new FrmNewFlow();
            newFlow.tabControl.TabIndex = tabIdx;

            if (tvwFlow.Selected != null && ((TreeNode)tvwFlow.Selected).IsSort)
            {
                newFlow.CurrentFlowSortName = tvwFlow.Selected.Title;
            }
            newFlow.FlowTempleteLoadCompeletedEventHandler += FlowTemplateLoadCompeleted;
            newFlow.Show();
        }

        void ViewShareTemplate()
        {
            FtpFileExplorer templateView = new FtpFileExplorer();
            templateView.FlowTemplateLoadCompeleted += this.FlowTemplateLoadCompeleted;
            templateView.Show();
        }
        //  Process loaded into the database end event , New and used when importing , Require manual initialization 
        EventHandler<FlowTemplateLoadCompletedEventArgs> FlowTemplateLoadCompeleted;
        void initFlowTempleteLoadCompeletedEventHandler()
        {
            FlowTemplateLoadCompeleted = (object sender, FlowTemplateLoadCompletedEventArgs e) =>
            {
                try
                {
                    if (null != e.Error)
                    {
                        Glo.ShowException(e.Error, " Error loading process ");
                    }
                    else
                    {
                        //  The return value of the format FlowSortID,FlowId,FlowName
                        var result = e.Result.Split(',');
                        if (!e.Result.Contains("TRUE") || result.Length != 4)
                        {
                            MessageBox.Show(" Error loading process " + e.Result);
                        }
                        else
                        {
                            string msgWin =
@"     Process template has been successfully downloaded and installed to a local server , But the process template may not work properly , You need to pay attention to the following points and make the appropriate changes :
    1, The template or process node binding posts , Department , Personnel information , Will work with your system posts , Department , Number inconsistent personnel information , You need to rebind to use .
    2, For a node , Process , Special events business process forms , Will affect the execution of your system , You can disable or edit them in line with their business environment needs .
";
                            MessageBox.Show(msgWin);
                            this.BindFlowAndFlowSort();
                            OpenFlow(result[1], result[2], result[3]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Glo.ShowException(ex, " Error loading process ");
                }
            };
        }

        #endregion

        #region GPMTree

        void checkOrgRoot()
        {
            // Check the root node 
            _service.Dept_CheckRootNodeAsync();
            EventHandler<Dept_CheckRootNodeCompletedEventArgs> handler = null;
            handler= (object sender, Dept_CheckRootNodeCompletedEventArgs e) =>
            {
                _service.Dept_CheckRootNodeCompleted -= handler;
            };
            _service.Dept_CheckRootNodeAsync();
            _service.Dept_CheckRootNodeCompleted += handler;
        }

        bool isGMPTreeInited;
        public void BingTreeOrg()
        {
            // Binding department             
            string sqls = string.Empty;
            if (Glo.OsModel == OSModel.BPM)
            {
                sqls = "SELECT No,Name,ParentNo,TreeNo FROM Port_Dept ORDER BY Idx ASC,No ASC";
                sqls += "@SELECT  No,Name,FK_Dept  FROM Port_Emp ";
            }
            else
            {
                sqls = "SELECT No,Name,ParentNo FROM Port_Dept ORDER BY Idx ASC,No ASC";
                sqls += "@SELECT  No,Name,FK_Dept  FROM Port_Emp ";
            }
            this.SetSelectedTool("Wait");

            _Service = Glo.GetDesignerServiceInstance();
            _service.RunSQLReturnTableSAsync(sqls);
            _service.RunSQLReturnTableSCompleted += Org_RunSQLReturnTableSCompleted;
        }
        void Org_RunSQLReturnTableSCompleted(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {

            _service.RunSQLReturnTableSCompleted -= Org_RunSQLReturnTableSCompleted;
            bool toBeContinued = true;
            if (null != e.Error)
            {
                Glo.ShowException(e.Error, " Organizational structure of the data access failure " );
                toBeContinued = false;
            }

            if(toBeContinued)
            try
            {
                nodeGpm.Nodes.Clear();

                DataSet ds = new DataSet();
                ds.FromXml(e.Result);
                DataTable dtDept = ds.Tables[0]; // Sector Information .
                TreeNode rootNode = new TreeNode();
                string rootId = string.Empty;
                foreach (DataRow dr in dtDept.Rows)
                {   // The root structure  

                    string parentNo = dr["ParentNo"];
                    if (parentNo != "0")
                        continue;

                    rootId = dr["No"];
                    rootNode.isRoot = true;
                    rootNode.Icon = "../Images/TreeMenu/Org.png";
                    rootNode.Title = dr["Name"];
                    rootNode.Name = rootId;
                    rootNode.ID = rootId;

                    nodeGpm.Nodes.Add(rootNode);
                   
                }
 
                if (string.IsNullOrEmpty(rootId)) return;
                //  Two directories 
                foreach (DataRow dr in dtDept.Rows)
                {
                    string parentNo = dr["ParentNo"];
                    if (!rootId.Equals(parentNo))
                        continue;

                    string subId = dr["No"];
                    TreeNode subNode = new TreeNode();
                    subNode.Title = dr["Name"];
                    subNode.Name = subId;
                    subNode.ID = subId;
                    subNode.Icon = "../Images/MenuItem/Post.png";
                    subNode.isDept = true;
                    rootNode.Nodes.Add(subNode);
                    nodeGpm.Nodes.Add(subNode);

                    AddSubTreeNode(subNode, dtDept, TreeType.GPM);
                }

                this.tvwOrg.Clear();
                this.tvwOrg.Nodes.Add(rootNode);
                rootNode.IsExpanded = true;
                rootNode.Expand();

                DataTable dtEmp = ds.Tables[1];
                foreach (var dr in dtEmp.Rows)
                {
                    foreach (TreeNode ne in this.nodeGpm.Nodes)
                        if (ne.ID == dr["FK_Dept"])
                        {
                            string no = dr["No"];
                            TreeNode nodeEmp = new TreeNode();
                            nodeEmp.Title = dr["Name"];
                            nodeEmp.Name = no;
                            nodeEmp.ID = no;
                            nodeEmp.Icon = "../Images/MenuItem/People.png";
                            ne.Nodes.Add(nodeEmp);
                            continue;
                        }
                }

                isGMPTreeInited = true;
            }
            catch (Exception ee)
            {
                Glo.ShowException(ee);
                toBeContinued = false;
            }
            this.SetSelectedTool("Arrow");

        }

        void tvwOrg_NodeDoubleClick(object sender, TreeEventArgs e)
        {
            // Left Press   Get deptNo
            TreeNode node = sender as TreeNode;
            if (node == null)
                return;

            Glo.CurNodeOrg = node;
            string url = string.Empty;

            if (node.isRoot)
            {
                return;
                //if (Glo.OsModel == OSModel.BPM)
                //{
                //    url = "/WF/Comm/Search.aspx?EnsName=BP.GPM.Depts&PK={0}&No={0}";
                //}
                //else
                //{
                //    url = "/WF/Comm/Search.aspx?EnsName=BP.Port.Depts&FK={0}&No={0}";
                //}

                //url = string.Format(url, node.ID);
                //Glo.OpenWindow(url, " Sector Information ");
            }
            if (node.isDept)
            {
                if (Glo.OsModel == OSModel.BPM)
                {
                    url = "/WF/Comm/Search.aspx?EnsName=BP.GPM.Depts&FK_Dept={0}&No={0}";
                }
                else
                {
                    url = "/WF/Comm/Search.aspx?EnsName=BP.Port.Depts&FK_Dept={0}&No={0}";
                }
                url = string.Format(url, node.ID);
                Glo.OpenWindow(url, " Sector Information ");
            }
            else
            {
                if (Glo.OsModel == OSModel.BPM)
                {
                    url = "/WF/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Emps&PK={0}";
                }
                else
                {
                    url = "/WF/Comm/RefFunc/UIEn.aspx?EnsName=BP.Port.Emps&PK={0}";
                }
                url = string.Format(url, node.ID);
                Glo.OpenWindow(url, " Employee Information ");

                //OnTreeNodeDBClick();  // Read the relevant data       

            }
        }
               
        void getOsModel()
        {
            WSDesignerSoapClient ws = Glo.GetDesignerServiceInstance();
            ws.GetConfigAsync("OSModel");
            ws.GetConfigCompleted += (object sender, GetConfigCompletedEventArgs e) =>
            {
                if (e.Error == null)
                {
                    Glo.OsModel = (OSModel)Enum.Parse(typeof(OSModel), e.Result,true);
                }
            };
        }

        /// <summary>
        ///  When you open a tree node 
        /// </summary>
        /// <param name="deptNo"></param>
        public void OnTreeNodeDBClick()
        {
            if (Glo.CurNodeOrg == null || string.IsNullOrEmpty(Glo.CurNodeOrg.ID))
                return;
            //BindEmpByDeptNo(Glo.CurNodeOrg);
        }
        private void BindEmpByDeptNo( TreeNode nodeDept)
        {
            EventHandler<RunSQLReturnTableCompletedEventArgs> handler = null;
            handler = (object sender, RunSQLReturnTableCompletedEventArgs e) =>
            {
                _service.RunSQLReturnTableCompleted -= handler;
                if (null != e.Error)
                {
                    Glo.ShowException(e.Error, " Organizational structure of the data access failure ");
                    return;
                }

                DataSet ds = new DataSet();
                ds.FromXml(e.Result);
                DataTable dtEmp = ds.Tables[0]; // Personnel information .

                foreach (var emp in dtEmp.Rows)
                {
                    string noEmp = emp["No"];
                    string fk_Dept = emp["FK_Dept"];
                    if (nodeDept.ID.Equals(fk_Dept))
                    {
                        TreeNode nodeEmp = new TreeNode();
                        nodeEmp.Title = emp["Name"];
                        nodeEmp.Name = noEmp;
                        nodeEmp.ID = noEmp;
                        nodeDept.Icon = "../Images/MenuItem/people.png";
                        bool find = false;
                        for (int i = 0; i < nodeDept.Nodes.Count; i++)
                        {
                            if (nodeDept.Nodes[i].ID == noEmp)
                            {
                                nodeDept.Nodes[i] = nodeEmp;
                                find = true;
                            }
                        }
                        if(!find)
                            nodeDept.Nodes.Add(nodeEmp);
                       
                    }
                }
            };
          
            string sql = "SELECT  No,Name,FK_Dept  FROM Port_Emp where FK_Dept ='{0}' ";
            sql = string.Format(sql, nodeDept.ID);
            _Service = null;
            _Service = Glo.GetDesignerServiceInstance();
            _service.RunSQLReturnTableCompleted += handler;
            _service.RunSQLReturnTableAsync(sql);
          
        }

        void setGMPMenu(TreeNode nodeOrg)
        {
            bool isEnabled ;
            Glo.CurNodeOrg = nodeOrg;
            

            if (nodeOrg.isRoot)
            {
                isEnabled = false;
                menuOrg.Get("Dept_CrateSameLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_CrateSubLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Refresh").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Edit").IsEnabled = isEnabled;

                menuOrg.Get("Dept_Delete").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Edit").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Add").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Related").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Up").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Down").IsEnabled = isEnabled;
            }
            else if(nodeOrg.isDept)
            {
                isEnabled = true;
                menuOrg.Get("Dept_Delete").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Edit").IsEnabled = isEnabled;
                menuOrg.Get("Dept_CrateSameLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_CrateSubLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Refresh").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Up").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Down").IsEnabled = isEnabled;

                isEnabled = !isEnabled;
                menuOrg.Get("Emp_Edit").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Add").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Related").IsEnabled = isEnabled;
            }else
            {
                isEnabled = false;
                menuOrg.Get("Dept_Delete").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Edit").IsEnabled = isEnabled;
                menuOrg.Get("Dept_CrateSameLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_CrateSubLevel").IsEnabled = isEnabled;
                menuOrg.Get("Dept_Refresh").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Up").IsEnabled = isEnabled;
                menuOrg.Get("Btn_Down").IsEnabled = isEnabled;

                isEnabled = !isEnabled;
                menuOrg.Get("Emp_Edit").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Add").IsEnabled = isEnabled;
                menuOrg.Get("Emp_Related").IsEnabled = isEnabled;
            }
        }

        private void menu_ItemOrg_Click(object sender, MenuEventArgs e)
        {
            TreeNode td = tvwOrg.Selected as TreeNode;
            if (td == null)
                return;

            Glo.CurNodeOrg = td;
            
            string deptNo = td.ID.ToString();

            FrmDept fd = new FrmDept() ;
            fd.ReFreshParentEve += new FrmDept.ReFreshParent(()=>
            {
                this.BingTreeOrg();
            });// Registration Event , Refresh the parent form 

            switch (e.Tag.ToString())
            {
                case "Dept_Refresh": // Refresh .
                    this.BingTreeOrg();
                    break;
                case "Dept_Edit": // Editor 

                    fd.InitDeptInfo("EditDept", deptNo);
                    fd.Show();
                    break;
                case "Dept_CrateSameLevel": //  Create the same directory .
                    fd.InitDeptInfo("CrateSameLevel", deptNo);
                    fd.Show();
                    
                    break;
                case "Dept_CrateSubLevel": //  Create a lower directory .
                    fd.InitDeptInfo("CrateSubLevel", deptNo);
                    fd.Show();
                    //this.Refresh();
                    break;
                case "Dept_Delete": //  Delete department .
                    if (MessageBox.Show(" Are you sure you want to delete it ?", " Prompt ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WSDesignerSoapClient daDelDept = Glo.GetDesignerServiceInstance();
                        daDelDept.Dept_DeleteAsync(deptNo, false);
                        daDelDept.Dept_DeleteCompleted += daDelDept_Dept_DeleteCompleted;
                    }
                    break;
                case "Emp_Edit": //  Editorial staff .
                    OnTreeNodeDBClick();  // Read the relevant data 
                    break;
                case "Emp_Add": //  Increase staff .
                    FrmDeptEmp fde = new FrmDeptEmp();
                    fde.ReFreshParentEve += new FrmDeptEmp.ReFreshParent(OrgRefresh);
                    fde.InitEmp("New", deptNo, null);
                    fde.Show();
                    break;
                case "Emp_Related":// Related persons 
                    FrmEmps emps = new FrmEmps();
                    emps.ReFreshParentEve += new FrmEmps.ReFreshParent(OrgRefresh);
                    emps.InitEmps(deptNo);
                    emps.Show();
                    break;
                case "Btn_Up": // Move .
                    WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
                    da.DoAsync("DeptUp", deptNo, true);
                    da.DoCompleted += da_DoCompleted;
                    break;
                case "Btn_Down": // Down .
                    da = Glo.GetDesignerServiceInstance();
                  
                    da.DoAsync("DeptDown", deptNo, true);
                    da.DoCompleted += da_DoCompleted;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Refresh staff list 
        /// </summary>
        void OrgRefresh()
        {
            if (Glo.CurNodeOrg != null || string.IsNullOrEmpty(Glo.CurNodeOrg.ID)) return;
            OnTreeNodeDBClick();  // Read the relevant data 
        }
       
        // Increase staff 
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (Glo.CurNodeOrg == null || string.IsNullOrEmpty(Glo.CurNodeOrg.ID))
            {
                MessageBox.Show(" Please select the department .", " Prompt ", MessageBoxButton.OK);
                return;
            }

            FrmDeptEmp fde = new FrmDeptEmp();
            fde.InitEmp("New", Glo.CurNodeOrg.ID, null);
            fde.ReFreshParentEve += new FrmDeptEmp.ReFreshParent(OrgRefresh);
            fde.Show();
        }
        // Delete staff 
        private void Btn_Delete1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to delete it ?", " Prompt ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;

            //DataGridSelectNode = this.DG_Emp.SelectedItem as PeopleNode;
            //if (DataGridSelectNode == null) //DataGridSelectNode  Put the left-selected data 
            //{
            //    return;
            //}
            //if (DataGridSelectNode.FK_Duty == "99999")
            //{
            //    MessageBox.Show(" You choose the job information , Please select staff .", " Prompt ", MessageBoxButton.OK);
            //    return;// If you select a job , Not be edited 
            //}

            //WSDesignerSoap daDeleteEmp = Glo.GetDesignerServiceInstance();
            //daDeleteEmp.Emp_DeleteAsync(DataGridSelectNode.No, DataGridSelectNode.FK_Dept);//("DeleteEmp", DataGridSelectNode.No, true);
            //daDeleteEmp.Emp_DeleteCompleted += new EventHandler<Emp_DeleteCompletedEventArgs>(DeleteEmp_DoCompleted);
        }
        // Refresh 
        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (Glo.CurNodeOrg == null || string.IsNullOrEmpty(Glo.CurNodeOrg.ID))
            {
                MessageBox.Show(" Please select the department .", " Prompt ", MessageBoxButton.OK);
                return;
            }
            OnTreeNodeDBClick();  // Read the relevant data 
        }
        void da_DoCompleted(object sender, DoCompletedEventArgs e)
        {
            this.BingTreeOrg();
        }
        void daDelDept_Dept_DeleteCompleted(object sender, Dept_DeleteCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Contains("err"))
            {
                if (MessageBox.Show(e.Result + ", Are you sure you want to delete it mandatory ?", " Prompt ", MessageBoxButton.OKCancel)
== MessageBoxResult.OK)
                {
                    //WSDesignerSoap daDelDept = Glo.GetDesignerServiceInstance();
                    //daDelDept.Dept_DeleteAsync(Glo.FK_Dept, true);
                    //daDelDept.Dept_DeleteCompleted += new EventHandler<Dept_DeleteCompletedEventArgs>(daDelDept_Dept_DeleteCompleted);
                    return;
                }
            }

            //  Remove Node.
            //this.tvwOrg.Nodes.Remove(Glo.CurNodeOrg);
            //Glo.CurNodeOrg = null;
            //this.BingTreeOrg();
        }
      

        #endregion

        #region FormTree
        public void BindFormTree()
        {
            this.SetSelectedTool("Wait");

            string sqls = "";
            sqls += "@SELECT No,Name,ParentNo FROM Sys_FormTree ORDER BY Idx ASC,No ASC";
            sqls += "@SELECT No,Name,FK_FormTree FROM Sys_MapData WHERE AppType=" + (int)AppType.Application
                + " AND FK_FormTree IN (SELECT No FROM Sys_FormTree) ORDER BY Idx ASC ,No ASC";
            _Service = Glo.GetDesignerServiceInstance();
            _Service.RunSQLReturnTableSAsync(sqls);
            _Service.RunSQLReturnTableSCompleted += BindFormTree_RunSQLReturnTableSCompleted;
        }
        void BindFormTree_RunSQLReturnTableSCompleted(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            _Service.RunSQLReturnTableSCompleted -= BindFormTree_RunSQLReturnTableSCompleted;
            this.SetSelectedTool("Arrow");

            if (null != e.Error)
            {
                Glo.ShowException(e.Error, " An error occurred while loading a form tree ");
                return;
            }

            DataSet ds = null;
            try
            {
                ds = new DataSet();
                ds.FromXml(e.Result);
            }
            catch (Exception ex)
            {
                Glo.ShowException(ex, " An error occurred while loading a form tree ");
                return;
            }

            try
            {
                // Root directory 
                TreeNode rootNode = new TreeNode(); 
                //rootNode.Title = " Form tree ";
                //rootNode.ID = "0";
                //rootNode.isRoot = true;
                //rootNode.IsSort = true;
                //rootNode.Icon = "../Images/MenuItem/FlowSheet.png";
                nodeForm = new TreeNode();
                nodeForm.Nodes.Add(rootNode);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string parentNo = dr["No"];
                    if (string.IsNullOrEmpty(parentNo))
                        parentNo = "0";

                    if (parentNo == "0")
                    {
                        rootNode.Title = dr["Name"].ToString();
                        rootNode.ID = dr["No"].ToString();
                        rootNode.IsSort = true;
                        rootNode.isRoot = true;
                        rootNode.Icon = "../Images/MenuItem/FlowSheet.png";
                        break;
                    }
                }

                // The second-level directory 
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string parentNo = dr["ParentNo"] as string;
                    if (parentNo != rootNode.ID.ToString())
                        continue;

                    var node = new TreeNode();
                    node.Title = dr["Name"].ToString();
                    node.ID = dr["No"].ToString();
                    node.Name = dr["No"].ToString();
                    node.IsSort = true;
                    node.Icon = "../Images/MenuItem/FlowSheet.png";

                    nodeForm.Nodes.Add(node);
                    rootNode.Nodes.Add(node);

                    // Increase subordinate .
                    AddSubTreeNode(node, ds.Tables[0], TreeType.MapData);
                }
                this.tvwForm.Clear();
                tvwForm.Nodes.Add(rootNode);
                rootNode.IsExpanded = true;
                rootNode.Expand();

                // Unbound form 
                foreach (DataRow d in ds.Tables[1].Rows)
                {
                    var node = new TreeNode();
                    node.Title = d["Name"].ToString();
                    node.ID = d["FK_FormTree"].ToString();
                    node.Name = d["No"].ToString();
                    node.Icon = "../Images/MenuItem/EditTable4.png";
                    node.IsSort = false;
                    foreach (TreeNode ne in nodeForm.Nodes)
                    {
                        if (ne.ID == d["FK_FormTree"].ToString())
                        {
                            ne.Nodes.Add(node);
                        }
                    }
                }

                //  Upon completion of the binding , Expand final FlowSort
                foreach (TreeNode node in this.tvwForm.Nodes)
                {
                    if (node.ID == Glo.FK_FormSort)
                    {
                        node.IsExpanded = true;
                        node.Expand();
                        Glo.FK_FlowSort = string.Empty;
                        break;
                    }
                }

            }
            catch (Exception ee)
            {
                Glo.ShowException(ee, " An error occurred while loading a form tree ");
            }
           
        }
     
        ///  Set shortcut menu tree form 
        /// <param name="isFormOrSort"> true:  Enable Forms menu ;false: Enable type menu </param>
        private void EnableMenuFormTree(TreeNode node, Point menuPosition)
        {
            this.menuForm.Visibility = System.Windows.Visibility.Collapsed;
            this.menuFormSort.Visibility = System.Windows.Visibility.Collapsed;
            Liquid.Menu menu = null;
            double 
                menuHeight = 235,
                menuWidth = 170;
            if (node.isRoot)
            {
                menu = this.menuFormSort;
                menuFormSort.Get("Frm_Delete").IsEnabled = false;
                menuFormSort.Get("Frm_Up").IsEnabled = false;
                menuFormSort.Get("Frm_Down").IsEnabled = false;
                menuFormSort.Get("Frm_NewSameLevelSort").IsEnabled = false;

                //menuFormSort.Get("Frm_NewForm").IsEnabled = true;
                //menuFormSort.Get("Frm_NewSubSort").IsEnabled = true;
                //menuFormSort.Get("Frm_EditSort").IsEnabled = true;
                //menuFormSort.Get("Frm_Refresh").IsEnabled = true;
            }
            else if (node.IsSort)
            {
                Glo.FK_FormSort = node.ID;

                menu = this.menuFormSort;
                menuFormSort.Get("Frm_Delete").IsEnabled = true;
                menuFormSort.Get("Frm_Up").IsEnabled = true;
                menuFormSort.Get("Frm_Down").IsEnabled = true;
                menuFormSort.Get("Frm_NewSameLevelSort").IsEnabled = true;
                //menuFormSort.Get("Frm_NewForm").IsEnabled = true;
                //menuFormSort.Get("Frm_NewSubSort").IsEnabled = true;
                //menuFormSort.Get("Frm_EditSort").IsEnabled = true;
                //menuFormSort.Get("Frm_Refresh").IsEnabled = true;
            }
            else
            {
                menuHeight = 205;
                menu = this.menuForm;
                menuForm.Get("Frm_EditForm").IsEnabled = true;
                menuForm.Get("Frm_FormDesignerFix").IsEnabled = true;
                menuForm.Get("Frm_FormDesignerFree").IsEnabled = true;
                menuForm.Get("Frm_Delete").IsEnabled = true;
                menuForm.Get("Frm_Refresh").IsEnabled = true;

            }

            double x = menuPosition.X;
            double y = menuPosition.Y;
            if (x + menuWidth > 220)
            {
                x = x - (x + menuWidth - 220);
            }
            if (y + menuHeight > Application.Current.Host.Content.ActualHeight)
            {
                y = y - (y + menuHeight - Application.Current.Host.Content.ActualHeight);
            }

            menu.SetValue(Canvas.LeftProperty, x);
            menu.SetValue(Canvas.TopProperty, y);
            menu.Visibility = System.Windows.Visibility.Visible;
            menu.Show();
        }

        /// <summary>
        ///  Category Close form action to be performed , Generally refresh the form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frmSameLevelSort_ServiceDoCompletedEvent(object sender, EventArgs e)
        {
            var add = (FrmSortEdit)sender;
            if (add.DialogResult == true)
            {
                this.BindFormTree();
            }
        }
       
        //  Add an action to be performed when closing process categories of , Generally refresh the form 
        void AddEditFlowSortDoCompletedEventHandler(object sender, EventArgs e)
        {
            var add = (NewFlowSort)sender;
            if (add.DialogResult == true)
            {
                this.BindFlowAndFlowSort();
            }
        }
        void EditFrmSortDoCompletedEventHandler(object sender, EventArgs e)
        {
            switch (Glo.TempCmd)
            {
                case "Frm_EditForm":
                case "Frm_NewForm":
                    var frm = (BP.Frm.Frm)sender;
                    if (frm.DialogResult == true)
                    {
                        this.BindFormTree();
                    }
                    break;
                case "Frm_NewFormSort":
                case "Frm_EditFormSort":
                    var add = (BP.Frm.FrmSortEdit)sender;
                    if (add.DialogResult == true)
                    {
                        this.BindFormTree();
                    }
                    break;
                default:
                    MessageBox.Show(" No judgment tag :" + Glo.TempCmd);
                    break;
            }
        }
        #endregion
      
        #region Toolbar 

        private void LoadToolbar()
        {
            var ens = new List<ToolbarItem>();
            ens = ToolbarItem.Instance.GetLists();
            foreach (ToolbarItem en in ens)
            {
                var btn = new ToolbarButton();
                btn.Name = "Btn_" + en.No;
                btn.IsEnabled = en.IsEnable;
                if (!en.IsEnable)
                {
                    btn.Tag = ToolBarEnableIsFlowSensitived;
                }
                btn.Click += new RoutedEventHandler(ToolBar_Click);

                var mysp = new StackPanel();
                mysp.Orientation = Orientation.Horizontal;
                mysp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                mysp.Name = "sp" + en.No;

                var img = new Image();
                var png = new BitmapImage(new Uri("/Images/" + en.No + ".png", UriKind.Relative));
                img.Source = png;
                mysp.Children.Add(img);

                var tb = new TextBlock();
                tb.Name = "tbT" + en.No;
                tb.Text = en.Name;
                tb.FontSize = 12;
                mysp.Children.Add(tb);

                btn.Content = mysp;
                this.toolbar1.AddBtn(btn);
                ToolBarButtonList.Add(btn);
            }
        }
        private void ToolBar_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as ToolbarButton;
            if (null == control)
                return;
            try
            {
                switch (control.Name)
                {
                    case "Btn_ToolBarFrmLab":
                        Glo.OpenWinByDoType("CH", BP.UrlFlag.FrmLib, "", "0", null);
                        return;

                    case "Btn_ToolBarLogin":
                        string url3 = @"/AppDemoLigerUI/Login.aspx?DoType=Logout";
                        Glo.OpenWindow(url3, " Landed ");
                        return;
                    case "Btn_ToolBarFlowUI":
                        this.ChageFlowUI(control);
                        break;
                    case "Btn_ToolBarSave":
                         this.save();
                        break;
                    case "Btn_ToolBarDesignReport":
                        //  Report Design Event 
                        if (SelectedContainer != null)
                        {
                            SelectedContainer.btnDesignerTable();
                        }
                        break;
                    case "Btn_ToolBarCheck":
                        // Click inspection process before , You need to perform a save , Otherwise there will be flow data error 
                        if (SelectedContainer != null)
                        {
                            this.save();
                         
                            SelectedContainer.btnCheck();
                        }
                        break;
                    case "Btn_ToolBarRun":
                        if (SelectedContainer != null)
                            SelectedContainer.btnRun();
                        break;
                    case "Btn_ToolBarEditFlow":
                        Glo.OpenWinByDoType("CH", "FlowP", SelectedContainer.FlowID, null, null);
                        break;
                    case "Btn_ToolBarDeleteFlow":
                        DeleteFlow(SelectedContainer.FlowID);
                        break;
                    case "Btn_ToolBarHelp":
                        Glo.OpenHelp();
                        break;
                    case "Btn_ToolBarToolBox":
                        setToolBoxVisiable(true);
                        break;
                    case "Btn_ToolBarGenerateModel":
                        Glo.OpenWindow("/WF/Admin/XAP/DoPort.aspx?DoType=ExpFlowTemplete&FK_Flow=" + SelectedContainer.FlowID + "&Lang=CH", "Help", 50, 50);
                        break;
                        
                    case "Btn_ToolBarLoadModel":
                        NewFlowHandler(0);
                        break;
                 
                    case "Btn_ToolBarReleaseToFTP":
                        ReleaseToFtp();
                        break;
                    case "Btn_ToolBarShareModel":
                        ViewShareTemplate();
                        break;
                }
            }
            catch (Exception ex)
            {
                Glo.ShowException(ex);
            }
        }
        void ReleaseToFtp()
        {
            if (SelectedContainer == null || SelectedContainer.FlowID != tvwFlow.Selected.Name)
            {
                MessageBox.Show(" Please open the flow chart ");
                return;
            }

            FrmFlowShareToFtp ftp = new FrmFlowShareToFtp(SelectedContainer.FlowID);
            ftp.Show();
        }

        private void setToolBarButtonEnableStatus(bool isEnable)
        {
            foreach (var toolbarButton in ToolBarButtonList)
            {
                if (toolbarButton.Tag != null && toolbarButton.Tag.ToString() == ToolBarEnableIsFlowSensitived)
                {
                    toolbarButton.IsEnabled = isEnable;
                }
            }

        }
        #endregion

      

        public void AddSubTreeNode(TreeNode myNode, DataTable dt, TreeType type)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string parentNo = dr["ParentNo"] as string;
                    if (parentNo == "0" || parentNo != myNode.ID.ToString())
                        continue;

                    TreeNode subNode = new TreeNode();
                    subNode.Title = dr["Name"].ToString();
                    subNode.Name = dr["No"].ToString();
                    subNode.ID = dr["No"].ToString();
                    subNode.IsSort = true;

                    switch (type)
                    {
                        case TreeType.Flow:

                            subNode.Icon = "../Images/MenuItem/FlowSort.png";
                            nodeFlow.Nodes.Add(subNode);
                            break;
                        case TreeType.GPM:

                            subNode.Icon = "../Images/MenuItem/Post.png";
                            nodeGpm.Nodes.Add(subNode);
                            break;
                        case TreeType.MapData:
                            subNode.Icon = "../Images/MenuItem/FlowSheet.png";
                            nodeForm.Nodes.Add(subNode);
                            break;
                        case TreeType.System:
                            break;
                    }

                    myNode.Nodes.Add(subNode);
                    AddSubTreeNode(subNode, dt, type);
                }
            }
            catch (Exception e)
            {
                Glo.ShowException(e);
            }
        }

        private void element_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            Tree treeView = null;
            TreeNode node = null;
            if (sender is Tree)
            {
                treeView = sender as Tree;

                if (null == treeView)
                    return;

                if (treeView == this.tvwSysMenu)
                {
                    return;
                }

                TextBlock tb = e.OriginalSource as TextBlock;
                if (tb != null && tb is DependencyObject)
                {
                    node = Glo.GetParentObject<TreeNode>(tb);
                }

                if (null == node)
                    return;
                else
                {
                    node.IsSelected = true;
                    treeView.Selected = node;
                }
            }
            else //// diable the default silverlight rightmenu
                return;

            // Get current menu position coordinates  
            Point position = e.GetPosition(treeView);
            Point menuPosition = treeView.TransformToVisual(treeView).Transform(position);

            switch (treeView.Name)
            {
                case "tvwFlow":
                    #region

                    //  Adjustment x,y 值 , To prevent the menu is obscured 
                    var x = menuPosition.X;
                    var y = menuPosition.Y;
                    var menuHeight = 380;
                    var menuWidth = 170;
                    if (x + menuWidth > 220)
                    {
                        x = x - (x + menuWidth - 220);
                    }
                    if (y + menuHeight > Application.Current.Host.Content.ActualHeight)
                    {
                        y = y - (y + menuHeight - Application.Current.Host.Content.ActualHeight);
                    }
                    MuFlowTree.SetValue(Canvas.LeftProperty, x);
                    MuFlowTree.SetValue(Canvas.TopProperty, y);
                    MuFlowTree.Show();

                    if (node.isRoot)
                    {
                        MuFlowTree.Get("OpenFlow").IsEnabled = false;
                        MuFlowTree.Get("NewFlow_Blank").IsEnabled = true;
                        MuFlowTree.Get("NewSameLevelFlowSort").IsEnabled = false;
                        MuFlowTree.Get("NewSubFlowSort").IsEnabled = true;
                        MuFlowTree.Get("Delete").IsEnabled = false;
                        MuFlowTree.Get("Edit").IsEnabled = true;
                    }
                    else if (node.IsSort)
                    {
                        Glo.FK_FlowSort = node.ID;
                        MuFlowTree.Get("OpenFlow").IsEnabled = false;
                        MuFlowTree.Get("NewFlow_Blank").IsEnabled = true;
                        MuFlowTree.Get("NewSameLevelFlowSort").IsEnabled = true;
                        MuFlowTree.Get("NewSubFlowSort").IsEnabled = true;
                        MuFlowTree.Get("Delete").IsEnabled = true;
                        MuFlowTree.Get("Edit").IsEnabled = true;
                    }
                    else
                    {
                        MuFlowTree.Get("OpenFlow").IsEnabled = true;
                        MuFlowTree.Get("NewFlow_Blank").IsEnabled = true;
                        MuFlowTree.Get("NewSameLevelFlowSort").IsEnabled = false;
                        MuFlowTree.Get("NewSubFlowSort").IsEnabled = false;
                        MuFlowTree.Get("Edit").IsEnabled = false;
                        MuFlowTree.Get("Delete").IsEnabled = true;
                    }
                    #endregion
                    break;
                case "tvwForm":
                    #region

                 

                    EnableMenuFormTree(node, menuPosition);
                  
                    #endregion
                    break;
                case "tvwOrg":
                    
                    setGMPMenu(node);

                    //  Adjustment x,y 值 , To prevent the menu is obscured 
                    x = menuPosition.X;
                    y = menuPosition.Y;
                    menuHeight = 230;
                    menuWidth = 170;
                    if (x + menuWidth > 220)
                    {
                        x = x - (x + menuWidth - 220);
                    }
                    if (y + menuHeight > Application.Current.Host.Content.ActualHeight)
                    {
                        y = y - (y + menuHeight - Application.Current.Host.Content.ActualHeight);
                    }
                    // Positioning right-click menu  
                    menuOrg.Margin = new Thickness(x, y, 0, 0);
                    // Display the context menu  
                    menuOrg.Show();
                    break;
            }
        }
        private void menu_ItemSelected(object sender, MenuEventArgs e)
        {
            
            Menu menu = sender as Menu;
            if (menu == null) return;
            menu.Hide();

            if (e.Tag == null) return;

            switch (menu.Name)
            {
                case "MuFlowTree":
                    #region

                    switch (e.Tag.ToString())
                    {
                        case "menuExp":
                            BP.Frm.FrmExp exp = new BP.Frm.FrmExp();
                            exp.Show();
                            break;
                        case "Help":
                            Glo.OpenHelp();
                            break;
                        case "OpenFlow":
                            OpenFlow(tvwFlow.Selected.ID, tvwFlow.Selected.Name, tvwFlow.Selected.Title);
                            break;
                        case "NewFlow_Blank":
                            NewFlowHandler(0);
                            break;
                        case "NewFlow_Disk":
                            NewFlowHandler(1);
                            break;
                        case "NewFlow_CCF":
                            NewFlowHandler(2);
                            break;
                        case "NewSameLevelFlowSort":// New sibling Process category 
                            var newFlowSort = new NewFlowSort(this);
                            newFlowSort.InitControl(tvwFlow.Selected.ID, "");
                            newFlowSort.DisplayType = NewFlowSort.DisplayTypeEnum.AddSameLevel;
                            newFlowSort.ServiceDoCompletedEvent += AddEditFlowSortDoCompletedEventHandler;
                            newFlowSort.Show();
                            break;
                        case "NewSubFlowSort":// New lower flow category 
                            var subFlowSort = new NewFlowSort(this);
                            subFlowSort.InitControl(tvwFlow.Selected.ID, "");
                            subFlowSort.DisplayType = NewFlowSort.DisplayTypeEnum.AddSub;
                            subFlowSort.ServiceDoCompletedEvent += AddEditFlowSortDoCompletedEventHandler;
                            subFlowSort.Show();
                            break;
                        case "Edit":
                            Glo.FK_FlowSort = tvwFlow.Selected.ID;
                            var editFlowSort = new NewFlowSort(this);
                            editFlowSort.InitControl(tvwFlow.Selected.ID, tvwFlow.Selected.EditedTitle);
                            editFlowSort.DisplayType = NewFlowSort.DisplayTypeEnum.Edit;
                            editFlowSort.ServiceDoCompletedEvent += AddEditFlowSortDoCompletedEventHandler;
                            editFlowSort.Show();
                            break;
                        case "Delete":
                            var deleteFlowNode = tvwFlow.Selected as TreeNode;
                            if (null == deleteFlowNode)
                                break;
                            
                            if (!deleteFlowNode.IsSort)
                            {
                                Glo.FK_FlowSort = tvwFlow.Selected.ID;
                                DeleteFlow(tvwFlow.Selected.Name);
                            }
                            else
                                DeleteFlowSort(deleteFlowNode.ID);
                            
                            break;
                        case "Share":  // Assign permissions ..
                            var dir = tvwFlow.Selected as TreeNode;
                            if (null == dir)
                            {
                                break;
                            }

                            if (dir.IsSort == false)
                            {
                                MessageBox.Show(" Please select the process tree , You can assign permissions .", " Prompt ", MessageBoxButton.OK);
                                return;
                            }

                            Glo.OpenWinByDoType("CH", UrlFlag.FlowSortP, Glo.FK_Flow, dir.ID, null);
                            break;

                        case "Refresh":
                            this.BindFlowAndFlowSort();
                            break;
                    }
                                        
                    #endregion
                    break;
                case "menuForm":
                case "menuFormSort":
                    #region
                    var selectedNode = this.tvwForm.Selected as TreeNode;
                    if (selectedNode == null)
                        return;
                      
                    Glo.TempCmd = e.Tag.ToString();
                    switch (e.Tag.ToString())
                    {
                        case "Frm_Up": // Move 
                            var ws = Glo.GetDesignerServiceInstance();
                            if (BP.DA.DataType.IsNum(selectedNode.Name))
                                Glo.TempCmd = "FrmTreeUp";
                            else
                                Glo.TempCmd = "FrmUp";
                            ws.DoTypeAsync(Glo.TempCmd, selectedNode.Name, null, null, null, null);
                            ws.DoTypeCompleted += designer_DoTypeCompleted;
                            break;
                        case "Frm_Down": // Move 
                            var wsDown = Glo.GetDesignerServiceInstance();

                            if (BP.DA.DataType.IsNum(selectedNode.Name))
                                Glo.TempCmd = "FrmTreeDown";
                            else
                                Glo.TempCmd = "FrmDown";
                            wsDown.DoTypeAsync(Glo.TempCmd, selectedNode.Name, null, null, null, null);
                            wsDown.DoTypeCompleted += designer_DoTypeCompleted;
                            break;
                        case "Frm_EditForm": // Form Properties 
                            BP.Frm.Frm frm = new BP.Frm.Frm();
                            frm.BindFrm(selectedNode.Name);
                            frm.HisMainPage = this;
                            frm.Show();
                            break;
                        case "Frm_NewForm": // New Form 
                            BP.Frm.Frm frm1 = new BP.Frm.Frm();
                            frm1.SortNo = Glo.FK_FormSort;
                            frm1.BindNew();
                            frm1.HisMainPage = this;
                            frm1.Show();
                            break;
                        case "Frm_FormDesignerFix": // Form design fool 
                            Glo.OpenWinByDoType("CH", UrlFlag.FormFixModel, selectedNode.Name, null, null);
                            break;
                        case "Frm_FormDesignerFree": // Design free Form 
                           string title=" Form ID: {0}  Storage Table :{1}  Name :{2}";
                           title = string.Format(title, selectedNode.Name, "Sys_MapData", selectedNode.Title);
                           OpenBPForm(BPFormType.FormFlow, title, selectedNode.Name, selectedNode.Name);
                            break;
                        case "Help":
                            Glo.OpenHelp();
                            break;
                        case "Frm_NewSameLevelSort": // New sibling Form Category 
                            BP.Frm.FrmSortEdit frmSameLevelSort = new BP.Frm.FrmSortEdit();
                            frmSameLevelSort.No = selectedNode.ID;
                            frmSameLevelSort.TB_Name.Text = "New Form Sort";
                            frmSameLevelSort.ServiceDoCompletedEvent += new EventHandler<DoCompletedEventArgs>(frmSameLevelSort_ServiceDoCompletedEvent);
                            frmSameLevelSort.DisplayType = FrmSortEdit.DisplayTypeEnum.AddSameLevel;
                            frmSameLevelSort.Show();
                            break;
                        case "Frm_NewSubSort":// New lower form category 
                            BP.Frm.FrmSortEdit frmSubLevelSort = new BP.Frm.FrmSortEdit();
                            frmSubLevelSort.No = selectedNode.ID;
                            frmSubLevelSort.TB_Name.Text = "New Form Sort";
                            frmSubLevelSort.ServiceDoCompletedEvent += new EventHandler<DoCompletedEventArgs>(frmSameLevelSort_ServiceDoCompletedEvent);
                            frmSubLevelSort.DisplayType = FrmSortEdit.DisplayTypeEnum.AddSub;
                            frmSubLevelSort.Show();
                            break;
                        case "Frm_EditSort": // Editor 
                            BP.Frm.FrmSortEdit frmSortEdit1 = new BP.Frm.FrmSortEdit();
                            frmSortEdit1.No = selectedNode.ID;
                            frmSortEdit1.TB_Name.Text = selectedNode.Title;
                            frmSortEdit1.ServiceSaveEnCompletedEvent += new EventHandler<SaveEnCompletedEventArgs>(frmSameLevelSort_ServiceDoCompletedEvent);
                            frmSortEdit1.DisplayType = FrmSortEdit.DisplayTypeEnum.Edit;
                            frmSortEdit1.Show();
                            break;
                        case "Frm_Delete": // Delete 
                            var deleteFlowNode = this.tvwForm.Selected as TreeNode;
                            if (null == deleteFlowNode)
                                break;

                            if (deleteFlowNode.isRoot)
                            {
                                MessageBox.Show(" The root can not be deleted .");
                                return;
                            }

                            if (MessageBox.Show(" You sure you want to delete it ?", "ccflow", MessageBoxButton.OKCancel)
                                == MessageBoxResult.Cancel)
                                return;

                            if (deleteFlowNode.IsSort == true)
                            {
                                if (deleteFlowNode.Nodes.Count > 0)
                                {
                                    if (MessageBox.Show(" Select items containing child nodes , Will remove it along with the child node ?", "ccflow", MessageBoxButton.OKCancel)
                                == MessageBoxResult.Cancel)
                                        return;
                                }
                                var ws1 = Glo.GetDesignerServiceInstance();
                                Glo.TempCmd = "DeleteFrmSort";
                                ws1.DoTypeAsync(Glo.TempCmd, deleteFlowNode.ID, null, null, null, null);
                                ws1.DoTypeCompleted += designer_DoTypeCompleted;
                            }
                            else
                            {
                                var ws2 = Glo.GetDesignerServiceInstance();
                                Glo.TempCmd = "DeleteFrm";
                                ws2.DoTypeAsync(Glo.TempCmd, deleteFlowNode.Name, null, null, null, null);
                                ws2.DoTypeCompleted += designer_DoTypeCompleted;

                            }
                            break;
                        case "Frm_Refresh": // Refresh 
                            this.BindFormTree();
                            break;
                        case "Frm_Imp":  //Imp
                            BP.Frm.FrmImp imp = new BP.Frm.FrmImp();
                            imp.Show();
                            break;
                        case "Frm_FormSln":
                            Glo.OpenDialog("/WF/MapDef/Sln.aspx?FK_MapData=" + selectedNode.Name, " Forms permission scheme ");
                            break;
                        default:
                            MessageBox.Show(" Function is not complete :" + Glo.TempCmd);
                            break;
                    }
                    #endregion
                    break;
                case "menuApp":
                    switch (e.Tag.ToString())
                    {
                        case "ExceptionLog":
                            BP.SL.OutputChildWindow.ShowException();
                            break;
                        case "Help":
                            Glo.OpenHelp();
                            break;
                    }
                    break;
                case "menuOrg":
                    menu_ItemOrg_Click(sender, e);
                    break;
            }
           
        }
        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            Liquid.Menu menu = sender as Liquid.Menu;
            menu.Hide();
        }
        void designer_DoTypeCompleted(object sender, DoTypeCompletedEventArgs e)
        {
            if (null != e.Error)
            {
                Glo.ShowException(e.Error);
                return;
            }
            switch (Glo.TempCmd)
            {
                case "DeleteFrmSort":
                case "DeleteFrm":
                case "FrmUp":   // Mobile .
                case "FrmDown": // Mobile .
                    if (e.Result != null)
                        MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
                    else
                        this.BindFormTree();
                    return;
                default:
                    break;
            }

            //DataSet ds = new DataSet();
            //ds.FromXml(e.Result);

            //bindTreeFlowData(ds.Tables[0]);
            //bindTreeSys(ds.Tables[1]);
        
        }

        private void TabControlTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TabItem tbItem = (sender as TabControl).SelectedItem as TabItem;
            if (null == tbItem) return;
            string name = tbItem.Name;

            switch (tbItem.Name)
            {
                case "tbiFlowLibrary":// Process tree 

                    break;
                case "tbiFormLibrary":// Form Library 

                    break;
                case "tbiOrg":// Process Optimization 
                    if (!isGMPTreeInited)
                        this.BingTreeOrg();
                    break;
                case "tbiSysManger":// System Maintenance 
                    Glo.OpenWindow("/GPM/Default.aspx?RefNo=CCFlowBPM", " System Maintenance ");
                    this.tbcLeft.SelectedIndex = 0;
                    break;

            }
        }

        void bindTreeSys(DataTable dt)
        {

            this.tvwSysMenu.Nodes.Clear();
            TreeNode liP = new TreeNode();
            foreach (DataRow dr in dt.Rows)
            {
                string no = dr["No"];
                string name = dr["Name"];
                string lab = dr["CH"];
                string w = dr["W"];
                string h = dr["H"];
                string url = dr["Url"];
                string icon = dr["Icon"];

                TreeNode tvwNodeSysMenu = new TreeNode();
                if (string.IsNullOrEmpty(icon) == false)
                    tvwNodeSysMenu.Icon = icon;

                tvwNodeSysMenu.Title = lab;
                tvwNodeSysMenu.Tag = dr;
                if (no.Length == 2)
                {
                    liP = tvwNodeSysMenu;
                    this.tvwSysMenu.Nodes.Add(tvwNodeSysMenu);
                }
                else
                {
                    liP.Nodes.Add(tvwNodeSysMenu);
                }
            }

            tvwSysMenu.ExpandAll();

            this.tvwSysMenu.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                #region
                Tree tree = sender as Tree;
                TreeNode node = null;
                if (null != tree)
                    node = tree.Selected as TreeNode;
                else return;

                if (null == node) return;

                if (tree == this.tvwSysMenu)
                {
                    DataRow dr = node.Tag as DataRow;
                    string w = dr["W"];
                    string h = dr["H"];
                    string url = dr["Url"];

                    Glo.OpenWindow(Glo.BPMHost + url, "name", int.Parse(h), int.Parse(w));
                };
                #endregion
            };
        }
        void bindTreeFlowData(DataTable dt)
        {
            //this.tvwFlowDataEnum.Nodes.Clear();
            //liP = new TreeNode();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string no = dr["No"];
            //    string name = dr["Name"];
            //    string lab = dr["CH"];
            //    string w = dr["W"];
            //    string h = dr["H"];
            //    string url = dr["Url"];
            //    string icon = dr["Icon"];
            //    TreeNode tvwNodeFlow = new TreeNode();
            //    if (string.IsNullOrEmpty(icon) == false)
            //        tvwNodeFlow.Icon = icon;

            //    tvwNodeFlow.Title = lab;
            //    tvwNodeFlow.Tag = dr;
            //    if (no.Length == 2)
            //    {
            //        liP = tvwNodeFlow;
            //        this.tvwFlowDataEnum.Nodes.Add(tvwNodeFlow);
            //    }
            //    else
            //    {
            //        liP.Nodes.Add(tvwNodeFlow);
            //    }
            //}
            //tvwFlowDataEnum.ExpandAll();
        }
      
        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            this.LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            this.LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;
        }

        void Content_Resized(object sender, EventArgs e)
        {
            this.LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            this.LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;

            if ( ccFormContainer != null)
            {
                ccFormContainer.content.Width = LayoutRoot.Width;
                ccFormContainer.content.Height = LayoutRoot.Height;
            }
        }

        void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (LayoutRoot.Height < 100) return;

            tbcLeft.Height = LayoutRoot.Height - 70;//imageLogo.Height  margion

            var height = tbcLeft.Height - 35;// tabcontrol.Height
            tvwFlow.Height = height ;
            tvwForm.Height = height ;
            this.tvwOrg.Height = height ;
            tvwSysMenu.Height = height ;

            this.flowToolBoxIcon.lbTools.Height = height - 20 ;
         
            tbDesigner.Height = LayoutRoot.Height  -35 ;// toolBar.Height
            tbDesigner.Width = LayoutRoot.ActualWidth - 285;// tbcLeft.ActualWidth + magrion
        }
     
    }
}