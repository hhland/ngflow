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
    public class BPEle : ContentControl
    {
        public string EleType = null;
        public string EleName = null;
        public string EleID = null;
        public string Tag1 = null;
        public string Tag2 = null;
        public string Tag3 = null;
        public string Tag4 = null;
        public double X = 0;
        public double Y = 0;
        public string KeyName = null;
        public BitmapImage HisPng = null;
        Image img;
        /// <summary>
        /// BPEle
        /// </summary>
        public BPEle()
        {
            ImageBrush ib = new ImageBrush();
            BitmapImage png = new BitmapImage(new Uri("/WorkNode;component/Img/FrmEleDef.png", UriKind.Relative));
            ib.ImageSource = png;
            this.HisPng = png;
            img = new Image() { Height = this.Height, Width = this.Width, Stretch = Stretch.Fill };
            img.Source = png;
            this.Content = img;
            this.MouseLeftButtonDown += BPEle_MouseLeftButtonDown;
            board.Closed += new EventHandler(board_Closed);
        }

        void board_Closed(object sender, EventArgs e)
        {
            if (board.DialogResult == true)
            {
                img.Source = board.image.Source;
            }
        }
        HandwritingBoard board = new HandwritingBoard();
        void BPEle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            board.Show();
        }
        public bool? DialogResult { get; set; }
    }
}
