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
using System.Text.RegularExpressions;

using Silverlight;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections;
using FtpFile = global::WF.WS.FtpFile;

namespace BP.Controls
{
    public partial class FtpFileDown : ChildWindow
    {
        string path, fileName, fileType, cmd;

        public FtpFileDown()
        {
            InitializeComponent();
            this.lbFlows.SelectionChanged += new SelectionChangedEventHandler(lbFlows_SelectionChanged);
        }
       
        public void Init(FtpFile superFile)
        {
            this.txtFlow.Text = superFile.Name;
            path = superFile.Path;

            this.lbFlows.ItemsSource = superFile.Subs;
            this.lbFlows.DisplayMemberPath = "Name";
            //this.lbFlows.SelectedItem = "Flow";
            this.lbFlows.SelectedIndex = superFile.Subs.Length-1;
        }

        /// <summary>
        ///  Picture Preview , Loaded by default Flow.png
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lbFlows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != this.lbFlows.SelectedValue)
            {
                fileName = (this.lbFlows.SelectedValue as FtpFile).Name + ".png";
                fileType = "PNG";
                cmd = "VIEW";
                DoFtp(path, fileName, fileType, cmd);
            }
        }

        bool sdREsult;
        SaveFileDialog sfd;
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Equals("btnDown"))
            {// xml  Download 
                if (null != this.lbFlows.SelectedValue)
                {
                    sfd = new SaveFileDialog()
                    {
                        DefaultExt = "xml",
                        Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*",
                        FilterIndex = 1
                    };

                    bool? result = sfd.ShowDialog();
                    sdREsult = result == null ?　false:(bool)result;

                    fileName = (this.lbFlows.SelectedValue as FtpFile).Name +".xml";
                    fileType = "XML";
                    cmd = "DOWN";
                    DoFtp(path, fileName, fileType, cmd);
                }
            }
            else if (btn.Name.Equals("btnInstall"))
            {//  Process template files online installation 

                MessageBoxResult result = MessageBox.Show(" Are you sure you want to download the process template and install it on the local server ?", " Download and install ", MessageBoxButton.OKCancel);
                if(result == MessageBoxResult.OK)
                {
                    fileName = "Flow.xml";
                    fileType = "XML";
                    cmd = "INSTALL";
                    DoFtp(path, fileName, fileType, cmd);
                }
            }
            else if (btn.Name.Equals("btnOK"))
            {
                this.DialogResult = true;
            }
            else if (btn.Name.Equals("btnCancel"))
            {
                this.DialogResult = false;
            }
        }
      
        void Loading(bool enabled)
        {
            Glo.Loading(enabled);

            enabled = !enabled;
            this.lbFlows.IsEnabled = enabled;
            this.btnDown.IsEnabled = enabled;
            this.btnInstall.IsEnabled = enabled;
          
        }

        public void DoFtp(string path, string fileName, string fileType, string cmd)
        {
            Loading(true);

            string[] FlowFileName = new string[] { path, fileName, fileType, cmd };
            var _service = Glo.GetDesignerServiceInstance();
            _service.FlowTemplateDownAsync(FlowFileName);
            _service.FlowTemplateDownCompleted += (object sender, global::WF.WS.FlowTemplateDownCompletedEventArgs e)=>
            {
                Loading(false);
                if (null != e.Error)
                {
                    Glo.ShowException(e.Error);
                    return;
                }

                if (fileType == "PNG")
                {
                    if (null == e.Result) return;
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ms.Write(e.Result, 0, e.Result.Length);
                    if (ms.Capacity > 0)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.SetSource(ms);
                        this.imgView.Width = bitmap.PixelWidth;
                        this.imgView.Height = bitmap.PixelHeight;
                        this.imgView.Source = bitmap;
                    }
                }
                else if (fileType == "XML")
                {
                    if (cmd == "DOWN")
                    {////  Saved locally 
                        if (sdREsult && null != e.Result)
                        {
                            using (Stream stream = sfd.OpenFile())
                            {
                                stream.Write(e.Result, 0, e.Result.Length);
                                stream.Close();
                            }
                        }
                    }
                    else if (cmd == "INSTALL")
                    {    //// Line installation 
                        if (fileName.Equals("Flow.xml"))
                        {
                            Loading(true);
                            string filePath = System.Text.Encoding.UTF8.GetString(e.Result, 0, e.Result.Length);
                            _service.FlowTemplateLoadAsync(Glo.FK_FlowSort, filePath, 0 , 0);// Installation Process 
                            _service.FlowTemplateLoadCompleted += (object senders, global::WF.WS.FlowTemplateLoadCompletedEventArgs ee)=>
                            {
                                Loading(false);
                                if (null == ee.Error && !string.IsNullOrEmpty(ee.Result))
                                {
                                    this.Close();
                                  
                                    if (null != FlowTempleteLoadCompeleted)
                                        FlowTempleteLoadCompeleted(sender, ee);
                                }
                            };
                        }
                    }
                }
            };
        }


        //  Upon completion of the installation of automatic shut-line and navigate to the Process Designer , And open the new process 
        public event EventHandler<global::WF.WS.FlowTemplateLoadCompletedEventArgs> FlowTempleteLoadCompeleted;
       
    }
}

