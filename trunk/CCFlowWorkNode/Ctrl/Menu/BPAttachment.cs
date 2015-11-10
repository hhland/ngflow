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
 
    public class BPAttachment : System.Windows.Controls.UserControl
    {

        #region  Property .
        public double X = 100;
        public double Y = 100;
        /// <summary>
        ///  Can upload 
        /// </summary>
        public bool IsUpload = true;
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public bool IsDelete = true;
        /// <summary>
        ///  Can I download 
        /// </summary>
        public bool IsDownload = true;
        public string Label = "";
        #endregion  Property .

        #region  Check processing .
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                if (value == true)
                {
                    Thickness d = new Thickness(0.5);
                    this.BorderThickness = d;
                    this.BorderBrush = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    Thickness d1 = new Thickness(0.5);
                    this.BorderThickness = d1;
                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }
        public void SetUnSelectedState()
        {
            if (this.IsSelected)
                this.IsSelected = false;
            else
                this.IsSelected = true;
        }
        #endregion  Check processing .

        /// <summary>
        /// Exts
        /// </summary>
        public string Exts = "*.doc|*.docx";
        /// <summary>
        ///  Save to 
        /// </summary>
        public string SaveTo = @"D:\ccflow\trunk\CCFlow\DataUser\UploadFile";
        private BPTextBox _HisTB = null;
        public BPTextBox HisTB
        {
            get
            {
                if (_HisTB == null)
                {
                    _HisTB = new BPTextBox();
                    _HisTB.Name = "TB_" + this.Name + "_" + DateTime.Now.ToString("hhmmss");
                }
                return _HisTB;
            }
            set
            {
                _HisTB = value;
            }
        }
        /// <summary>
        /// BPAttachment
        /// </summary>
        public BPAttachment()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public BPAttachment(string NoOfObj, string name, string exts, double tbWidth, string _saveTo)
        {
            if (tbWidth < 20)
                tbWidth = 20;

            this.Name = NoOfObj;
            StackPanel sp = new StackPanel();
            sp.Name = "sp" + NoOfObj;
            sp.Orientation = Orientation.Horizontal;
            this.HisTB = new BPTextBox();
            this.HisTB.Name = "No" + NoOfObj;
            this.HisTB.Width = tbWidth;
            this.Exts = exts;
            this.SaveTo = _saveTo;
            sp.Children.Add(this.HisTB);
            Button btn = new Button();
            btn.Name = "btns" + NoOfObj;
            btn.Content = " Browse - Upload - Delete ";
            sp.Children.Add(btn);
            this.Content = sp;
        }

        #region  Mobile Event 
        bool trackingMouseMove = false;
        Point mousePosition;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(null);
            trackingMouseMove = true;
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            trackingMouseMove = false;
            base.OnMouseLeftButtonUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //FrameworkElement element = sender as FrameworkElement;
            if (trackingMouseMove)
            {
                double moveH = e.GetPosition(null).Y - mousePosition.Y;
                double moveW = e.GetPosition(null).X - mousePosition.X;
                double newTop = moveH + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = moveW + (double)this.GetValue(Canvas.LeftProperty);
                this.SetValue(Canvas.TopProperty, newTop);
                this.SetValue(Canvas.LeftProperty, newLeft);
                mousePosition = e.GetPosition(null);
            }
            base.OnMouseMove(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            //  Get  textBox  With respect to the object  Canvas 的 x Coordinate  和 y Coordinate 
            double x = (double)this.GetValue(Canvas.LeftProperty);
            double y = (double)this.GetValue(Canvas.TopProperty);
            // KeyEventArgs.Key -  Event-related keyboard keys  [System.Windows.Input.Key Enumerate ]
            switch (e.Key)
            {
                // 按 Up  Key after  textBox  Objects to  上  Mobile  1  Pixels 
                // Up  Key corresponding to  e.PlatformKeyCode == 38 
                //  When available  e.Key == Key.Unknown 时, You can use  e.PlatformKeyCode  To determine the user pressed the button 
                case Key.Up:
                    this.SetValue(Canvas.TopProperty, y - 1);
                    break;

                // 按 Down  Key after  textBox  Objects to  下  Mobile  1  Pixels 
                // Down  Key corresponding to  e.PlatformKeyCode == 40
                case Key.Down:
                    this.SetValue(Canvas.TopProperty, y + 1);
                    break;

                // 按 Left  Key after  textBox  Objects to  左  Mobile  1  Pixels 
                // Left  Key corresponding to  e.PlatformKeyCode == 37
                case Key.Left:
                    this.SetValue(Canvas.LeftProperty, x - 1);
                    break;

                // 按 Right  Key after  textBox  Objects to  右  Mobile  1  Pixels 
                // Right  Key corresponding to  e.PlatformKeyCode == 39 
                case Key.Right:
                    this.SetValue(Canvas.LeftProperty, x + 1);
                    break;
                case Key.Delete:
                    if (this.Name.Contains("_blank_") == false)
                    {
                        if (MessageBox.Show(" Are you sure you want to delete it ?",
                            " Delete Tip ", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                            return;
                    }
                    Canvas c = this.Parent as Canvas;
                    c.Children.Remove(this);
                    break;
                case Key.C:
                    break;
                case Key.V:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        BPAttachment tb = new BPAttachment();
                        tb.Cursor = Cursors.Hand;
                        tb.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + 15);
                        tb.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + 15);
                        Canvas s1c = this.Parent as Canvas;
                        try
                        {
                            s1c.Children.Add(tb);
                        }
                        catch
                        {
                            s1c.Children.Remove(tb);
                        }
                    }
                    break;
                default:
                    break;
            }
            base.OnKeyDown(e);
        }
        #endregion  Mobile Event 
    }
}
