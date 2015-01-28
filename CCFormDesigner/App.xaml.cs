using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Browser;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
namespace CCForm
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);

            // Set the current thread culture, To load the specified language characters .
            var culture = new CultureInfo("zh-cn");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Start the primary interface . 
            MainPage main = MainPage.Instance;
            main.Load();
            this.RootVisual = main;
        }
        private void Application_Exit(object sender, EventArgs e)
        {
        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //  If the application is running outside the debugger , Then use the browser 
            //  Exception reporting mechanism that exception .在 IE 上, A status bar will be used .
            //  Yellow alert icon to display the exception ,而 Firefox  Appears a script error .
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                //  Watch out :  This allows applications has been thrown in but not yet handle the exception of the case 
                //  Continue to run . 
                //  For production applications , This error handling should be replaced with the website report errors 
                //  And stop the application .
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;

            string alert = " Please handle this error as follows .";
            alert += "\t\n1, Press F5 Refresh this page .";
            alert += "\t\n2, Please iis Restart the ,used administrator login the server ,at cmd Carried out iisreset Then refresh the page .";
            alert += "\t\n3, If this is the first time , Please open the installation file has a common problem , This file is located D:\\ccflow\\doc\\.";
            alert += "\t\n4, Visit the official website (http://ccflow.org) , Get more help .";
            alert += "\t\n5, To this screen copy A picture ( Must be full screen ), Post to http://bbs.ccflow.org ";
            alert += "\t\n6, Please google About  ccflow  Frequently Asked Questions , Maybe you can find the answer .";
            alert += "\t\n";
            MessageBox.Show(alert+errorMsg);
            return;
        }
    }
}
