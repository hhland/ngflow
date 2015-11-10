using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkNode.WorkRefFunc
{
    public partial class DelWorkFlow : ChildWindow
    {
        public DelWorkFlow()
        {
            InitializeComponent();
            int delModel = Glo.GetNodeAttrByKeyInt("DelEnable");

            //  Different delete mode it shows the different types of .
            switch (delModel)
            {
                case 0:
                    MessageBox.Show("@ This node does not allow the process to delete .");
                    break;
                case 1:
                    Glo.BindDDL(this.comboBox1, "@1= Tombstone ", "1");
                    break;
                case 2:
                    Glo.BindDDL(this.comboBox1, "@2= Logging way to delete ", "2");
                    break;
                case 3:
                    Glo.BindDDL(this.comboBox1, "@3= Completely remove ", "3");
                    break;
                case 4:
                    Glo.BindDDL(this.comboBox1, "@1= Tombstone @2= Logging way to delete @3= Completely remove ", "3");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Determine 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to delete it ?", " Confirm ", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            // Delete the implementation process .
            var da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.DoItAsync("DelWorkFlow", BP.Port.WebUser.No, BP.Port.WebUser.SID, Glo.FK_Flow, Glo.WorkID.ToString(), 
                this.textBox1.Text,
                Glo.GetDDLValOfString(this.comboBox1),null,null);
            da.DoItCompleted+=new EventHandler<FF.DoItCompletedEventArgs>(da_DoItCompleted);
        }

        void da_DoItCompleted(object sender, FF.DoItCompletedEventArgs e)
        {


            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

