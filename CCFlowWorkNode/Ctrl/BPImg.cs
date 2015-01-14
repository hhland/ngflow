using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BP.En;
using Microsoft.Expression.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
namespace WorkNode
{
    public class BPImg : System.Windows.Controls.TextBox
    {
        public string WinTarget = "_blank";
        public string WinURL = "";
     
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Text = "";
            base.OnMouseLeave(e);
        }

        public double X = 0;
        public double Y = 0;
        public string KeyName = null;
        /// <summary>
        /// BPImg
        /// </summary>
        public BPImg()
        {
            this.Name = "TB" + DateTime.Now.ToString("yyMMddhhmmss");
            this.IsReadOnly = true;

            this.Width = 200;
            this.Height = 120;

            ImageBrush ib = new ImageBrush();
            BitmapImage png = new BitmapImage(new Uri("/WorkNode;component/Img/Logo/"+Glo.CompanyID+"/LogoBig.png", UriKind.Relative));
            ib.ImageSource = png;
            this.Background = ib;

            this.HisPng = png;
            this.TextWrapping = System.Windows.TextWrapping.Wrap;
        }
        public BitmapImage HisPng = null;

        
    }
}
