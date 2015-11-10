using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Browser;
using Silverlight;
using BP.Sys;
using WorkNode.FF;
using BP.Sys.SL;

namespace WorkNode
{
    public class DBType
    {
        public const string MSSQL = "MSSQL";
        public const string Oracle = "Oracle";
        public const string MySQL = "MySQL";
        public const string DB2 = "DB2";
    }
    public class Glo
    {
        public static DataSet FrmDS = null;

        #region  Get property 
        /// <summary>
        ///  Get attribute nodes String
        /// </summary>
        /// <param name="attrKey">Key</param>
        /// <returns> Attribute nodes </returns>
        public static string GetNodeAttrByKeyString(string attrKey)
        {
            DataTable dt = FrmDS.Tables["WF_Node"];
            return dt.Rows[0][attrKey].ToString();
        }
        /// <summary>
        ///  Get attribute nodes Int
        /// </summary>
        /// <param name="attrKey">Key</param>
        /// <returns> Attribute nodes </returns>
        public static int GetNodeAttrByKeyInt(string attrKey)
        {
            DataTable dt = FrmDS.Tables["WF_Node"];
            return int.Parse(dt.Rows[0][attrKey].ToString());
        }
        #endregion   Get property 

        /// <summary>
        ///  Get WebService Object 
        /// </summary>
        /// <returns></returns>
        public static FF.CCFlowAPISoapClient GetCCFlowAPISoapClientServiceInstance()
        {
            var basicBinding = new BasicHttpBinding() { MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647, Name = "CCFlowAPISoapClient" };
            basicBinding.Security.Mode = BasicHttpSecurityMode.None;
            var endPoint = new EndpointAddress(Glo.BPMHost + "/WF/WorkOpt/CCFlowAPI.asmx");
            var ctor = typeof(CCFlowAPISoapClient).GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
            return (CCFlowAPISoapClient)ctor.Invoke(new object[] { basicBinding, endPoint });
        }
        public static string FK_MapData
        {
            get
            {
                return "ND" + Glo.FK_Node;
            }
        }
        /// <summary>
        ///  Current BPMHost 
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
                var location = (HtmlPage.Window.GetProperty("location")) as ScriptObject;
                _BPMHost = "http://" + location.GetProperty("host");
                return _BPMHost;
            }
        }

        public static void OpenWindowOrDialog(string url, string title, int height, int width, BP.WindowModelEnum windowModel)
        {
            if (url.Contains("ttp://") == false)
                url = Glo.BPMHost + url;

            if (BP.WindowModelEnum.Dialog == windowModel)
            {
                HtmlPage.Window.Eval(
                    string.Format("window.showModalDialog('{0}',window,'dialogHeight:" + height + "px;dialogWidth:" + width + "px;help:no;scroll:auto;resizable:yes;status:no;');",
                        url));
            }
            else
            {
                HtmlPage.Window.Eval("window.open('" + url + "','_blank')");
            }
        }

        public static string CompanyID = "CCFlow";
        public static string AppCenterDBType = "MSSQL";
        public static void SetComboBoxSelected(ComboBox cb, string val)
        {
            foreach (ComboBoxItem item in cb.Items)
            {
                item.IsSelected = false;
            }
            foreach (ComboBoxItem item in cb.Items)
            {
                if (item.Tag.ToString() == val)
                    item.IsSelected = true;
            }
            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
        }
        public static void BindComboBoxFontSize(ComboBox cb, double selectDB)
        {
            cb.Items.Clear();
            for (int i = 6; i < 25; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = i.ToString();
                if (i.ToString() == selectDB.ToString())
                    cbi.IsSelected = true;
                cb.Items.Add(cbi);
            }
            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
        }
        /// <summary>
        ///  Bind values 
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="kvs"></param>
        /// <param name="selectVal"></param>
        public static void BindDDL(ComboBox cb, string kvs, string selectVal)
        {
            string[] strs = kvs.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kk = str.Split('=');
                string no = kk[0].ToString();
                string name = kk[1].ToString();

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = name;
                cbi.Tag = no;
                if (no == selectVal)
                    cbi.IsSelected = true;
                else
                    cbi.IsSelected = false;
                cb.Items.Add(cbi);
            }
            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
        }
        /// <summary>
        ///  Set Binding drop-down box 
        /// </summary>
        /// <param name="cb"> Drop-down box </param>
        /// <param name="dataSet"> Data Sources </param>
        /// <param name="tableName"> Table name </param>
        /// <param name="noCol"> Number column </param>
        /// <param name="nameCol"> Name column </param>
        /// <param name="selectVal"> Selected data items </param>
        public static void BindDDL(ComboBox cb, DataSet dataSet, int tableIdx, string noCol, string nameCol, string selectVal)
        {
            BindDDL(cb, dataSet, dataSet.Tables[tableIdx].TableName, noCol, nameCol, selectVal);
        }
        /// <summary>
        ///  Set Binding drop-down box 
        /// </summary>
        /// <param name="cb"> Drop-down box </param>
        /// <param name="dataSet"> Data Sources </param>
        /// <param name="tableName"> Table name </param>
        /// <param name="noCol"> Number column </param>
        /// <param name="nameCol"> Name column </param>
        /// <param name="selectVal"> Selected data items </param>
        public static void BindDDL(ComboBox cb, DataSet dataSet, string tableName, string noCol, string nameCol, string selectVal)
        {
            cb.Items.Clear();
            DataTable dt = dataSet.Tables[tableName];
            foreach (DataRow dr in dt.Rows)
            {
                string no = dr[noCol].ToString();
                string name = dr[nameCol].ToString();

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = name;
                cbi.Tag = no;
                if (no == selectVal)
                    cbi.IsSelected = true;
                else
                    cbi.IsSelected = false;
                cb.Items.Add(cbi);
            }

            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
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
        public static void BindComboBoxLineBorder(ComboBox cb, double selectDB)
        {
            cb.Items.Clear();
            for (int i = 1; i < 15; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = i.ToString();
                if (i.ToString() == selectDB.ToString())
                    cbi.IsSelected = true;
                else
                    cbi.IsSelected = false;
                cb.Items.Add(cbi);
            }

            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
        }
        public static Color ToColor(string colorName)
        {
            try
            {
                if (colorName.StartsWith("#"))
                    colorName = colorName.Replace("#", string.Empty);
                int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
                return new Color()
                {
                    A = Convert.ToByte((v >> 24) & 255),
                    R = Convert.ToByte((v >> 16) & 255),
                    G = Convert.ToByte((v >> 8) & 255),
                    B = Convert.ToByte((v >> 0) & 255)
                };
            }
            catch
            {
                return Colors.Black;
            }
        }
        public static void WinOpen(string url)
        {
            HtmlPage.Window.Eval("window.showModalDialog('" + url + "',window,'dialogHeight:600px;dialogWidth:800px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
        }
        public static void WinOpenModalDialog(string url, int h, int w)
        {
            HtmlPage.Window.Eval("window.showModalDialog('" + url + "',window,'dialogHeight:" + h + "px;dialogWidth:" + w + "px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
        }
        public static void WinOpen(string url, int h, int w)
        {
            string p = "dialogHeight:" + h + "px;dialogWidth:" + w + "px";
            HtmlPage.Window.Eval(string.Format("window.open('{0}','{1}','{2};scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');", url,
                      "Title", p));
        }
        public static string FK_Flow
        {
            get
            {
                if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("FK_Flow") == false)
                    throw new Exception("@ Lose FK_Flow Parameters .");
                return System.Windows.Browser.HtmlPage.Document.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  Whether read ?
        /// </summary>
        private static bool _IsRead = true;
        public static bool IsRead
        {
            get
            {
                if (_IsRead != null)
                    return _IsRead;

                if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("IsRead") == false)
                    _IsRead = true;

                if (System.Windows.Browser.HtmlPage.Document.QueryString["IsRead"] == "0")
                    _IsRead = false;
                else
                    _IsRead = true;
                return _IsRead;
            }
        }
        /// <summary>
        ///  Whether it is the start node 
        /// </summary>
        public static bool IsStartNode
        {
            get
            {
                string fk_node = Glo.FK_Node.ToString();
                if (fk_node.Substring(fk_node.Length - 2) == "01")
                    return true;
                else
                    return false;
            }
        }
        private static int _FK_Node = 0;
        public static int FK_Node
        {
            get
            {
                if (_FK_Node == 0)
                {
                    if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("FK_Node") == false)
                        throw new Exception("@ Lose FK_Node Parameters .");
                    _FK_Node = int.Parse(System.Windows.Browser.HtmlPage.Document.QueryString["FK_Node"]);
                }
                return _FK_Node;
            }
            set
            {
                _FK_Node = value;
            }
        }
        private static Int64 _WorkID = 0;
        public static Int64 WorkID
        {
            get
            {
                if (_WorkID == 0)
                {
                    if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("WorkID") == false)
                        throw new Exception("@ Lose WorkID Parameters .");
                    _WorkID = int.Parse(System.Windows.Browser.HtmlPage.Document.QueryString["WorkID"]);
                }
                return _WorkID;
            }
            set
            {
                _WorkID = value;
            }
        }
        private static Int64 _FID = 0;
        public static Int64 FID
        {
            get
            {
                if (_FID == 0)
                {
                    return 0;
                    if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("FID") == false)
                        throw new Exception("@ Lose FID Parameters .");

                    _FID = int.Parse(System.Windows.Browser.HtmlPage.Document.QueryString["FID"]);
                }
                return _FID;
            }
            set
            {
                _FID = value;
            }
        }
        public static object TempVal = null;
        public static MapData HisMapData = null;
    }
}

