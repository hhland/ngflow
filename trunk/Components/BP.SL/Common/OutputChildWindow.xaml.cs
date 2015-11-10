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

namespace BP.SL
{
    public partial class OutputChildWindow : ChildWindow
    {

        public static void ShowException()
        {
            try
            {
                BP.SL.OutputChildWindow opw = new BP.SL.OutputChildWindow();
                opw.Show();
            }
            catch (System.Exception ex)
            {
                BP.SL.LoggerHelper.Write(ex);
            }
        }
        private OutputChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        ///  Get the number of rows 
        /// </summary>
        private void Count()
        {
            Regex split = new Regex("\r", RegexOptions.Multiline);
            int lines = split.Matches(this.log.Text).Count;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                log.Text = LoggerHelper.Read();

                // Automatically scroll to the last line 
                log.Focus();
                log.Select(this.log.Text.Length, 0);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, " System Messages ", MessageBoxButton.OK);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoggerHelper.Clear();
                log.Text = string.Empty;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, " System Messages ", MessageBoxButton.OK);
            }
        }
    }
}

