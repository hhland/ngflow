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
using System.IO;
using WorkNode;
namespace CCForm
{
    public partial class FrmExp : ChildWindow
    {
        public FrmExp()
        {
            InitializeComponent();
        }
        public LoadingWindow loading = new LoadingWindow();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            loading.Show();
           WorkNode.FF.CCFlowAPISoapClient ff = Glo.GetCCFlowAPISoapClientServiceInstance();
            ff.DoTypeAsync("FrmTempleteExp", Glo.FK_MapData, null, null, null, null);
            ff.DoTypeCompleted += new EventHandler<WorkNode.FF.DoTypeCompletedEventArgs>(ff_DoTypeCompleted);
        }
        void ff_DoTypeCompleted(object sender, WorkNode.FF.DoTypeCompletedEventArgs e)
        {
            loading.DialogResult = false;
            if (e.Result != null)
            {
                MessageBox.Show(e.Result, " Execution failed ", MessageBoxButton.OK);
                return;
            }
            //Glo.WinOpen(Glo.BPMHost + "/WF/MapDef/Handler.ashx?DoType=DownTempFrm&FK_MapData=" + Glo.FK_MapData);
            Glo.WinOpen(Glo.BPMHost + "/WF/Admin/XAP/DoPort.aspx?DoType=DownFormTemplete&FK_MapData=" + Glo.FK_MapData,
                100, 100);
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

