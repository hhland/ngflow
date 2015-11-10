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

namespace WorkNode.WorkRefFunc
{
    public partial class SendObjsAlert : ChildWindow
    {
        public SendObjsAlert()
        {
            InitializeComponent();
        }
        bool isErr = false;
        string toNodeID = null;
        /// <summary>
        ///  After sending the binding object returned 
        /// </summary>
        /// <param name="infos"> Information returned </param>
        public void BindIt(string myinfos)
        {
            
            #region  Rights management capabilities .
            this.Btn_AllotTask.Visibility = System.Windows.Visibility.Collapsed;
            if (Glo.IsStartNode)
                this.Btn_UnDo.Visibility = System.Windows.Visibility.Visible;
            else
                this.Btn_UnDo.Visibility = System.Windows.Visibility.Collapsed;
            #endregion  Rights management capabilities .


            if (myinfos.Contains("err@"))
            {
                /*  If it is wrong */
                isErr = true;
                myinfos = myinfos.Replace("@@", "@");
                myinfos = myinfos.Replace("@", "\t\n");
                this.textBlock1.Text = myinfos;

                this.Btn_UnDo.Visibility = System.Windows.Visibility.Collapsed;
                this.Btn_AllotTask.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            #region  Start parsing parameters .
            // Analytical parameters .
            string[] strs = myinfos.Split('$');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                string[] kv = str.Split('^');
                if (kv.Length == 1)
                    continue;

                string key = kv[0];
                string val = kv[1];
                switch (key)
                {
                    case BP.WF.SendReturnMsgFlag.VarToNodeID: // If it is a text message , Prompt to the user .
                        this.toNodeID = val;
                        break;
                    case BP.WF.SendReturnMsgFlag.MsgOfText: // If it is a text message , Prompt to the user .
                        val = val.Replace("@@", "@");
                        val = val.Replace("@", "\t\n");
                        this.textBlock1.Text = val;
                        break;
                    case BP.WF.SendReturnMsgFlag.VarAcceptersName:
                        if (val.Contains(","))
                        {
                            // If the recipient has multiple users ,  And allows automatic memory path is enabled .
                            if (Glo.GetNodeAttrByKeyInt(BP.WF.NodeAttr.IsRM) == 1)
                            {
                                /* Enable the distribution of staff functions .*/
                                this.Btn_AllotTask.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion  Start parsing parameters .

            //  Setting Information .
            this.textBlock1.Text = myinfos.Replace("@", "\t\n@");
            if (Glo.GetNodeAttrByKeyString(BP.WF.NodeAttr.HisToNDs).Length > 2)
            {
                /* If the non-end node , Undo function is enabled ..*/
                this.Btn_UnDo.Visibility = System.Windows.Visibility.Visible;
            }
        }
        /// <summary>
        ///  Determine 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ( isErr)
                this.DialogResult = false;
            else
                this.DialogResult = true;
        }
        /// <summary>
        ///  Send revocation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_UnDo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(" Are you sure you want to cancel it ?", " Confirm ", MessageBoxButton.OKCancel) 
                == MessageBoxResult.Cancel)
                return;

            var da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.DoItAsync("UnSend", BP.Port.WebUser.No, BP.Port.WebUser.SID,
                Glo.FK_Flow,Glo.WorkID.ToString(), null,null, null, null);
            da.DoItCompleted += new EventHandler<FF.DoItCompletedEventArgs>(BtnUnSend_DoItCompleted);
        }

        void BtnUnSend_DoItCompleted(object sender, FF.DoItCompletedEventArgs e)
        {
            if (e.Result.Contains("err@") == true)
            {
                /* An error message */
                MessageBox.Show(e.Result, " Error ", MessageBoxButton.OK);
                return;
            }

            MessageBox.Show(e.Result,
                " Execution information ", MessageBoxButton.OK);
            this.DialogResult = false; // If successful, do not cancel the lock screen .
            return;
        }

        private void Btn_AllotTask_Click(object sender, RoutedEventArgs e)
        {
            Glo.WinOpen("/WF/AllotTask.aspx?FK_Flow=" + Glo.FK_Flow + "&WorkID=" + Glo.WorkID + "&FK_Node=" + this.toNodeID,
                400, 300);
            this.DialogResult = true; // It locked .
        }
    }
}

