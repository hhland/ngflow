using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BP;
using WF.WS;
using Silverlight;
using System.Collections;
using BP.Controls;

namespace BP.Frm
{
    public partial class FrmImp : ChildWindow
    {
        OpenFileDialog dialog = new OpenFileDialog();
        private byte[] buffer;
        FileInfo file;
        string currFK_FrmSort = "01";
        public FrmImp()
        {
            InitializeComponent();
            WSDesignerSoapClient da_InitFrmSort = Glo.GetDesignerServiceInstance();
            da_InitFrmSort.RunSQLReturnTableAsync("SELECT No,Name FROM Sys_FormTree");
            da_InitFrmSort.RunSQLReturnTableCompleted += new EventHandler<RunSQLReturnTableCompletedEventArgs>(da_InitFrmSort_RunSQLReturnTableCompleted);
        }

        void da_InitFrmSort_RunSQLReturnTableCompleted(object sender, RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            this.comboBox1.Items.Clear();
            Glo.Ctrl_DDL_BindDataTable(this.comboBox1, ds.Tables[0], currFK_FrmSort);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = tabControl.SelectedItem as TabItem;
            if (null == selectedItem)
                return;

            if (selectedItem == tabItem1)
            {
                if (buffer == null || buffer.Length <= 0 || file == null
                    || this.comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show(" Please select the template file , Or select a category to import .", " Prompt ", MessageBoxButton.OK);
                    return;
                }

                // Upload calling service 
                Glo.Loading(true);
                try
                {
                    WSDesignerSoapClient da = Glo.GetDesignerServiceInstance();
                    da.UploadfileCCFormAsync(buffer, file.Name, this.comboBox1.SelectedValue.ToString());
                    da.UploadfileCCFormCompleted += da_UploadfileCCFormCompleted;
                }
                catch (Exception ex)
                {
                    this.DialogResult = false;
                    MessageBox.Show(ex.Message, " Error ", MessageBoxButton.OK);
                }
            }

            if (selectedItem == tabItem2)
            {
                MessageBox.Show(" This feature is in construction , Stay tuned . Or visit http://templete.ccflow.org  Processes and form template network downloaded to the machine in the import .",
                    "Sorry", MessageBoxButton.OK);
            }
            this.DialogResult = false;
        }

        void da_UploadfileCCFormCompleted(object sender, UploadfileCCFormCompletedEventArgs e)
        {
            Glo.Loading(true);

            bool result = true;
            if (null != e.Error)
            {
                Glo.ShowException(e.Error);
                result = false;
                return;
            }
            else if (e.Result != null)
            {
                result = false;
                MessageBox.Show(e.Result, " Import Error ", MessageBoxButton.OK);
            }
            else
            {
                result = true;
                this.DialogResult = result;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
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
                this.textBox1.Text = dialog.File.Name;
            }
            else
            {
                MessageBox.Show(" Please select the file !", " Prompt ", MessageBoxButton.OK);
            }
        }

        
    }
}

