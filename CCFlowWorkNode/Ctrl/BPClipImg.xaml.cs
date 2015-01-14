using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace WorkNode
{
    public partial class BPClipImg : ChildWindow
    {
        public WriteableBitmap ClipImage
        {
            get;
            set;
        }
        private static BPClipImg _Instance;
        public static BPClipImg Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BPClipImg();
                }
                return _Instance;
            }
        }
        private BPClipImg()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImgCropBorder.Visibility == Visibility.Visible)
            {
               // ImgCropBorder.Visibility = System.Windows.Visibility.Collapsed;
                ClipImage = new WriteableBitmap((int)ImgCropBorder.Width, (int)ImgCropBorder.Width);
                TranslateTransform t = new TranslateTransform();
                t.Y = -1 * Convert.ToDouble(ImgCropBorder.GetValue(Canvas.TopProperty));
                t.X = -1 * Convert.ToDouble(ImgCropBorder.GetValue(Canvas.LeftProperty));
                ClipImage.Render(SourceImg, t);
                ClipImage.Invalidate();
                this.DialogResult = true;
                //SourceImg.Source = ClipImage;
                //ResetMask();
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #region move & resize the Red Boreder
        private bool isBorder, isPath;
        private Point mousePosition;
        private void OnImgMouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            if (ImgCropBorder.Visibility == Visibility.Visible)
            {
                FrameworkElement element = sender as FrameworkElement;
                Point p = e.GetPosition(ImgCropBorder);
                if (p.X > 0 && p.X <= ImgCropBorder.Width && p.Y > 0 && p.Y <= ImgCropBorder.Height)
                {
                    isBorder = true;
                    mousePosition = e.GetPosition(null);
                    if (element != null)
                    {
                        element.CaptureMouse();
                        element.Cursor = Cursors.Hand;
                    }
                }
            }
        }
        private void OnImgMouseMove(object sender, MouseEventArgs e)
        {
            if (isBorder)
            {
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newTop = deltaV + (double)ImgCropBorder.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)ImgCropBorder.GetValue(Canvas.LeftProperty);
                if (newTop >= 0 && newTop <= SourceImg.Height - ImgCropBorder.Height)
                {
                    ImgCropBorder.SetValue(Canvas.TopProperty, newTop);
                }
                if (newLeft >= 0 && newLeft <= SourceImg.Width - ImgCropBorder.Width)
                {
                    ImgCropBorder.SetValue(Canvas.LeftProperty, newLeft);
                }
                mousePosition = e.GetPosition(null);
                //CropImg();
                SetMask();
            }

        }
        private void OnImgMosueLeftBtnUp(object sender, MouseButtonEventArgs e)
        {
            isBorder = false;
            SourceImg.ReleaseMouseCapture();
            mousePosition.X = mousePosition.Y = 0;
            SourceImg.Cursor = null;
        }
        private void OnBorderMouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            mousePosition = e.GetPosition(null);
            Point p = e.GetPosition(ImgCropBorder);
            if (p.X > 0 && p.X <= ImgCropBorder.Width && p.Y > 0 && p.Y <= ImgCropBorder.Height)
            {
                mousePosition = e.GetPosition(null);
                isPath = true;
                if (element != null)
                {
                    element.CaptureMouse();
                    element.Cursor = Cursors.SizeNWSE;
                }
            }
        }
        private void OnBorderMouseMove(object sender, MouseEventArgs e)
        {
            if (isPath)
            {
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newHeight = deltaV + ImgCropBorder.Height;
                double newWidth = deltaH + ImgCropBorder.Width;
                if (newHeight + Convert.ToDouble(ImgCropBorder.GetValue(Canvas.TopProperty)) > SourceImg.Height)
                {
                    newHeight = SourceImg.Height - Convert.ToDouble(ImgCropBorder.GetValue(Canvas.TopProperty));
                }
                if (newWidth + Convert.ToDouble(ImgCropBorder.GetValue(Canvas.LeftProperty)) > SourceImg.Width)
                {
                    newWidth = SourceImg.Width - Convert.ToDouble(ImgCropBorder.GetValue(Canvas.LeftProperty));
                }
                if (newHeight > 0 && newWidth > 0)
                {
                    ImgCropBorder.Height = newHeight;
                    ImgCropBorder.Width = newWidth;
                }
                mousePosition = e.GetPosition(null);
                //CropImg();
                SizeToMask();
            }
        }
        private void OnBorderMosueLeftBtnUp(object sender, MouseButtonEventArgs e)
        {
            isPath = false;
            FrameworkElement element = sender as FrameworkElement;
            element.ReleaseMouseCapture();
            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;
        }
        #endregion

        private void OnSelectPicClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false };
                if (true == ofd.ShowDialog())
                {
                    FileInfo fi = ofd.File;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(fi.OpenRead());
                    if (bitmap.PixelHeight >= bitmap.PixelWidth)
                    {
                        if (bitmap.PixelHeight >= ImgCanvas.Height)
                        {
                            SourceImg.Width = ImgCanvas.Height / bitmap.PixelWidth * bitmap.PixelWidth;
                            SourceImg.Height = ImgCanvas.Height;
                        }
                        else
                        {
                            SourceImg.Width = bitmap.PixelWidth;
                            SourceImg.Height = bitmap.PixelHeight;
                        }
                    }
                    else
                    {
                        if (bitmap.PixelWidth > ImgCanvas.Width)
                        {
                            SourceImg.Height = bitmap.PixelHeight * ImgCanvas.Width / bitmap.PixelWidth;
                            SourceImg.Width = ImgCanvas.Width;
                        }
                        else
                        {
                            SourceImg.Height = bitmap.PixelHeight;
                            SourceImg.Width = bitmap.PixelWidth;
                        }
                    }
                    SourceImg.Source = bitmap;
                    ResetMask();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void OnClipClick(object sender, RoutedEventArgs e)
        {
            if (SourceImg.Source != null)
            {
                ImgCropBorder.Width = ImgCropBorder.Height = 140;
                if (SourceImg.Height < 140)
                {
                    ImgCropBorder.Height = SourceImg.Height / 2;
                }
                if (SourceImg.Width < 140)
                {
                    ImgCropBorder.Width = SourceImg.Width / 2;
                }
                if (ImgCropBorder.Width > ImgCropBorder.Height)
                {
                    ImgCropBorder.Width = ImgCropBorder.Height;
                }
                else
                {
                    ImgCropBorder.Height = ImgCropBorder.Width;
                }
                Canvas.SetTop(ImgCropBorder, SourceImg.Height / 2 - ImgCropBorder.Width / 2);
                Canvas.SetLeft(ImgCropBorder, SourceImg.Width / 2 - ImgCropBorder.Width / 2);
                if (ImgCropBorder.Visibility == Visibility.Collapsed)
                {
                    ImgCropBorder.Visibility = System.Windows.Visibility.Visible;
                    SetMask();
                }
                else
                {
                    //ImgCropBorder.Visibility = System.Windows.Visibility.Collapsed;
                    //WriteableBitmap _WriteableBitmap = new WriteableBitmap((int)ImgCropBorder.Width, (int)ImgCropBorder.Width);
                    //TranslateTransform t = new TranslateTransform();
                    //t.Y = -1 * Convert.ToDouble(ImgCropBorder.GetValue(Canvas.TopProperty));
                    //t.X = -1 * Convert.ToDouble(ImgCropBorder.GetValue(Canvas.LeftProperty));
                    //_WriteableBitmap.Render(SourceImg, t);
                    //_WriteableBitmap.Invalidate();
                    //SourceImg.Source = _WriteableBitmap;
                    //ResetMask();
                }
            }
        }
        private void SetMask()
        {
            double left = Canvas.GetLeft(ImgCropBorder);
            double top = Canvas.GetTop(ImgCropBorder);
            topMask.Height = top;
            leftMask.Height = SourceImg.Height - top;
            leftMask.Width = left;
            Canvas.SetTop(leftMask, top);
            SizeToMask();
        }
        private void SizeToMask()
        {
            double left = Canvas.GetLeft(ImgCropBorder);
            double top = Canvas.GetTop(ImgCropBorder);
            topMask.Width = left + ImgCropBorder.Width;
            rightMask.Height = top + ImgCropBorder.Height;
            rightMask.Width = SourceImg.Width - left - ImgCropBorder.Width;
            Canvas.SetLeft(rightMask, left + ImgCropBorder.Width);
            bottomMask.Width = SourceImg.Width - left;
            bottomMask.Height = SourceImg.Height - top - ImgCropBorder.Height;
            Canvas.SetLeft(bottomMask, left);
            Canvas.SetTop(bottomMask, top + ImgCropBorder.Height);
        }
        private void ResetMask()
        {
            leftMask.Width = 0;
            rightMask.Width = 0;
            topMask.Width = 0;
            bottomMask.Width = 0;
        }
    }

    public class ImgByte
    {
        public static byte[] BitMapToByte(System.Windows.Media.Imaging.WriteableBitmap bitmap)
        {
            if (bitmap == null) return null;
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bands = 3;
            byte[][,] raster = new byte[bands][,];
            for (int i = 0; i < bands; i++)
            {
                raster[i] = new byte[width, height];
            }
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int pixel = bitmap.Pixels[width * row + column];
                    byte a = ((byte)(pixel >> 24));
                    byte r = (byte)(pixel >> 16);//4 R
                    byte g = (byte)(pixel >> 8);//2 G
                    byte b = (byte)pixel;//0 B
                    if (a < 2)
                    {
                        raster[0][column, row] = (byte)(255 - r);
                        raster[1][column, row] = (byte)(255 - g);
                        raster[2][column, row] = (byte)(255 - b);
                    }
                    else
                    {
                        raster[0][column, row] = (byte)(r * 255.0 / a);
                        raster[1][column, row] = (byte)(g * 255.0 / a);
                        raster[2][column, row] = (byte)(b * 255.0 / a);
                    }
                }
            }
            FluxJpeg.Core.ColorModel model = new FluxJpeg.Core.ColorModel { colorspace = FluxJpeg.Core.ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);
            //Encode the Image as a JPEG

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            FluxJpeg.Core.Encoder.JpegEncoder encoder = new FluxJpeg.Core.Encoder.JpegEncoder(img, 100, stream);
            encoder.Encode();
            //Back to the start

            stream.Seek(0, System.IO.SeekOrigin.Begin);
            //Get teh Bytes and write them to the stream
            byte[] binaryData = new byte[stream.Length];
            long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
            return binaryData;
        }

    }
}

