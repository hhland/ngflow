#region
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Browser;
using Silverlight;
using WF.WS;
using WF;
using BP.SL;
using BP.Controls;
using System.Collections.Generic;

#endregion
namespace BP
{

    public class UrlFlag
    {
        /// <summary>
        ///  Node Properties 
        /// </summary>
        public const string NodeP = "NodeP";
        /// <summary>
        ///  Directory Permissions 
        /// </summary>
        public const string FlowSortP = "FlowSortP";
        /// <summary>
        ///  Process Attributes 
        /// </summary>
        public const string FlowP = "FlowP";
        /// <summary>
        ///  Running processes 
        /// </summary>
        public const string RunFlow = "RunFlow";
        /// <summary>
        ///  Process inspection 
        /// </summary>
        public const string FlowCheck = "FlowCheck";
        /// <summary>
        ///  Report Definition 
        /// </summary>
        public const string WFRpt = "WFRpt";
        /// <summary>
        ///  Conditions set direction 
        /// </summary>
        public const string Dir = "Dir";
        /// <summary>
        ///  Nodes form design - Fool 
        /// </summary>
        public const string MapDefFixModel = "MapDefFixModel";
        /// <summary>
        ///  Nodes form design - Free 
        /// </summary>
        public const string MapDefFreeModel = "MapDefFreeModel";
        /// <summary>
        ///  Form Design - Fool 
        /// </summary>
        public const string FormFixModel = "FormFixModel";
        /// <summary>
        ///  Form Design - Free 
        /// </summary>
        public const string FormFreeModel = "FormFreeModel";
        /// <summary>
        ///  Node positions 
        /// </summary>
        public const string StaDef = "StaDef";
        /// <summary>
        ///  Process Form 
        /// </summary>
        public const string FlowFrms = "FlowFrms";
        /// <summary>
        ///  Form Library 
        /// </summary>
        public const string FrmLib = "FrmLib";
    }
    /// <summary>
    ///  Overall situation 
    /// </summary>
    public class Glo
    {
        static Dictionary<string, List<SEnum>> dicEnums = new Dictionary<string, List<SEnum>>();
        public static List<SEnum> GetEnumByTypeKey(string key)
        {
            List<SEnum> lists = null;
            if (dicEnums.ContainsKey(key))
                lists = dicEnums[key];
            else
            {
                lists = new List<SEnum>();
            }
            return lists;
        }
        public static void Element_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        static CustomCursor cCursor = null;// which is used to replace the default cursor of UIElement
        public static CustomCursor CCursor
        {
            get
            {
                if (null == cCursor)
                    cCursor = new CustomCursor(MainPage.Instance);
                return cCursor;
            }
        }        /// <summary>
        ///   Node is automatically adjusts the size of the container movement 
        /// </summary>
        public static bool IsDragNodeResizeContainer = true;
        /// <summary>
        ///   Open Form Designer ,url 和sl Subform mode 
        /// </summary>
        public static bool UrlOrForm = false;

        public static OSModel OsModel = OSModel.WorkFlow;

        public static BP.Controls.TreeNode CurNodeFlow;
        public static BP.Controls.TreeNode CurNodeOrg = new TreeNode();
        public static BP.Controls.TreeNode CurrNodeForm;

   
        /// <summary>
        ///  Temporary variables .
        /// </summary>
        public static string TempCmd = null;
        /// <summary>
        ///   The final type of process , After the process tree for rebinding , Re-open the process category last operation 
        /// </summary>
        public static string FK_FlowSort = "01";
        /// <summary>
        ///  The current process ID 
        /// </summary>
        public static string FK_Flow = null;
        public static string FK_FormSort = "01";

