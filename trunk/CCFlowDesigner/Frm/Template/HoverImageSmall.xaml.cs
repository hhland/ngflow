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
using System.Windows.Media.Imaging;
using System.Windows.Messaging;
using System.Windows.Browser;
using System.IO;

namespace BP.Controls
{
    // This statement should be added , Otherwise JS Less than calling 
    [ScriptableType]
    public partial class HoverImageSmall : UserControl
    {
        // Wide mask blocks ,高
        double thumbWidth, thumbHeight;
        // The need for an enlarged display 
        bool IsHoverView = true;

        public HoverImageSmall()
        {
            InitializeComponent();
            canMove.MouseMove += new MouseEventHandler(ImgThumb_MouseMove);
            canMove.MouseEnter += new MouseEventHandler(ImgThumb_MouseEnter);
            canMove.MouseLeave += new MouseEventHandler(ImgThumb_MouseLeave);

            ImgOriginal.ImageOpened += new System.EventHandler<RoutedEventArgs>(ImgOriginal_ImageOpened);
            ImgThumb.ImageOpened += new System.EventHandler<RoutedEventArgs>(ImgThumb_ImageOpened);

        }
        // This structure can be from html Pass parameters to come 
        public HoverImageSmall(IDictionary<string, string> InitParams):this()
        {

            //从html Receiving parameters   Get this way 
            var parm1 = InitParams["parm1"].ToString();

            // Registration js Host name calling 
            HtmlPage.RegisterScriptableObject("HoverImageSmall", this);
        }

        #region image event
        // After loading a small map , Sizing mask blocks , Least 20*20
        void ImgThumb_ImageOpened(object sender, RoutedEventArgs e)
        {
            thumbWidth = ImgThumb.ActualWidth;
            thumbHeight = ImgThumb.ActualHeight;

            recMove.Width = 400 / (ImgOriginal.ActualWidth / ImgThumb.ActualWidth);
            if (recMove.Width > ImgThumb.ActualWidth)
                recMove.Width = ImgThumb.ActualWidth;

            recMove.Height = 400 / (ImgOriginal.ActualHeight / ImgThumb.ActualHeight);
            if (recMove.Height > ImgThumb.ActualHeight)
                recMove.Height = ImgThumb.ActualHeight;

            if (recMove.Width < 20)
                recMove.Width = 20;

            if (recMove.Height < 20)
                recMove.Height = 20;
        }

        // Determine the current figure is less than the size of the big picture , If less than , This function is not required 
        void ImgOriginal_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (ImgOriginal.ActualWidth < 400 && ImgOriginal.ActualHeight < 400)
            {
                recMove.Visibility = System.Windows.Visibility.Collapsed;
                IsHoverView = false;
            }
        }

        // Mouse leaves a small map 
        void ImgThumb_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            hoverPosition = "Visibility:0";
            //if (HoverChanged != null)
            //    HoverChanged(this, null);
        }

        // Mouse into the small map area 
        void ImgThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            hoverPosition = "Visibility:1";
            if (HoverChanged != null)
                HoverChanged(this, null);
        }


        void ImgThumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsHoverView)
            {
                hoverPosition = "NoHover:";
                if (HoverChanged != null)
                    HoverChanged(this, null);
                return;
            }
            Point p = e.GetPosition(ImgThumb);
            double moveLeft = p.X - recMove.Width / 2;
            double moveTop = p.Y - recMove.Height / 2;
            if (moveLeft < 0) moveLeft = 0;
            if (moveTop < 0) moveTop = 0;

            if (moveLeft > thumbWidth - recMove.Width)
                moveLeft = thumbWidth - recMove.Width;
            if (moveTop > thumbHeight - recMove.Height) moveTop = thumbHeight - recMove.Height;

            Canvas.SetLeft(recMove, moveLeft);
            Canvas.SetTop(recMove, moveTop);

            double newLeft = moveLeft * ImgOriginal.ActualWidth / thumbWidth;
            double newTop = moveTop * ImgOriginal.ActualHeight / thumbHeight;

            hoverPosition = "Position:" + newLeft + "," + newTop;
            if (HoverChanged != null)
                HoverChanged(this, null);
        }

    
        public void AddFile(MemoryStream fs)
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);

            imgBase64 = Convert.ToBase64String(bytes);

            string imgFormat = "<img id=\"testImg\" style=\"width:500px; height:400px;\" src=\"data:image/png;base64,{0}\"; />";
            imgHtml = string.Format(imgFormat, imgBase64);

            BitmapImage bitImg = new BitmapImage();
            bitImg.SetSource(fs);

            ImgThumb.Source = bitImg;
            ImgOriginal.Source = bitImg;

            #region xx
            if (bitImg.PixelWidth < 400 && bitImg.PixelHeight < 400)
            {
                recMove.Visibility = System.Windows.Visibility.Collapsed;
                IsHoverView = false;
            }
            else
            {
                IsHoverView = true;
                recMove.Visibility = System.Windows.Visibility.Visible;
            }

            if (bitImg.PixelWidth <= 160 && bitImg.PixelHeight <= 160)
            {
                thumbWidth = bitImg.PixelWidth;
                thumbHeight = bitImg.PixelHeight;
            }
            else if (bitImg.PixelWidth < 160)
            {
                thumbHeight = 160;
                thumbWidth = 160 * bitImg.PixelWidth / bitImg.PixelHeight;
            }
            else if (bitImg.PixelHeight < 160)
            {
                thumbWidth = 160;
                thumbHeight = 160 * bitImg.PixelHeight / bitImg.PixelWidth;
            }
            else
            {
                if (bitImg.PixelWidth > bitImg.PixelHeight)
                {
                    thumbWidth = 160;
                    thumbHeight = 160 * bitImg.PixelHeight / bitImg.PixelWidth;
                }
                else
                {
                    thumbHeight = 160;
                    thumbWidth = 160 * bitImg.PixelWidth / bitImg.PixelHeight;
                }
            }

            recMove.Width = 400 / (bitImg.PixelWidth / thumbWidth);
            if (recMove.Width > thumbWidth)
                recMove.Width = thumbWidth;

            recMove.Height = 400 / (bitImg.PixelHeight / thumbHeight);
            if (recMove.Height > thumbHeight)
                recMove.Height = thumbHeight;

            if (recMove.Width < 20)
                recMove.Width = 20;

            if (recMove.Height < 20)
                recMove.Height = 20;
            #endregion

            if (FileSelected != null)
                FileSelected(this, null);
        }
        #endregion


        #region ScriptableMemberAttribute
        [ScriptableMemberAttribute]
        public string imgBase64 { get; set; }

        [ScriptableMemberAttribute]
        public string imgHtml { get; set; }

        [ScriptableMemberAttribute]
        public string hoverPosition { get; set; }

        [ScriptableMemberAttribute]
        public string imgViewVisible { get; set; }
        #endregion

        #region ScriptableEvent
        [ScriptableMember()]
        public event EventHandler FileSelected;

        [ScriptableMember()]
        public event EventHandler MoveOnImage;

        [ScriptableMember()]
        public event EventHandler ViewVisibilityChanged;

        [ScriptableMember()]
        public event EventHandler HoverChanged;
        #endregion

        #region Scriptable members to control functions via javascript

        [ScriptableMember]
        public void EnableDrop()
        {
            //button1 image1.AllowDrop = true;
        }

        [ScriptableMember()]
        public event EventHandler MaximumFileSizeReached;

        #endregion
    }
}
