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

namespace WF
{
    public partial class AdminLogin : ChildWindow
    {
        
        public AdminLogin()
        {
            InitializeComponent();
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            this.OKButton.Focus();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            ChildWindow_KeyDown(this, e);
        }
        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
          
            switch (e.Key)
            {
                case Key.Escape:
                    this.DialogResult = false;
                    break;
                case Key.Enter:
                    Login();
                    break;
                default:
                    break;
            }
        }

        void Login()
        {
            string user = this.textBox1.Text.Trim();
            string pass = this.passwordBox1.Password.Trim();

            var da = BP.Glo.GetDesignerServiceInstance();

            da.EncryptAsync(pass);
            da.EncryptCompleted += new EventHandler<WS.EncryptCompletedEventArgs>(api_EncryptCompleted);
        }

        void api_EncryptCompleted(object sender, WS.EncryptCompletedEventArgs e)
        {
            string user = this.textBox1.Text.Trim();
            string pass = e.Result;

            var da = BP.Glo.GetDesignerServiceInstance();
            da.DoTypeAsync("AdminLogin", user, pass, null, null, null);
            da.DoTypeCompleted += new EventHandler<WS.DoTypeCompletedEventArgs>(da_DoTypeCompleted);
        }

        void da_DoTypeCompleted(object sender, WS.DoTypeCompletedEventArgs ee)
        {
            if (null != ee.Error)
            {
                BP.Glo.ShowException(ee.Error, " Log error ");
                return;
            }
            if (ee.Result != null)
            {
                MessageBox.Show(ee.Result, "Error", MessageBoxButton.OK);
                return;
            }
            this.DialogResult = true;
        }
    }
}

