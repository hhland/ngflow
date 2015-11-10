using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using WF.WS;
using BP;

namespace BP.Frm
{
    public partial class FrmSortEdit : ChildWindow
    {
        public string No = "";
        /// <summary>
        ///  Display Type 
        /// </summary>
        public DisplayTypeEnum DisplayType { get; set; }
        public event EventHandler<DoCompletedEventArgs> ServiceDoCompletedEvent;
        public event EventHandler<SaveEnCompletedEventArgs> ServiceSaveEnCompletedEvent;
        public FrmSortEdit()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DisplayType == DisplayTypeEnum.AddSameLevel)// New sibling directory 
            {
                var doit = BP.Glo.GetDesignerServiceInstance();
                doit.DoAsync("NewSameLevelFrmSort", this.No + "," + this.TB_Name.Text, true);
                doit.DoCompleted += new EventHandler<DoCompletedEventArgs>(doit_DoCompleted);
            }
            else if (this.DisplayType == DisplayTypeEnum.AddSub)// New lower directory 
            {
                var doit = BP.Glo.GetDesignerServiceInstance();
                doit.DoAsync("NewSubLevelFrmSort", this.No + "," + this.TB_Name.Text, true);
                doit.DoCompleted += new EventHandler<DoCompletedEventArgs>(doit_DoCompleted);
            }
            else if (this.DisplayType == DisplayTypeEnum.Edit)// Edit Directory 
            {
                string strs = "";
                strs += "@EnName=BP.Sys.SysFormTree@PKVal=" + this.No;
                strs += "@Name=" + this.TB_Name.Text;
                var da = BP.Glo.GetDesignerServiceInstance();
                da.SaveEnAsync(strs);
                da.SaveEnCompleted += new EventHandler<SaveEnCompletedEventArgs>(da_SaveEnCompleted);
            }
            this.DialogResult = true;
        }

       
        void doit_DoCompleted(object sender, DoCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
                return;
            }

            if (null != ServiceDoCompletedEvent)
                ServiceDoCompletedEvent(this, e);
        }
        void da_SaveEnCompleted(object sender, SaveEnCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.Length > 4)
                {
                    MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
                    return;
                }
            }
            if (null != ServiceSaveEnCompletedEvent)
                ServiceSaveEnCompletedEvent(this, e);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        ///  Display Type enumeration 
        /// </summary>
        public enum DisplayTypeEnum
        {
            AddSameLevel,
            AddSub,
            Edit
        }
    }
}