        #region  Control Operation 
        public static bool Ctrl_DDL_SetSelectVal(ComboBox ddl, string setVal)
        {
            string oldVal = "";
            foreach (ComboBoxItem item in ddl.Items)
            {
                if (item.IsEnabled == true)
                {
                    oldVal = item.Tag.ToString();
                    item.IsSelected = false;
                    break;
                }
            }
            foreach (ComboBoxItem item in ddl.Items)
            {
                if (item.Tag.ToString() == setVal)
                {
                    item.IsSelected = true;
                    return true;
                }
            }

            foreach (ComboBoxItem item in ddl.Items)
            {
                if (item.Tag.ToString() == oldVal)
                {
                    item.IsSelected = true;
                    break;
                }
            }
            return false;
        }
        public static void Ctrl_DDL_BindDataTable(ComboBox ddl, DataTable dt, string selectVal)
        {
            ddl.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                ComboBoxItem li = new ComboBoxItem();
                li.Content = dr[1].ToString();
                li.Tag = dr[0].ToString();
                if (dr[0].ToString() == selectVal)
                    li.IsSelected = true;
                ddl.Items.Add(li);
            }
        }
        public static int GetDDLValOfInt(ComboBox cb)
        {
            ComboBoxItem it = cb.SelectedItem as ComboBoxItem;
            if (it == null)
                throw new Exception(" No choice data " + cb.Name);
            return int.Parse(it.Tag.ToString());
        }
        public static string GetDDLValOfString(ComboBox cb)
        {
            ComboBoxItem it = cb.SelectedItem as ComboBoxItem;
            if (it == null)
                throw new Exception(" No choice data " + cb.Name);
            return it.Tag.ToString();
        }
        #endregion
            
        #region  Property 

        public static bool IsDbClick
        {
            get
            {
                return BP.SL.Glo.IsDbClick;
            }
        }

        /// <summary>
        ///  Current  BPMHost 
        /// </summary>
        private static string _BPMHost = null;
        /// <summary>
        ///  Current BPMHost 
        ///  Such as :http://demo.ccflow.org:8888
        /// </summary>
        public static string BPMHost
        {
            get
            {
                if (_BPMHost != null)
                    return _BPMHost;

                try
                {
                    string myurl = System.Windows.Browser.HtmlPage.Document.DocumentUri.AbsoluteUri;

                    myurl = myurl.Replace("//", "");
                    int posStart = myurl.IndexOf("/");

                    string appPath = myurl.Substring(posStart);
                    if (appPath.Contains("/WF/Admin"))
                    {
                        appPath = appPath.Substring(0, appPath.IndexOf("/WF/Admin", StringComparison.CurrentCultureIgnoreCase));
                    }
                    else
                    {
                        appPath = appPath.Substring(0, appPath.IndexOf("/WF/", StringComparison.CurrentCultureIgnoreCase));
                    }
                    var location = (HtmlPage.Window.GetProperty("location")) as ScriptObject;
                    string host = location.GetProperty("host").ToString();
                    _BPMHost = "http://" + host + appPath;
                }
                catch (Exception e)
                {
                    BP.SL.LoggerHelper.Write(e);
                }
                return _BPMHost;
            }
        }

        public static int ScreenWidth
        {
            get
            {
                int width = 0;
                try
                {
                    double val = (double)HtmlPage.Window.Invoke("GetBrowserWidth");
                    width = (int)val;
                }
                catch { }

                return width <= 0 ? 1024 : width;
            }
        }
        public static int ScreenHeight
        {
            get
            {
                int height = 0;
                try
                {
                    double val = (double)HtmlPage.Window.Invoke("GetBrowserHeight");
                    height = (int)val;
                }
                catch (Exception) { }

                return height <= 0 ? 768 : height;
            }
        }


        #endregion

        #region  Get Service 
        /// <summary>
        ///  Get WebService Object  
        /// </summary>
        /// <returns></returns>
        public static WSDesignerSoapClient GetDesignerServiceInstance()
        {
            var basicBinding = new BasicHttpBinding()
            {
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                Name = "WSDesignerSoap"
            };
            basicBinding.Security.Mode = BasicHttpSecurityMode.None;
            string url = Glo.BPMHost + "/WF/Admin/XAP/WebService.asmx";
            url = url.Replace("//", "/");
            url = url.Replace(":/", "://");

            var endPoint = new EndpointAddress(url);
            var ctor =
                typeof(WSDesignerSoapClient).GetConstructor(
                new Type[] {
                    typeof(Binding), 
                    typeof(EndpointAddress)
                });
            return (WSDesignerSoapClient)ctor.Invoke(
                new object[] { basicBinding, endPoint });
        }
      
  
        #endregion

