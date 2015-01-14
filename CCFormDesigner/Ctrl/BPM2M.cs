using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Silverlight;

namespace CCForm
{
    public class BPM2M : UCExt, IDelete
    {
        public Grid MyDG = null;
        public int IsM2M = 0;
        void init()
        {
            new Adjust().Bind(this);
            this.BindDrag();

            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            this.isCanReSize = true;

            this.IsSelected = false;
            this.SizeChanged += new SizeChangedEventHandler((object sender, SizeChangedEventArgs e)=>
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
            });
            this.Width = 500;
            this.Height = 200;
        }
        public BPM2M(int IsM2M)
        {
            this.IsM2M = IsM2M;
            init();
            AddGridUI();
        }
        void AddGridUI()
        {
            Grid dg = CreateControls();
            dg.Name = "DG" + this.Name;
            dg.Width = this.Width;
            dg.Height = this.Height;
            this.layout.Child = dg;
            this.MyDG = dg;
        }

        /// <summary>
        /// Dtl
        /// </summary>
        public BPM2M(string name)
        {
            this.Name = name;
            this.init();
            this.LoadDtl();
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
          <TextBlock >{0}</TextBlock>
    </Border>
    <TextBlock Grid.Row='1' TextWrapping='Wrap' VerticalAlignment='Center' HorizontalAlignment='Center'> Prompt :aswd Keys to change the position , Move the mouse to the border , Press and hold the left button to resize , Double-click or right-modifying properties .</TextBlock>
</Grid>";
           
            if (this.IsM2M == 0)
            {
                xaml = string.Format(xaml, " Many controls ");
            }
            else
            {
                xaml = string.Format(xaml, " Many more controls ");
            }

            Grid grid = (Grid)System.Windows.Markup.XamlReader.Load(xaml);

            return grid;
        }


        public void LoadDtl()
        {
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableAsync("SELECT * FROM Sys_MapM2M WHERE NoOfObj='" + this.Name + "' AND FK_MapData='"+Glo.FK_MapData+"'");
            da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnTableCompleted);
        }
        void da_RunSQLReturnTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            DataSet ds = new DataSet();
            ds.FromXml(e.Result);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            else
                int.TryParse(ds.Tables[0].Rows[0]["M2MType"], out IsM2M);

            AddGridUI();
        }
        public void NewM2M(double x, double y)
        {
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.DoTypeAsync("NewM2M", Glo.FK_MapData, this.Name, x.ToString(), y.ToString(), null);
            da.DoTypeCompleted += (object sender, FF.DoTypeCompletedEventArgs e)=>
            {
                if (e.Result != null)
                {
                    MessageBox.Show(e.Result, " New error ", MessageBoxButton.OK);
                    return;
                }
                Glo.OpenM2M(Glo.FK_MapData, this.Name + Glo.TimeKey);
            };
        }

        public void UpdatePos()
        {
        }

        /// <summary>
        ///  Remove it 
        /// </summary>
        public void DeleteIt()
        {
            if (MessageBox.Show(" Are you sure you want to delete [" + this.Name + "]吗? If it is determined that the relevant fields and settings will also be deleted , Historical data previously generated will be deleted .", " Delete Tip ", MessageBoxButton.OKCancel)
                == MessageBoxResult.Cancel)
                return;

            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.DoTypeAsync("DelM2M", this.Name, null, null, null, null, null);
            da.DoTypeCompleted +=(object sender, FF.DoTypeCompletedEventArgs e)=>
            {
                if (e.Result != null)
                {
                    MessageBox.Show(e.Result, " Delete error ", MessageBoxButton.OK);
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
            this.Visibility = System.Windows.Visibility.Collapsed;
            FF.CCFormSoapClient hidDA = Glo.GetCCFormSoapClientServiceInstance();
            hidDA.RunSQLsAsync("UPDATE Sys_MapDtl SET IsView=0 WHERE No='" + Glo.FK_MapData + "'");
            hidDA.RunSQLsCompleted +=(object sender, FF.RunSQLsCompletedEventArgs e)=>
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
            };
        }
    }
}
