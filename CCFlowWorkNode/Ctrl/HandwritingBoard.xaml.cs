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
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Ink;

namespace WorkNode
{
    public partial class HandwritingBoard : ChildWindow
    {
        Stroke _drawStroke;
        public double Scale = 1;
        List<FillColor> lstFillColor;
        List<SizeData> lstSizeData;
        List<OpacityData> lstOpacityData;

        bool _isLoaded = false;

        public HandwritingBoard()
        {
            InitializeComponent();
            // Initialization data 
            lstFillColor = new List<FillColor>() { 
                new FillColor(){ Color = new SolidColorBrush(Colors.Black), Name=" Black "},
                new FillColor(){ Color = new SolidColorBrush(Colors.Red), Name=" Red "},
                new FillColor(){ Color = new SolidColorBrush(Colors.Blue), Name=" Blue "},
                new FillColor(){ Color = new SolidColorBrush(Colors.Green),Name=" Green "},
                new FillColor(){ Color = new SolidColorBrush(Colors.Magenta), Name=" Magenta "},               
                new FillColor(){ Color = new SolidColorBrush(Colors.Orange), Name=" Orange "},
            };

            lstSizeData = new List<SizeData>()
            {
                new SizeData(){ Size=1.0},
                new SizeData(){ Size=3.0},
                new SizeData(){ Size=5.0},
                new SizeData(){ Size=7.0},
                new SizeData(){ Size=9.0},
                new SizeData(){ Size=11.0},
                new SizeData(){ Size=13.0},
                new SizeData(){ Size=15.0}
               
            };

            lstOpacityData = new List<OpacityData>(){
                new OpacityData(){ Value=0.1},
                new OpacityData(){ Value=0.2},
                new OpacityData(){ Value=0.3},
                new OpacityData(){ Value=0.4},
                new OpacityData(){ Value=0.5},
                new OpacityData(){ Value=0.6},
                new OpacityData(){ Value=0.7},
                new OpacityData(){ Value=0.8},
                new OpacityData(){ Value=0.9},
                new OpacityData(){ Value=1.0}
            };

            this.cboColor.ItemsSource = lstFillColor;
            this.cboColor.SelectedIndex = 0;

            this.cboOutlineColor.ItemsSource = lstFillColor;
            this.cboOutlineColor.SelectedIndex = 0;

            this.cboWidth.ItemsSource = lstSizeData;
            this.cboWidth.SelectedIndex = 0;

            this.cboHeight.ItemsSource = lstSizeData;
            this.cboHeight.SelectedIndex = 0;

            this.cboOpactiy.ItemsSource = lstOpacityData;
            this.cboOpactiy.SelectedIndex = 5;

            this.Loaded += new RoutedEventHandler(Page_Loaded);
            this.KeyDown += new KeyEventHandler(HandwritingBoard_KeyDown);
        }

        void HandwritingBoard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
            }
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }


        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ink.CaptureMouse();
            StylusPointCollection MyStylusPointCollection = new StylusPointCollection();
            MyStylusPointCollection.Add(e.StylusDevice.GetStylusPoints(ink));
            _drawStroke = new Stroke(MyStylusPointCollection);
            _drawStroke.DrawingAttributes.Color = (cboColor.SelectedItem as FillColor).Color.Color;
            _drawStroke.DrawingAttributes.OutlineColor = (this.cboOutlineColor.SelectedItem as FillColor).Color.Color;
            _drawStroke.DrawingAttributes.Width = (this.cboWidth.SelectedItem as SizeData).Size;
            _drawStroke.DrawingAttributes.Height = (this.cboHeight.SelectedItem as SizeData).Size;
            //_drawStroke.SetValue(OpacityProperty, (this.cboOpactiy.SelectedItem as OpacityData).Value);
            ink.Strokes.Add(_drawStroke);
            ink.Opacity = (cboOpactiy.SelectedItem as OpacityData).Value;
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_drawStroke != null)
            {
                _drawStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(ink));
            }
        }
        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            _drawStroke = null;
        }

        private void btnClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ink.Strokes.Clear();
        }

        private void cboOpactiy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_isLoaded)
            {
                ink.Opacity = (cboOpactiy.SelectedItem as OpacityData).Value;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //  HtmlPage.Window.Navigate(new System.Uri("http://yjmyzz.cnblogs.com/"), "_blank");
        }

        public Image image = new System.Windows.Controls.Image();
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //  Create a WriteableBitmap And the elements need to be assigned to render bitmaps WriteableBitmap
            WriteableBitmap wb = new WriteableBitmap(inkC, null);
            //  Create a Image Elements to carry bitmaps 
            image.Height = 120;
            image.Margin = new Thickness(5);  
            image.Source = wb;
            // 将Image Element into a container control 
            // thumbs.Children.Add(image);
            //ScreenshotViewer.ScrollToHorizontalOffset(ScreenshotViewer.ExtentWidth);
            this.DialogResult = true;
        }

        private void btnClearSave_Click(object sender, RoutedEventArgs e)
        {
            //thumbs.Children.Clear();
        }

        private void btnSaveLocal_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap(inkC, null);

            if (wb != null)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "JPEG Files (*.jpeg)|*.jpeg";
                saveDlg.DefaultExt = ".jpeg";

                if (saveDlg.ShowDialog().Value)
                {
                    using (Stream fs = saveDlg.OpenFile())
                    {
                        SaveToFile(wb, fs);
                        MessageBox.Show(string.Format(" File has been saved to [{0}]", saveDlg.SafeFileName));
                    }
                }
            }


        }


        private void SaveToFile(WriteableBitmap bitmap, Stream fs)
        {
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
                    raster[0][column, row] = (byte)(pixel >> 16);
                    raster[1][column, row] = (byte)(pixel >> 8);
                    raster[2][column, row] = (byte)pixel;
                }

            }

            FluxJpeg.Core.ColorModel model = new FluxJpeg.Core.ColorModel { colorspace = FluxJpeg.Core.ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);


            //Encode the Image as a JPEG
            MemoryStream stream = new MemoryStream();
            FluxJpeg.Core.Encoder.JpegEncoder encoder = new FluxJpeg.Core.Encoder.JpegEncoder(img, 100, stream);
            encoder.Encode();

            //Back to the start
            stream.Seek(0, SeekOrigin.Begin);

            //Get teh Bytes and write them to the stream
            byte[] binaryData = new byte[stream.Length];
            long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
            fs.Write(binaryData, 0, binaryData.Length);
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }




    public class FillColor
    {
        public SolidColorBrush Color { set; get; }
        public string Name { set; get; }
    }


    public class SizeData
    {
        public double Size { set; get; }
    }


    public class OpacityData
    {
        public double Value { set; get; }
    }




    //public partial class HandwritingBoard : ChildWindow
    //{
    //    public HandwritingBoard()
    //    {
    //        InitializeComponent();
    //    }
    //}
}

