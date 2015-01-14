using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using System.Text;
using Microsoft.Expression.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
using System.Windows.Media.Imaging;
using System.Windows.Interactivity;
using Silverlight;
using BP.En;
using BP.Sys;
using CCForm.FF;
using CCForm.Ctrl;

namespace CCForm
{
    public partial class FrmImpFromInternet : ChildWindow
    {
        #region  Property 
        public DateTime _lastTime = DateTime.Now;
        public string PathOfFtp = "/Form. Form templates /";
        public string PathOfFtpCurrDir = "";

        LoadingWindow loadingWindow = new LoadingWindow();
        #endregion

        public FrmImpFromInternet()
        {
            InitializeComponent();
        }
        protected override void OnOpened()
        {
            this.canvas1.Children.Clear();
            this.OKButton.Visibility = System.Windows.Visibility.Collapsed;

            /* Connect to the network */
            FF.CCFormSoapClient da =Glo.GetCCFormSoapClientServiceInstance();
            da.FtpMethodAsync("GetDirs", "/Form. Form templates /", null, null);
            da.FtpMethodCompleted += new EventHandler<FtpMethodCompletedEventArgs>(da_FtpMethodCompleted);

            this.loadingWindow.Title = " Retrieving directory listing please wait ....";
            this.loadingWindow.Show();
            base.OnOpened();
        }
        void da_FtpMethodCompleted(object sender, FtpMethodCompletedEventArgs e)
        {
            this.loadingWindow.Close();
            if ( e.Result==null ||  e.Result.Contains("Err") == true)
            {
                MessageBox.Show(e.Result, " Connect to a network error ", MessageBoxButton.OK);
                return;
            }

            string[] strs = e.Result.Split('@');
            int colIdx = 0;
            int rowIdx = 0;
            foreach (string s in strs)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                colIdx++;
                if (colIdx == 5)
                {
                    rowIdx++;
                    colIdx = 0;
                }
                Dir dir = new Dir();
                dir.BindText(s);
                dir.SetValue(Canvas.LeftProperty, (double)100*colIdx);
                dir.SetValue(Canvas.TopProperty, (double)100*rowIdx);
                dir.Tag = s;

                MouseDragElementBehavior mdeImg = new MouseDragElementBehavior();
                Interaction.GetBehaviors(dir).Add(mdeImg);
                this.canvas1.Children.Add(dir);
                dir.MouseLeftButtonDown += new MouseButtonEventHandler(dir_MouseLeftButtonDown);
            }
        }

        void dir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now.Subtract(_lastTime).TotalMilliseconds) < 300  )
            {
                Dir dir = sender as Dir;
                if (dir != null)
                {
                    FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                    da.FtpMethodAsync("GetFls", this.PathOfFtp + "/" + dir.Tag.ToString(), null, null);
                    this.PathOfFtpCurrDir = dir.Tag.ToString();

                    da.FtpMethodCompleted += new EventHandler<FtpMethodCompletedEventArgs>(da_Fls_FtpMethodCompleted);
                    this.loadingWindow.Title = " Getting template file list, please wait ....";
                    this.loadingWindow.Show();
                }
            }
            _lastTime = DateTime.Now;
        }
        void da_Fls_FtpMethodCompleted(object sender, FtpMethodCompletedEventArgs e)
        {
            this.loadingWindow.Close();
            if (e.Result == null || e.Result.Contains("Err") == true)
            {
                MessageBox.Show(e.Result, " Connect to a network error ", MessageBoxButton.OK);
                return;
            }

            this.OKButton.Visibility = System.Windows.Visibility.Visible;
            this.canvas1.Children.Clear();

            string[] strs = e.Result.Split('@');
            int colIdx = 0;
            int rowIdx = 0;
            foreach (string s in strs)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                colIdx++;
                if (colIdx == 5)
                {
                    rowIdx++;
                    colIdx = 0;
                }
                TempleteFile tempFile = new TempleteFile();
                tempFile.BindText(s);
                tempFile.SetValue(Canvas.LeftProperty, (double)100 * colIdx);
                tempFile.SetValue(Canvas.TopProperty, (double)100 * rowIdx);
                tempFile.Tag = s;
                MouseDragElementBehavior mdeImg = new MouseDragElementBehavior();
                Interaction.GetBehaviors(tempFile).Add(mdeImg);
                this.canvas1.Children.Add(tempFile);
                tempFile.MouseLeftButtonDown += new MouseButtonEventHandler(tempFile_MouseLeftButtonDown);
            }
        }

        void tempFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now.Subtract(_lastTime).TotalMilliseconds) < 300)
            {
                TempleteFile fl = sender as TempleteFile;
                if (fl != null)
                {
                    if (MessageBox.Show(" Are you sure you want to install [" + fl.Tag.ToString() + "] Templates do ?", " Confirm ", MessageBoxButton.OKCancel)
               != MessageBoxResult.OK)
                        return;

                    FF.CCFormSoapClient loadTemplete = Glo.GetCCFormSoapClientServiceInstance();
                    loadTemplete.FtpMethodAsync("LoadTempleteFile" ,fl.Tag.ToString(), Glo.FK_MapData, this.PathOfFtpCurrDir);
                    loadTemplete.FtpMethodCompleted += new EventHandler<FtpMethodCompletedEventArgs>(loadTemplete_FtpMethodCompleted);
                    this.loadingWindow.Title = " Please wait while the template is being loaded ...";
                    this.loadingWindow.Show();
                }
            }
            _lastTime = DateTime.Now;
        }
        public MainPage HisMainPage = null;
        void loadTemplete_FtpMethodCompleted(object sender, FtpMethodCompletedEventArgs e)
        {
            loadingWindow.DialogResult = false;
            if (e.Result != null)
            {
                loadingWindow.DialogResult = false;
                MessageBox.Show(e.Result, " Error ", MessageBoxButton.OK);
                return;
            }
            this.HisMainPage.BindFrm();
            this.DialogResult = true;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

