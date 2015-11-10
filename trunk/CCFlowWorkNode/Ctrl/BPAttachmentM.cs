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
using Silverlight;

namespace WorkNode
{
    public class BPAttachmentM : System.Windows.Controls.TextBox
    {
        public double X = 0;
        public double Y = 0;
        public string Label = null;
        public string SaveTo = null;
        public bool IsDelete = false;
        public bool IsDownload = false;
        public bool IsUpload = false;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        public BPAttachmentM()
        {
            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            this.Width = 500;
            this.Height = 200;
            this.BorderThickness = new Thickness(5);
        }
    }
}
