using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CCForm
{

    public class BPAttachment : UCExt, IRouteEvent
    {
        #region  Check processing 
        public override bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                base.IsSelected = value;
                if (value)
                {
                    this.layout.BorderThickness = new Thickness(0.5);
                }
                else
                {
                    this.BorderThickness = new Thickness(0);
                }
            }
        }
      
        #endregion


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
        #endregion  Property .

        public MouseButtonEventHandler LeftDown { get; set; }
        public MouseButtonEventHandler LeftUp { get; set; }

        /// <summary>
        /// BPAttachment
        /// </summary>
        public BPAttachment()
        {
            this.IsSelected = false;
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public BPAttachment(string NoOfObj, string name, string exts, double tbWidth, string _saveTo):this()
        {
            if (tbWidth < 20)
                tbWidth = 20;


            this.Exts = exts;
            this.SaveTo = _saveTo;
            this.Name = NoOfObj;

            this._HisTB = new BPTextBox();
            this.HisTB.HisTBType = TBType.Date;
            this.HisTB.Name = "No" + NoOfObj;
            this.HisTB.Width = tbWidth;
         
            Button btn = new Button();
            btn.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(BPAttachment_MouseLeftButtonDown), true);
            btn.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(BPAttachment_MouseLeftButtonUp), true);
            
            btn.Name = "btns" + NoOfObj;
            btn.Content = " Browse - Upload - Delete ";

            StackPanel sp = new StackPanel();
            sp.Name = "sp" + NoOfObj;
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(this.HisTB);
            sp.Children.Add(btn);
           
            this.Content = sp;

        }


        #region  Mobile Event 
        void BPAttachment_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        void BPAttachment_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonUp(e);
        }

        void BPAttachment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
          
        }
        Point mousePosition;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);

            mousePosition = e.GetPosition(null);

            if (null != LeftDown)
                LeftDown(this, e);

        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.TrackingMouseMove = false;
            if (null != LeftUp)
                LeftUp(this, e);
         
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (TrackingMouseMove)
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
       

        #endregion  Mobile Event 

    }
}
