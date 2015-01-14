using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CCForm
{
    public class BPWorkCheck : UCExt, IDelete
    {
        /// <summary>
        ///  Audit Component Status 0, Unavailable ,1 Available ,2 Read-only 
        /// </summary>
        public string FWC_Sta = "1";
        /// <summary>
        ///  Audit Component Type :0 Audit Components ,1 Multi-recording component 
        /// </summary>
        public string FWC_Type = "0";

      
        public string Name
        {
            get 
            {
                if (string.IsNullOrEmpty(base.Name))
                {
                    base.Name = MainPage.Instance.getElementNameFromUI(this);
                }

                return base.Name.Replace("Wc", "");
            }
            set
            {
                base.Name = "Wc" + value;
            }
        }
        #region 
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
                    selected();
                }
                else
                {
                    canChildrenMove = false;
                }
            }
        }

        public virtual bool IsCanReSize { get; set; }

        #endregion 

        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            BPWorkCheckMove();
            base.OnMouseMove(e);
        }
    
        Grid MyDG;

        /// <summary>
        ///  Called when a new control 
        /// </summary>
        public BPWorkCheck()
        {
            this.IsSelected = false;
            this.isCanReSize = true;

            new Adjust().Bind(this);
            this.BindDrag();

            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            //  Filling 
            Grid grid = CreateControls();
            MyDG = grid;
          
            grid.Name = "DG" + this.Name;
            this.layout.Child = grid;
            this.SizeChanged += (object sender, SizeChangedEventArgs e)=>
            {
                this.Width = this.Width;
                this.Height = this.Height;
                if (MyDG != null)
                {
                    if (!Convert.IsDBNull(this.Width))
                        MyDG.Width = this.Width;
                    if (!Convert.IsDBNull(this.Height))
                        MyDG.Height = this.Height;
                }
            }; 
            this.Width = 400;
            this.Height = 200;
        }

        /// <summary>
        ///  Use when loading an existing control ,Name Obtained from the database 
        /// </summary>
        /// <param name="name"></param>
        public BPWorkCheck(string name)
            : this()
        {
            this.Name = name;
        }

        private Grid CreateControls()
        {
            string xaml = @"<Grid Background='Transparent' ShowGridLines='True'
        xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  
        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
        xmlns:sdk='http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk'>
        <Grid.RowDefinitions>
            <RowDefinition Height='20'></RowDefinition>
            <RowDefinition Height='*'></RowDefinition>
        </Grid.RowDefinitions>

        <Border Background='LightGray' Grid.Row='0'>
            <TextBlock> Audit Components </TextBlock>
        </Border>
        <StackPanel Grid.Row='1' VerticalAlignment='Center' HorizontalAlignment='Center'>
            <TextBlock > Prompt :aswd Keys to change the position , Move the mouse to the border , Press and hold the left button to resize .</TextBlock>
            <TextBlock > You can only add nodes form </TextBlock>
            <TextBlock > And only one instance </TextBlock>
        </StackPanel>
       
    </Grid>";


            Grid grid = (Grid)System.Windows.Markup.XamlReader.Load(xaml);

            return grid;
        }
      
      
        public void UpdatePos()
        {
            OnMouseMove(null);
        }
      
        List<FrameworkElement> selectedElements = new List<FrameworkElement>();

        bool canChildrenMove = false;
        public void selected()
        {
            Canvas workSpace = this.Parent as Canvas;
            if (null == workSpace) return;

            BPWorkCheck bpWorkCheck = this;

            start = false;
            selectedElements.Clear();

            // Automatic variable adjustment marquee area  
            double finalX = double.MaxValue;
            double finalXMax = double.MinValue;
            double finalY = double.MaxValue;
            double finalYMax = double.MinValue;


            double xBegin =  Convert.ToDouble(bpWorkCheck.GetValue(Canvas.LeftProperty));
            double yBegin =  Convert.ToDouble(bpWorkCheck.GetValue(Canvas.TopProperty));
            double xEnd = xBegin + bpWorkCheck.ActualWidth;
            double yEnd = yBegin + bpWorkCheck.ActualHeight;

            double left = 0.0;
            double top = 0.0;
            double leftM = 0.0;
            double topM = 0.0;

            foreach (UIElement ue in workSpace.Children)
            {
                if (ue is FrameworkElement && !object.ReferenceEquals(ue, bpWorkCheck))
                {
                    FrameworkElement c = ue as FrameworkElement;

                    if (c is BPLine)
                    {//  There is no line judge 
                        BPLine line = c as BPLine;
                        left = line.MyLine.X1;
                        top = line.MyLine.Y1;
                        leftM = line.MyLine.X2;
                        topM = line.MyLine.Y2;
                    }
                    else
                    {
                        left = Convert.ToDouble(c.GetValue(Canvas.LeftProperty));
                        top = Convert.ToDouble(c.GetValue(Canvas.TopProperty));
                        leftM = left + c.ActualWidth;
                        topM = top + c.ActualHeight;
                    }

                    if (xBegin < leftM && xEnd > left && yBegin < topM && yEnd > top)
                    {
                        selectedElements.Add(c);
                        if (finalX > left) finalX = left;
                        if (finalY > top) finalY = top;

                        if (finalXMax < leftM) finalXMax = leftM;
                        if (finalYMax < topM) finalYMax = topM;
                    }

                }
            }

            canChildrenMove = true;

        }

     
        double left = 0.0;
        double top = 0.0;
        bool start = false;
        public void BPWorkCheckMove()
        {
            if (!canChildrenMove) return;

       
            BPWorkCheck rectSelected = this;
            if (!start)
            {
                left = Convert.ToDouble(rectSelected.GetValue(Canvas.LeftProperty));
                top = Convert.ToDouble(rectSelected.GetValue(Canvas.TopProperty));
                start = true;
            }
            int ind =(int)rectSelected.GetValue(Canvas.ZIndexProperty);
            foreach (var item in selectedElements)
            {
                if (ind >= (int)item.GetValue(Canvas.ZIndexProperty))
                    continue;

                if (item is BPLine)
                {
                    BPLine line = item as BPLine;
                    if (line != null)
                    {
                        line.MyLine.SetValue(Canvas.LeftProperty, Convert.ToDouble(item.GetValue(Canvas.LeftProperty)) + Convert.ToDouble(rectSelected.GetValue(Canvas.LeftProperty)) - left);
                        line.MyLine.SetValue(Canvas.TopProperty, Convert.ToDouble(item.GetValue(Canvas.TopProperty)) + Convert.ToDouble(rectSelected.GetValue(Canvas.TopProperty)) - top);
                    }
                }
                else
                {
                    item.SetValue(Canvas.LeftProperty, Convert.ToDouble(item.GetValue(Canvas.LeftProperty)) + Convert.ToDouble(rectSelected.GetValue(Canvas.LeftProperty)) - left);
                    item.SetValue(Canvas.TopProperty, Convert.ToDouble(item.GetValue(Canvas.TopProperty)) + Convert.ToDouble(rectSelected.GetValue(Canvas.TopProperty)) - top);
                }
            }
            left = Convert.ToDouble(rectSelected.GetValue(Canvas.LeftProperty));
            top = Convert.ToDouble(rectSelected.GetValue(Canvas.TopProperty));

        }
   
        /// <summary>
        ///  Remove it 
        /// </summary>
        public void DeleteIt()
        {
            if (MessageBox.Show(" Are you sure you want to delete the audit component [" + this.Name + "]吗?", " Delete Tip ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;

            /*  Update audit component status . */
            string nodeid = this.Name.Replace("Wc", "");
            string sql = "UPDATE WF_Node SET FWCSta=0 WHERE NodeID=" + nodeid;
                   
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLAsync(sql);
            da.RunSQLCompleted += (object sender, FF.RunSQLCompletedEventArgs e) =>
            {
                this.ViewDeleted = false;
                if (e.Error != null)
                {
                    MessageBox.Show(" Delete error " +e.Error.Message);
                    return;
                }

                Glo.Remove(this);
            };
        }
        /// <summary>
        ///  Hide it 
        /// </summary>
        public void HidIt()
        {
            FF.CCFormSoapClient hidDA = Glo.GetCCFormSoapClientServiceInstance();
            hidDA.RunSQLsAsync("UPDATE Sys_MapDtl SET IsView=0 WHERE No='" + Glo.FK_MapData + "'");
            hidDA.RunSQLsCompleted += (object sender, FF.RunSQLsCompletedEventArgs e)=>
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
            };
        }
    }
}
