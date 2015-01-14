using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Silverlight;
using WF.WS;
using System.Windows.Media;
namespace BP.Controls
{
    public partial class FrmNewFlow : ChildWindow
    {
        OpenFileDialog dialog = new OpenFileDialog();
        private byte[] buffer;
        FileInfo file;

        public event EventHandler<FlowTemplateLoadCompletedEventArgs> FlowTempleteLoadCompeletedEventHandler;
     
        /// <summary>
        ///  Default Process category 
        /// </summary>
        public String CurrentFlowSortName { get; set; }

        public FrmNewFlow()
        {
            InitializeComponent();

            WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
            da.RunSQLReturnTableAsync("SELECT no,name FROM WF_FlowSort ");
            da.RunSQLReturnTableCompleted += da_RunSQLReturnTableCompleted;

            string EnumText = "Name";
            DataTable dt=new DataTable();
            dt.Columns.Add(new DataColumn("No"));
            dt.Columns.Add(new DataColumn(EnumText));

            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = " Data track mode ";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = " Data consolidation mode ";
            dt.Rows.Add(dr);
            IList list = dt.GetBindableData(new Connector());

            DDL_DataStoreModel.ItemsSource = list;
            DDL_DataStoreModel.DisplayMemberPath = EnumText;
            DDL_DataStoreModel.SelectedIndex = 0;

            dt = new DataTable();
            dt.Columns.Add(new DataColumn("No"));
            dt.Columns.Add(new DataColumn(EnumText));

            dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = " According to the new process ";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = " The process according to the original number ";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "2";
            dr[1] = " Original process ID if there is coverage ";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "3";
            dr[1] = " Number import the specified process ";
            dt.Rows.Add(dr);
            list = dt.GetBindableData(new Connector());
            cbxFlowImpType.ItemsSource = list;
            cbxFlowImpType.DisplayMemberPath = EnumText;
            cbxFlowImpType.SelectionChanged += (object sender, SelectionChangedEventArgs e)=>
            {
                System.Windows.Visibility  visi= System.Windows.Visibility.Collapsed;
                if (cbxFlowImpType.SelectedIndex == 3)
                {
                    visi = System.Windows.Visibility.Visible;
                }
                this.flowImpSpecialNo.Visibility = visi;
            };
            cbxFlowImpType.SelectedIndex = 0;

            tabControl.SelectionChanged +=  tabControl_SelectionChanged;
        }

      
        void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tbItem = (sender as TabControl).SelectedItem as TabItem;
            if (null == tbItem) return;

            if (tbItem == this.tabImportNew)
            {

            }
            else if (tbItem == this.tabStandardNew)
            {

            }
            else if (tbItem == this.tabItemCCflow)
            {
                new FtpFileExplorer().Show();
                this.DialogResult = true;
            }
        }
      
        void da_RunSQLReturnTableCompleted(object sender, RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            DataTable dt = ds.Tables[0];

            //  Get the default process category 
            int defaultFlowSort = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1] == CurrentFlowSortName)
                {
                    defaultFlowSort = i;
                }
            }

            IList list = dt.GetBindableData(new Connector());
            if (list.Count > 0)
            {
                cbxFlowSortImport.ItemsSource = list;
                cbxFlowSortImport.DisplayMemberPath = dt.Columns[1].ColumnName;
                cbxFlowSortImport.SelectedIndex = defaultFlowSort;

                DDL_FlowSort.ItemsSource = list;
                DDL_FlowSort.DisplayMemberPath = dt.Columns[1].ColumnName;
                DDL_FlowSort.SelectedIndex = defaultFlowSort;
            }
        }

		void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = tabControl.SelectedItem as TabItem;
            if ( selectedItem == null )
                return;

            if (selectedItem == tabStandardNew)
            {
                if (string.IsNullOrWhiteSpace(TB_FlowName.Text))
                {
                    MessageBox.Show(" Please enter the process name ", " Prompt ", MessageBoxButton.OK);
                    return;
                }
               
                    var flowSortID = (DDL_FlowSort.SelectedItem as BindableObject).GetValue("NO");
                    var DataStoreModel = this.DDL_DataStoreModel.SelectedIndex;
                    var ptable = this.TB_PTable.Text;
                    var flowCode = this.TB_FlowCode.Text;

                    MainPage.Instance.NewFlow(flowSortID, TB_FlowName.Text, DataStoreModel, ptable, flowCode);
                    this.DialogResult = true;
            }

            if (selectedItem == tabImportNew)
            {
                if (buffer == null || buffer.Length <= 0 || file == null || cbxFlowSortImport.SelectedIndex == -1)
                {
                    MessageBox.Show(" Please select the template file ", " Prompt ", MessageBoxButton.OK);
                    return;
                }

                if (this.flowImpSpecialNo.Visibility == System.Windows.Visibility.Visible)
                {
                    if (!Glo.IsNum(this.flowImpSpecialNo.Text))
                    {
                        MessageBox.Show(" Process ID only allowed to enter numbers ", " Prompt ", MessageBoxButton.OK);
                        this.flowImpSpecialNo.Focus();
                        return;
                    }
                }

                // Upload calling service 
                try
                {
                    UpLoad();
                }
                catch (Exception ex)
                {
                    this.DialogResult = false;
                    MessageBox.Show(ex.Message, " Error ", MessageBoxButton.OK);
                }
            }
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void BtnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            dialog.Filter = "Xml Files (.xml)|*.xml|All Files (*.*)|*.*";
            if (dialog.ShowDialog().Value)
            {
                //  Select a file to upload 
                file = dialog.File;
                Stream stream = file.OpenRead();
                stream.Position = 0;
                buffer = new byte[stream.Length + 1];
                // The file is read into a byte array 
                stream.Read(buffer, 0, buffer.Length);

                tbxFileName.Text = dialog.File.Name;
            }
            else
            {
                MessageBox.Show(" Please select the file !", " Prompt ", MessageBoxButton.OK);
            }
        }

        void UpLoad()
        {
            Glo.Loading(true);
            WSDesignerSoapClient service = Glo.GetDesignerServiceInstance();
            service.FlowTemplateUploadAsync(buffer, file.Name);
            service.FlowTemplateUploadCompleted += (object sender, FlowTemplateUploadCompletedEventArgs e) =>
            {
                if (null != e.Error)
                {
                    Glo.Loading(false);
                    Glo.ShowException(e.Error);
                }
                else if (e.Result.Contains("Error:"))
                {
                    Glo.Loading(false);
                    MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
                }
                else
                {
                    string flowSort = (cbxFlowSortImport.SelectedItem as BindableObject).GetValue("NO");
                    service.FlowTemplateLoadAsync(flowSort, e.Result, cbxFlowImpType.SelectedIndex,-1 );
                    service.FlowTemplateLoadCompleted += (object senders, FlowTemplateLoadCompletedEventArgs ee) =>
                    {
                        Glo.Loading(false);
                        if (null != FlowTempleteLoadCompeletedEventHandler)
                            FlowTempleteLoadCompeletedEventHandler(sender, ee);
                    };
                }
            };

            this.DialogResult = true;
        }

      

     

        private void flowImpSpecialNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(flowImpSpecialNo.Text))
            {
                flowImpSpecialNo.Text = " Please enter the specified process ID ";
                flowImpSpecialNo.SelectAll();
            }
        }
       
    }
}

