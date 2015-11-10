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
using BP.SL;
using BP.WF;
using BP.DA;
using BP.Sys;
using BP.En;
using Silverlight;
using BP.Port;

namespace WorkNode
{
    public partial class ReturnWork : ChildWindow
    {
        public ReturnWork()
        {            
            InitializeComponent();

            if (Glo.GetNodeAttrByKeyInt(NodeAttr.IsBackTracking) == 1)
            {
                this.checkBox1.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.checkBox1.Visibility = System.Windows.Visibility.Collapsed;
            }

            var da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.DB_GenerWillReturnNodesAsync(Glo.FK_Node, Glo.WorkID, Glo.FID,BP.Port.WebUser.No);
            da.DB_GenerWillReturnNodesCompleted += new EventHandler<FF.DB_GenerWillReturnNodesCompletedEventArgs>(da_DB_GenerWillReturnNodesCompleted);
        }

        void da_DB_GenerWillReturnNodesCompleted(object sender, FF.DB_GenerWillReturnNodesCompletedEventArgs e)
        {
            if (e.Result.Contains("err@"))
            {
                MessageBox.Show(e.Result);
                this.DialogResult = false;
                return;
            }

            DataSet ds = new DataSet();
            ds.FromXml(e.Result);

            this.comboBox1.Items.Clear();

            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string no = dr["No"].ToString();
                string name = dr["Name"].ToString() + " - " + dr["Rec"].ToString() + "-" + dr["RecName"].ToString();

                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = name;
                cbi.Tag = no;
                comboBox1.Items.Add(cbi);
            }

            if (comboBox1.Items.Count == 0)
            {
                MessageBox.Show(" Error :" + e.Result + ", Did not get to return a list of .");
                this.DialogResult = false;
                return;
            }

            if (comboBox1.SelectedIndex == -1)
                comboBox1.SelectedIndex = 0;

            // Glo.BindDDL(this.comboBox1, ds, 0, "No", "Name", null);

            string text = " Hello : ";
            text += " \t\n    Your work for errors , Please re-edit .";
            text += " \t\n 致!!!";
            text += " \t\n                " + BP.Port.WebUser.No;

            this.textBox1.Text = text;
        }
        /// <summary>
        ///  Determine .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Boolean isBack = false;
            if (this.checkBox1.Visibility == System.Windows.Visibility.Visible)
                isBack = (Boolean)this.checkBox1.IsChecked;

            var da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.Node_ReturnWorkAsync(Glo.FK_Flow, Glo.WorkID, Glo.FID, Glo.FK_Node,
                Glo.GetDDLValOfInt(this.comboBox1), this.textBox1.Text, isBack, BP.Port.WebUser.No, BP.Port.WebUser.SID);
            da.Node_ReturnWorkCompleted += new EventHandler<FF.Node_ReturnWorkCompletedEventArgs>(da_Node_ReturnWorkCompleted);
        }
        void da_Node_ReturnWorkCompleted(object sender, FF.Node_ReturnWorkCompletedEventArgs e)
        {
            MessageBox.Show(e.Result," Bounce message ", MessageBoxButton.OK);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

