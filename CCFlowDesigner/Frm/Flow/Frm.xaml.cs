using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Silverlight;
using WF.WS;


namespace BP.Frm
{
    public partial class Frm : ChildWindow
    {
        public MainPage HisMainPage = null;
        public AppType HisAppType = AppType.Application;
        public bool IsNew = false;
        public string SortNo { get; set; }
        public Frm()
        {
            InitializeComponent();
            this.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
            };
        }
        public void BindFrm(string fk_mapdata)
        {
            this.IsNew = false;
            string sqls = "SELECT * FROM Sys_FormTree order by Name";
            sqls += "@SELECT * FROM Sys_MapData WHERE No='" + fk_mapdata + "'";
            this.TB_No.IsEnabled = false;
            WSDesignerSoapClient daBindFrm = Glo.GetDesignerServiceInstance();
            daBindFrm.RunSQLReturnTableSAsync(sqls);
            daBindFrm.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(daBindFrm_RunSQLReturnTableSCompleted);
        }
        void daBindFrm_RunSQLReturnTableSCompleted(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            this.Btn_Del.IsEnabled = false;
            this.Btn_Save.IsEnabled = false;

            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dtSort = ds.Tables[0];
            if (dtSort.Rows.Count == 0)
            {
                DataRow dr = dtSort.NewRow();
                dr[0] = "01";
                dr[1] = " Default category ";
                dtSort.Rows.Add(dr);
            }
            if (this.IsNew)
            {
                Glo.Ctrl_DDL_BindDataTable(this.DDL_FrmSort, dtSort, SortNo);
                this.HisAppType = AppType.Application;
                this.Btn_Del.IsEnabled = true;
                this.Btn_Save.IsEnabled = true;
                return;
            }

            DataTable dtMapdata = ds.Tables[1];
            if (dtMapdata.Rows.Count == 0)
            {
                MessageBox.Show(" Data has been deleted , Please refresh the list .");
                this.DialogResult = false;
                return;
            }
            DDL_FrmType.Items.Clear();
            this.TB_No.Text = dtMapdata.Rows[0]["No"];
            this.TB_Name.Text = dtMapdata.Rows[0]["Name"];
            this.TB_PTable.Text = dtMapdata.Rows[0]["PTable"];

            //string tag = dtMapdata.Rows[0]["Url"];
            //if (tag != null)
            //    this.TB_URL.Text = tag;

            string Url = dtMapdata.Rows[0]["Url"];
            if (Url != null)
                this.TB_URL.Text = Url;

         
            int tag = 0;
            for (int i = 0; i < 6; i++)
            {
                ComboBoxItem li = new ComboBoxItem();
                li.Tag = tag;
                if (tag == 0)
                {
                    li.Content = " Fool form ";
                }
                else if (tag == 1)
                {
                    li.Content = " Freedom Form ";
                }
                else if (tag == 2)
                {
                    li.Content = "SL Form ";
                }
                else if (tag == 3)
                {
                    li.Content = " Custom Form ";
                }
                else if (tag == 4)
                {
                    li.Content = "Word Form ";
                }
                else if (tag == 5)
                {
                    li.Content = "Excel Form ";
                }
                if (i.ToString() == dtMapdata.Rows[0]["FrmType"])
                    li.IsSelected = true;
                this.DDL_FrmType.Items.Add(li);
                tag++;
            }
            //Glo.Ctrl_DDL_SetSelectVal(this.DDL_FrmType, dtMapdata.Rows[0]["FrmType"]);

            //this.TB_Designer.Text = dtMapdata.Rows[0]["Designer"];
            //this.TB_DesignerUnit.Text = dtMapdata.Rows[0]["DesignerUnit"];
            //this.TB_DesignerContact.Text = dtMapdata.Rows[0]["DesignerContact"];

            //  Application Type .
            this.HisAppType = (AppType)int.Parse(dtMapdata.Rows[0]["AppType"]);

            Glo.Ctrl_DDL_SetSelectVal(this.DDL_DBUrl, dtMapdata.Rows[0]["DBURL"]);
            //Glo.Ctrl_DDL_SetSelectVal(this.DDL_FrmType, dtMapdata.Rows[0]["FrmType"]);
            Glo.Ctrl_DDL_BindDataTable(this.DDL_FrmSort, dtSort, dtMapdata.Rows[0]["FK_FrmSort"]);

            if (this.HisAppType == AppType.Application)
            {
                this.Btn_Del.IsEnabled = true;
                this.Btn_Save.IsEnabled = true;
            }
        }
        public void BindNew()
        {
            this.Title = " New Form ";
            this.IsNew = true;
            this.TB_No.IsEnabled = true;
            string sqls = "SELECT No,Name FROM Sys_FormTree order by Name";
            WSDesignerSoapClient daNew = Glo.GetDesignerServiceInstance();
            daNew.RunSQLReturnTableSAsync(sqls);
            daNew.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(daNew_RunSQLReturnTableSCompleted);
            this.HisAppType = AppType.Application;
        }
        void daNew_RunSQLReturnTableSCompleted(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dtSort = ds.Tables[0];
            Glo.Ctrl_DDL_BindDataTable(this.DDL_FrmSort, dtSort, SortNo);
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to delete it ?", "ccflow Prompt ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;
            WSDesignerSoapClient delMap = Glo.GetDesignerServiceInstance();
            delMap.DoTypeAsync("DelFrm", this.TB_No.Text, null, null, null, null);
            delMap.DoTypeCompleted += new EventHandler<DoTypeCompletedEventArgs>(delMap_DoTypeCompleted);
        }
        void delMap_DoTypeCompleted(object sender, DoTypeCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                this.DialogResult = true;
                this.HisMainPage.BindFormTree();
                return;
            }
            MessageBox.Show(e.Result, " Error ", MessageBoxButton.OK);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Save.IsEnabled = false;
            if (this.IsNew)
            {
                WSDesignerSoapClient daCheckID = Glo.GetDesignerServiceInstance();
                daCheckID.RunSQLReturnValIntAsync("SELECT count(*) FROM Sys_MapData WHERE No='" + this.TB_No.Text.Trim() + "'");
                daCheckID.RunSQLReturnValIntCompleted += new EventHandler<RunSQLReturnValIntCompletedEventArgs>(daCheckID_RunSQLReturnValIntCompleted);
            }
            else
            {
                this.SaveEn();
            }
        }
        void daCheckID_RunSQLReturnValIntCompleted(object sender, RunSQLReturnValIntCompletedEventArgs e)
        {
            if (e.Result > 0)
            {
                this.Btn_Save.IsEnabled = true;
                MessageBox.Show(" Already numbered (" + this.TB_No.Text + ") Form ", " Failed to save ", MessageBoxButton.OK);
                return;
            }
            else
            {
                this.SaveEn();
            }
        }
        public void SaveEn()
        {
            string error = "";
            if (string.IsNullOrEmpty(this.TB_No.Text.Trim()))
                error += " Number can not be empty .";

            if (string.IsNullOrEmpty(this.TB_Name.Text.Trim()))
                error += " Name can not be empty .";

            string strs = "";
            strs += "@EnName=BP.Sys.MapData@PKVal=" + this.TB_No.Text;
            strs += "@No=" + this.TB_No.Text;
            strs += "@Name=" + this.TB_Name.Text;
            strs += "@PTable=" + this.TB_PTable.Text;
            strs += "@Url=" + this.TB_URL.Text;
            strs += "@Url=" + this.TB_URL.Text;

            ListBoxItem lb = this.DDL_FrmSort.SelectedItem as ListBoxItem;
            if (lb != null)
                strs += "@FK_FrmSort=" + lb.Tag.ToString();
            if (lb != null)
                strs += "@FK_FormTree=" + lb.Tag.ToString();

            lb = this.DDL_FrmType.SelectedItem as ListBoxItem;
            if (lb != null)
                strs += "@FrmType=" + lb.Tag.ToString();

            lb = this.DDL_DBUrl.SelectedItem as ListBoxItem;
            if (lb != null)
                strs += "@DBURL=" + lb.Tag.ToString();

            strs += "@AppType=" + (int)this.HisAppType;
            strs += "@Designer=" + this.TB_Designer.Text;
            strs += "@DesignerContact=" + this.TB_DesignerContact.Text;
            strs += "@DesignerUnit=" + this.TB_DesignerUnit.Text;

            WSDesignerSoapClient daSaveEn = Glo.GetDesignerServiceInstance();
            daSaveEn.SaveEnAsync(strs);
            daSaveEn.SaveEnCompleted += new EventHandler<SaveEnCompletedEventArgs>(daSaveEn_SaveEnCompleted);
        }
        void daSaveEn_SaveEnCompleted(object sender, SaveEnCompletedEventArgs e)
        {
            this.Btn_Save.IsEnabled = true;
            if (e.Result.Contains("Err"))
            {
                MessageBox.Show(e.Result, "error", MessageBoxButton.OK);
            }
            else
            {
                this.HisMainPage.BindFormTree();
                this.DialogResult = true;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.HisMainPage.BindFormTree();
            this.DialogResult = false;
        }
        private void DDL_FrmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetState();
        }
        public void SetState()
        {
            try
            {
                this.TB_PTable.IsEnabled = true;
                this.DDL_DBUrl.IsEnabled = true;
                this.TB_URL.IsEnabled = true;
                if (this.DDL_FrmType.SelectedIndex == 3)
                {
                    /* Custom Form */
                    this.TB_PTable.IsEnabled = false;
                    this.DDL_DBUrl.IsEnabled = false;
                    this.TB_URL.IsEnabled = true;
                }
                else
                {
                    this.TB_PTable.IsEnabled = true;
                    this.DDL_DBUrl.IsEnabled = true;
                    this.TB_URL.IsEnabled = false;
                }
            }
            catch
            {
            }
        }
        private void TB_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            string s = this.TB_Name.Text;
            var daPinYin = Glo.GetDesignerServiceInstance();
            daPinYin.ParseStringToPinyinAsync(s);
            daPinYin.ParseStringToPinyinCompleted += new EventHandler<ParseStringToPinyinCompletedEventArgs>(daPinYin_ParseStringToPinyinCompleted);
        }

        void daPinYin_ParseStringToPinyinCompleted(object sender, ParseStringToPinyinCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TB_No.Text.Trim()) == true)
            {
                this.TB_No.Text = e.Result;
            }

            if (string.IsNullOrEmpty(this.TB_PTable.Text.Trim()) == true)
            {
                this.TB_PTable.Text = e.Result;
            }
        }
    }
}

