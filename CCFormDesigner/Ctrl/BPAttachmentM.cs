using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CCForm
{
    public class BPAttachmentM : UCExt, IDelete
    {
        public string Label = null;
        public string SaveTo = null;
        public bool IsDelete = false;
        public bool IsDownload = false;
        public bool IsUpload = false;

        public BPAttachmentM()
        {
            new Adjust().Bind(this);
            this.BindDrag();
            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            this.Width = 500;
            this.Height = 200;
            this.IsSelected = false;
            this.isCanReSize = true;

            //  Filling 
            Grid grid = CreateControls();
            grid.Width = this.Width;
            grid.Height = this.Height;
            grid.Name = "DG" + this.Name;
            this.layout.Child = grid;

            this.SizeChanged += (object sender, SizeChangedEventArgs e) =>
            {
                this.Width = this.Width;
                this.Height = this.Height;
                if (grid != null)
                {
                    if (!Convert.IsDBNull(this.Width))
                        grid.Width = this.Width;
                    if (!Convert.IsDBNull(this.Height))
                        grid.Height = this.Height;
                }
            };
        }

        private Grid CreateControls()
        {
            string xaml =
@"<Grid Background='Transparent' ShowGridLines='True'
    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  
    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
    xmlns:sdk='http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk'>
    <Grid.RowDefinitions>
        <RowDefinition Height='20'></RowDefinition>
        <RowDefinition Height='*'></RowDefinition>
    </Grid.RowDefinitions>
 
    <Border Background='LightGray' Grid.Row='0'>
          <TextBlock > More Accessories </TextBlock>
    </Border>
    <TextBlock Grid.Row='1' TextWrapping='Wrap' VerticalAlignment='Center' HorizontalAlignment='Center'> Prompt :aswd Keys to change the position , Move the mouse to the border , Press and hold the left button to resize .</TextBlock>
</Grid>";


            Grid grid = (Grid)System.Windows.Markup.XamlReader.Load(xaml);

            return grid;
        }


        public void UpdatePos()
        {
            Glo.ViewNeedSave = true;
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
            }
        }

        public double X = 0;
        public double Y = 0;
        /// <summary>
        ///  Remove it 
        /// </summary>
        public void DeleteIt()
        {
            if (MessageBox.Show(" Are you sure you want to delete [" + this.Name + "]吗? If it is determined previously generated historical data will be deleted .",
                " Delete Tip ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;

            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLAsync("DELETE FROM Sys_FrmAttachment WHERE NoOfObj='" + this.Name + "' AND FK_MapData='" + Glo.FK_MapData + "'");
            da.RunSQLCompleted += (object sender, FF.RunSQLCompletedEventArgs e)=>
            {
                if (e.Result != 1)
                    return;
                Glo.Remove(this);
            };
        }
     
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            TrackingMouseMove = true;
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            TrackingMouseMove = false;
        }
        /// <summary>
        ///  Hide it 
        /// </summary>
        public void HidIt()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }      
    }
}