        #region  Called window 

        public static void OpenHelp()
        {
            Glo.OpenWindow("http://online.ccflow.org/","");

        }
        /// <summary>
        ///  Set to open the property page window 
        /// </summary>
        /// <param name="lang"> Language </param>
        /// <param name="dotype"> Window Types </param>
        /// <param name="fk_flow"> Workflow ID</param>
        /// <param name="node1"> Node 1</param>
        /// <param name="node2"> Node 2</param>
        public static void OpenWinByDoType(string lang, string dotype, string fk_flow, string node1, string node2)
        {
            string url = "";
            switch (dotype)
            {
                case UrlFlag.StaDef:
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=StaDef&PK=" + node1 + "&Lang=CH";
                    Glo.OpenDialog(Glo.BPMHost + url, " Carried out ", 500, 400);
                    return;
                case UrlFlag.FrmLib:
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=FrmLib&FK_Flow=" + fk_flow + "&FK_Node=0&Lang=CH";
                    Glo.OpenWindow(Glo.BPMHost + url, " Carried out ", 800, 760);
                    return;
                case UrlFlag.FlowFrms:
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=FlowFrms&FK_Flow=" + fk_flow + "&FK_Node="+node1+"&Lang=CH";
                    Glo.OpenWindow(Glo.BPMHost + url, " Carried out ", 800, 760);
                    return;
                case UrlFlag.FlowSortP:
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=En&EnName=BP.WF.FlowSort&PK=" + node1 + "&Lang=CH";
                    Glo.OpenDialog(Glo.BPMHost + url, " Carried out ", 600, 500);
                    return;
                case UrlFlag.NodeP:
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=En&EnName=BP.WF.Node&PK=" + node1 + "&Lang=CH";
                    Glo.OpenDialog(Glo.BPMHost + url, " Carried out ", 600, 500);
                    return;
                case UrlFlag.FlowP: //  Node attributes and process attributes .
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=En&EnName=BP.WF.Flow&PK=" + fk_flow + "&Lang=CH";
                    Glo.OpenDialog(Glo.BPMHost + url, "", 500, 400);
                    return;


                case UrlFlag.MapDefFixModel: // SDK Form Design .
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=MapDefFixModel&FK_MapData=ND" + node1 + "&FK_Node=" + node1 + "&Lang=CH&FK_Flow=" + fk_flow;
                    Glo.OpenDialog(url, " Nodes form design ");
                    return;
                case UrlFlag.MapDefFreeModel: //  Freedom Form Design .

                    string fk_MapData = "ND" + node1;
                    string title = " Form ID: {0}  Storage Table :{1}  Name :{2}";
                    title = string.Format(title, fk_MapData, fk_MapData, node2);
                    if (Glo.UrlOrForm)
                    {
                        url = "/WF/Admin/XAP/DoPort.aspx?DoType=MapDefFreeModel&FK_MapData="
                        + fk_MapData + "&FK_Node=" + node1 + "&Lang=CH&FK_Flow=" + fk_flow;
                        MainPage.Instance.OpenBPForm(BPFormType.FormNode, title, url);
                    }
                    else
                    {
                        MainPage.Instance.OpenBPForm(BPFormType.FormNode, title, fk_MapData, fk_flow);
                    }
                  
                    break;
                case UrlFlag.FormFixModel: //  Nodes form design .
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=MapDefFixModel&FK_MapData=" + fk_flow;
                 
                    Glo.OpenDialog(Glo.BPMHost + url, " Nodes form design ");
                    return;
                case UrlFlag.FormFreeModel: //  Nodes form design .
                    url = "/WF/Admin/XAP/DoPort.aspx?DoType=MapDefFreeModel&FK_MapData=" + fk_flow;
                    Glo.OpenDialog(Glo.BPMHost + url, " Nodes form design ");
                    return;
                case UrlFlag.Dir: //  Direction of the condition .
                    url = "/WF/Admin/ConditionLine.aspx?FK_Flow=" + fk_flow + "&FK_MainNode=" + node1 + "&FK_Node=" + node1 + "&ToNodeID=" + node2 + "&CondType=2&Lang=CH";
                    Glo.OpenDialog(Glo.BPMHost + url, " Direction of the condition ", 550, 500);
                    break;
                case UrlFlag.RunFlow: //  Running processes .
                    url = "/WF/Admin/TestFlow.aspx?FK_Flow=" + fk_flow + "&Lang=CH";
                    Glo.OpenWindow(Glo.BPMHost + url, " Running processes ", 850, 990);
                    return;
                case UrlFlag.FlowCheck: //  Process Design .
                    url = "/WF/Admin/DoType.aspx?RefNo=" + fk_flow + "&DoType=" + dotype + "&Lang=CH";
                    Glo.OpenWindow(Glo.BPMHost + url, " Running processes ", 850, 990);
                    break;
                case "LoginPage": //  Log in .
                    url = @"/AppDemoLigerUI/Login.aspx?DoType=Logout";
                    Glo.OpenWindow(Glo.BPMHost + url, " Log in ", 850, 990);
                    break;
                case UrlFlag.WFRpt: //  Process Design .
                    url = "/WF/Admin/XAP/DoPort.aspx?RefNo=" + fk_flow + "&DoType=" + dotype + "&Lang=CH&PK="+fk_flow;
                    Glo.OpenDialog(Glo.BPMHost + url, " Running processes ", 850, 990);
                    break;
                default:
                    MessageBox.Show(" No judgment url Execution mark :" + dotype);
                    break;
            }
        }

