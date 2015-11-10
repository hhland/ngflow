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
using BP.Port;

namespace WorkNode.WorkRefFunc
{
    public partial class EndWorkFlow : ChildWindow
    {
        /// <summary>
        ///  End Process .
        /// </summary>
        public EndWorkFlow()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Length <= 3)
            {
                MessageBox.Show(" Reasons for the termination of the process must be described in detail .");
                return;
            }

            var da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.DoItAsync("EndWorkFlow", BP.Port.WebUser.No, BP.Port.WebUser.SID,
                Glo.FK_Flow, Glo.WorkID.ToString(), this.textBox1.Text, null, null, null);
            da.DoItCompleted += new EventHandler<FF.DoItCompletedEventArgs>(DoItCompleted);
        }
        void DoItCompleted(object sender, FF.DoItCompletedEventArgs e)
        {
            if (e.Result.Contains("err"))
            {
                MessageBox.Show(e.Result, "Error", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(e.Result, " End Process Success ", MessageBoxButton.OK);
                this.DialogResult = true;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

