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
    public class BPImgAth : System.Windows.Controls.TextBox
    {
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.Text = " Hold down and drag the border location ,Shift+ Arrow keys to change the size .";
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Text = "";
            base.OnMouseLeave(e);
        }

        public double X = 0;
        public double Y = 0;
        public string KeyName = null;
        /// <summary>
        /// BPImgAth
        /// </summary>
        public BPImgAth()
        {
            this.Name = "TB" + DateTime.Now.ToString("yyMMddhhmmss");
            this.IsReadOnly = true;

            this.Width = 160;
            this.Height = 200;

            ImageBrush ib = new ImageBrush();
          //  BitmapImage png = new BitmapImage(new Uri("/WorkNode;component/Img/LogoH.png", UriKind.Relative));
            BitmapImage png = new BitmapImage(new Uri("/WorkNode;component/Img/Logo/" + Glo.CompanyID + "/LogoH.png", UriKind.Relative));

            ib.ImageSource = png;
            this.Background = ib;
            this.TextWrapping = System.Windows.TextWrapping.Wrap;

        }

         
    }
}