        public static void OpenMax(string url, string title)
        {
            OpenWindowOrDialog(url, title, WindowModelEnum.Max);
        }
        public static void OpenDialog(string url, string title, int h = 0, int w = 0)
        {
            OpenWindowOrDialog(url, title,  WindowModelEnum.Dialog,h,w);
        }
        public static void OpenWindow(string url, string title, int h = 0, int w = 0)
        {
            OpenWindowOrDialog(url, title, WindowModelEnum.Window,h,w);
        }
     
        /// <summary>
        ///  Pop-page window 
        /// </summary>
        /// <param name="url"> Web address </param>
        private static void OpenWindowOrDialog(string url, string title, WindowModelEnum windowModel, int height = 0, int width = 0, int left = 0, int top = 0, bool resizable = true)
        {
            if (!url.Contains("ttp://") || !url.Contains("http"))
                url = Glo.BPMHost + url;

            try
            {
                if (windowModel == WindowModelEnum.Dialog)
                {
                    BrowserInformation info = HtmlPage.BrowserInformation;
                    if (!info.Name.Contains("Netscape"))
                    {
                        HtmlPage.Window.Eval(
                        string.Format("window.showModalDialog('{0}',window,'dialogHeight:600px;dialogWidth:950px;help:no;scroll:auto;resizable:yes;status:no;');",
                            url));
                    }
                    else
                    {
                        title = title.Replace(" ", "_");
                        //HtmlPage.Window.Invoke("showDialog", url, title, height, width);
                        OpenWindow(url, title, height, width);
                        return;
                        //HtmlPopupWindowOptions options = new HtmlPopupWindowOptions()
                        //{
                        //    Directories = false,
                        //    Location = false,
                        //    Menubar = false,
                        //    Status = false,
                        //    Toolbar = false,
                        //    Width = 1024,
                        //    Height = 600,
                        //    Scrollbars = true,
                        //    Resizeable = false
                        //};
                        //options.Left = (Glo.ScreenWidth - options.Width) / 2;
                        //options.Top = (Glo.ScreenHeight - options.Height) / 2;
                        //HtmlPage.PopupWindow(new Uri(url), "self", options);
                    }
                }
                else if (windowModel == WindowModelEnum.Max)
                {
                    string tmp = "window.open('{0}','_blank','left=0,top=0,height={1},width={2},resizable={3},scrollbars=yes,help=no,toolbar=no,menubar=no,scrollbars=yes,status=yes,location=no')";
                    width = 0 < width ? width : ScreenWidth;
                    height = 0 < height ? height : ScreenHeight - 100;//  System tray height ??
                    string resize = resizable ? "yes" : "no";
                    url = string.Format(tmp, url, height, width, resize);
                    HtmlPage.Window.Eval(url);
                }
                else
                {
                    if (0 < height && 0 < width)
                    {
                        string tmp = "window.open('{0}','_blank','height={1},width={2},resizable=yes,help=no,toolbar =no, menubar=no, scrollbars=yes,status=yes,location=no')";
                        url = string.Format(tmp, url, height, width);
                    }
                    else
                    {
                        url = "window.open('" + url + "','_blank')";
                    }
                    HtmlPage.Window.Eval(url);
                }
            }
            catch (Exception e)
            {
                Glo.ShowException(e);
            }
        }
        #endregion

