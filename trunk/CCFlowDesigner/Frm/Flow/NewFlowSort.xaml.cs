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

namespace BP.Controls
{
    public partial class NewFlowSort : ChildWindow
    {
        /// <summary>
        ///  Display Type enumeration 
        /// </summary>
        public enum DisplayTypeEnum
        {
            AddSameLevel,
            AddSub,
            Edit
        }
        public string No { get; set; }
        public string FK_FlowSort { get; set; }
        /// <summary>
        ///  Process category names 
        /// </summary>
        public string FlowSortName { get; set; }
        /// <summary>
        ///  Display Type 
        /// </summary>
        public DisplayTypeEnum DisplayType { get; set; }
        public event EventHandler<DoCompletedEventArgs> ServiceDoCompletedEvent;
        public NewFlowSort(MainPage contaniner): this()
        {
            this._container = contaniner;
        }
        public NewFlowSort()
        {
            InitializeComponent();
        }
        MainPage _container;
        public MainPage Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }
        /// <summary>
        ///  Initialization control content  
        /// </summary>
        /// <param name="no"></param>
        /// <param name="name"></param>
        public void InitControl(string no, string name)
        {
            this.No = no;
            txtNodeName.Text = name;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayTypeEnum.AddSameLevel == DisplayType)// New sibling nodes 
            {
                Container._Service.DoAsync("NewSameLevelFlowSort", this.No + "," + txtNodeName.Text, true);
                Container._Service.DoCompleted += new EventHandler<DoCompletedEventArgs>(_service_DoCompleted);
            }
            if (DisplayTypeEnum.AddSub == DisplayType)// New lower node 
            {
                Container._Service.DoAsync("NewSubFlowSort", this.No + "," + txtNodeName.Text, true);
                Container._Service.DoCompleted += new EventHandler<DoCompletedEventArgs>(_service_DoCompleted);
            }
            if(DisplayTypeEnum.Edit == DisplayType)
            {
                Container._Service.DoAsync("EditFlowSort", this.No + "," + txtNodeName.Text, true);
                Container._Service.DoCompleted += new EventHandler<DoCompletedEventArgs>(_service_DoCompleted);
            }
            this.DialogResult = true;
        }
        void _service_DoCompleted(object sender, DoCompletedEventArgs e)
        {
            this.No=  e.Result;
            if(null != ServiceDoCompletedEvent)
                ServiceDoCompletedEvent(this, e);
            Container._Service.DoCompleted -= _service_DoCompleted;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

