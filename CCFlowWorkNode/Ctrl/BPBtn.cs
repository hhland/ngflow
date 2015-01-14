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
using BP.En;

namespace WorkNode
{
    public class BPBtn : System.Windows.Controls.Button
    {
        #region  Check processing .
        public BtnType HisBtnType = BtnType.Self;
        public EventType HisEventType = EventType.Disable;
        private string _EventContext = "";
        public string EventContext
        {
            get
            {
                if (_EventContext == null)
                    _EventContext = "";
                return _EventContext;
            }
            set
            {
                _EventContext = value;
            }
        }
        private string _MsgErr = "";
        public string MsgErr
        {
            get
            {
                if (_MsgErr == null)
                    _MsgErr = "";
                return _MsgErr;
            }
            set
            {
                _MsgErr = value;
            }
        }

        private string _MsgOK = "";
        public string MsgOK
        {
            get
            {
                if (_MsgOK == null)
                    _MsgOK = "";
                return _MsgOK;
            }
            set
            {
                _MsgOK = value;
            }
        }

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
                    Thickness d1 = new Thickness(0);
                    this.BorderThickness = d1;
                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }
        public void SetUnSelectedState()
        {
            if (this.IsSelected)
            {
                this.IsSelected = false;
            }
            else
            {
                this.IsSelected = true;
            }
        }
        #endregion  Check processing .


        /// <summary>
        /// BPBtn
        /// </summary>
        public BPBtn()
        {
            this.Name = "Btn" + DateTime.Now.ToString("yyMMddhhmmss");
            this.Content = "Btn...";
            this.Foreground = new SolidColorBrush(Colors.Black);
            this.FontStyle = FontStyles.Normal;
            this.AllowDrop = false;
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
                case Key.W:
                    this.SetValue(Canvas.TopProperty, y - 1);
                    break;
                case Key.Down:
                case Key.S:
                    this.SetValue(Canvas.TopProperty, y + 1);
                    break;
                case Key.Left:
                case Key.A:
                    this.SetValue(Canvas.LeftProperty, x - 1);
                    break;
                case Key.Right:
                case Key.D:
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
                        BPBtn tb = new BPBtn();
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