        #region  Abnormal , Load Tips , Log in , Color 
        public static void ShowException(Exception e, string error = "")
        {
            BP.SL.LoggerHelper.Write(e);

            if (string.IsNullOrEmpty(error))
                MessageBox.Show(e.Message);
            else
            {
                MessageBox.Show(error + ":" + e.Message + "\t\n@ For details, please see the exception log ");
            }
        }

        static BP.Frm.Waiting waiting;
        /// <summary>
        ///  Show Waiting Forms 
        /// </summary>
        public static void Loading(bool showIt)
        {
            if (null == waiting)
                waiting = new Frm.Waiting();

            if (!showIt)
            {
                waiting.DialogResult = true;
                //waiting.Close();
            }
            else
            {
                waiting.Show();
            }
        }

        static ChildWindow login;
        public static void Login()
        {
            if (login == null)
            {
                login = new AdminLogin();
                login.Closed += (object sender, EventArgs e)=>
                {
                    if( login.DialogResult == true)
                        MainPage.Instance.LoginCompleted();
                };
            }
            login.Show();
        }

        /// <summary>
        ///  Is Digital 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static bool IsNum(string exp)
        {
            try
            {
                Int64 i = Int64.Parse(exp);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  Get associated with a particular object type elements 
        /// </summary>
        /// <typeparam name="T"> To match the type element </typeparam>
        /// <param name="obj"></param>
        /// <param name="name"> Specific name , May be empty </param>
        /// <returns></returns>
        public static T GetParentObject<T>(DependencyObject obj, string name= "") where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
           
            //bool flag =typeof(Border).IsInstanceOfType(parent);

            while (parent != null)
            {
                if (parent is T
                    && (string.IsNullOrEmpty(name) | ((T)parent).Name == name))
                {
                    return (T)parent;
                }
              
               parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        public static T GetChildObject<T>(DependencyObject obj, string name = "") where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;
  
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
  
                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
         }


        /// <summary>
        ///  Get TreeView Control with the specified name TreeNode
        /// </summary>
        /// <param name="myNode"></param>
        /// <param name="nodeName"></param>
        /// <returns> Match TreeNode</returns>
        public static TreeNode findNode(TreeNode myNode, string nodeName)
        {
            TreeNode node = null;

            if (myNode.Name == nodeName || myNode.EditedTitle == nodeName)
                return myNode;

            else if (myNode.HasChildren)
            {
                foreach (TreeNode n in myNode.Nodes)
                {
                    node = findNode(n, nodeName);
                    if (null != node)
                        break;
                }
                return node;
            }

            return node;
        }

        #endregion

    }
}
