using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Ink;
using System.Windows.Browser;
using System.Windows.Media.Imaging;

using System.IO;
using System;
using BPPaint.FF;

namespace BP
{
    public partial class SignControl : UserControl
    {
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //SignDomainContext sds = new SignDomainContext();
            //  Create a WriteableBitmap And the elements need to be assigned to render bitmaps WriteableBitmap
            WriteableBitmap wb = new WriteableBitmap(inkC, null);
            //  Create a Image Elements to carry bitmaps 
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Height = App.H;
            image.Width = App.W;
            image.Margin = new Thickness(5);
            image.Source = wb;
            thumbs.Children.Add(image);
            ScreenshotViewer.ScrollToHorizontalOffset(ScreenshotViewer.ExtentWidth);
           
            //this.LayoutRoot.Children.Clear();
            //this.LayoutRoot.Children.Add(new TextBlock() { Text =" I is the latest "});
            BPPaint.FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.SaveImageAsFileAsync(ImageHandle.ImageToByte(image.Source), App.pkval, App.mypk);
            da.SaveImageAsFileCompleted += new EventHandler<SaveImageAsFileCompletedEventArgs>(da_SaveImageAsFileCompleted);
        }
        void da_SaveImageAsFileCompleted(object sender, SaveImageAsFileCompletedEventArgs e)
        {
            if (e.Error == null)
            {
               // MessageBox.Show(e.Result);
                HtmlPage.Window.Invoke("GoBack", e.Result, App.mypk, App.pkval);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        Stroke _drawStroke;
        List<FillColor> lstFillColor;
        List<SizeData> lstSizeData;
        List<OpacityData> lstOpacityData;
        bool _isLoaded = false;
        public SignControl()
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
            //this.LayoutRoot.Height = App.H; this.LayoutRoot.Width = App.W;
            //this.LayoutRoot.Children.Clear();
            //this.LayoutRoot.Children.Add(new TextBlock() { Text = " I is the latest " });
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

            this.Loaded += new RoutedEventHandler(Page_Loaded);
            Width = App.W;
            Height = App.H;
            LayoutRoot.Width = App.W;
            LayoutRoot.Height = App.H;
            rg.Rect = new Rect() { X = 0, Y = 0, Height = App.H , Width = App.W  };
        }
        void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
            _isLoaded = true;
        }
        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ink.CaptureMouse();
            StylusPointCollection MyStylusPointCollection = new StylusPointCollection();
            MyStylusPointCollection.Add(e.StylusDevice.GetStylusPoints(ink));
            _drawStroke = new Stroke(MyStylusPointCollection);

            Color x = Colors.Black;

            _drawStroke.DrawingAttributes.Color = x;
            _drawStroke.DrawingAttributes.Width = 0.1;
            _drawStroke.DrawingAttributes.OutlineColor = x;
            _drawStroke.DrawingAttributes.Height = 0.1;
            ink.Strokes.Add(_drawStroke);
            ink.Opacity = (cboOpactiy.SelectedItem as OpacityData).Value;
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_drawStroke != null)
                _drawStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(ink));
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
                ink.Opacity = (cboOpactiy.SelectedItem as OpacityData).Value;
        }

        private void btnToogleBg_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            if (btn.Content.ToString() == " Hide background ")
            {
                //imgBg.Visibility = Visibility.Collapsed;
                btn.Content = " Display background ";
            }
            else
            {
                //imgBg.Visibility = Visibility.Visible;
                btn.Content = " Hide background ";
            }
        }
        private void btnToogleBgRect_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            if (btn.Content.ToString() == " Hide the background color ")
            {
                this.rectBg.Visibility = Visibility.Collapsed;
                btn.Content = " The background color ";
            }
            else
            {
                rectBg.Visibility = Visibility.Visible;
                btn.Content = " Hide the background color ";
            }
        }
        private void btnClearSave_Click(object sender, RoutedEventArgs e)
        {
            thumbs.Children.Clear();
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
}
