using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkNode
{
    public class BPLabel : System.Windows.Controls.Label
    {
        /// <summary>
        /// BPLabel
        /// </summary>
        public BPLabel()
        {
            this.Name = "Lab" + DateTime.Now.ToString("yyMMddhhmmss");
            this.Content = "label...";
            this.Foreground = new SolidColorBrush(Colors.Black);
            this.FontStyle = FontStyles.Normal;
            this.AllowDrop = false;
        }
    }
}
