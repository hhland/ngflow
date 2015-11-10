using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using Silverlight;
using BP.En;
using System.Windows.Markup;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace WorkNode
{
    public class BPDtl : System.Windows.Controls.HyperlinkButton
    {
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }
        DataTable dt = new DataTable();

        public BPDtl()
        {
            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            this.Width = 400;
            this.Height = 200;
            this.BorderThickness = new Thickness(5);
        }

        ObservableCollection<string[]> source = new ObservableCollection<string[]>();
        List<string> ColumnsName = new List<string>();
        string[] strs2;
        /// <summary>
        ///  First test of this dtl表
        /// </summary>
        /// <param name="dtlTableID"> To the first test of the table ID</param>
        /// <param name="ds"> Data Sources </param>
        public BPDtl(DataRow drDtl, DataSet ds)
        {
            this.Name = drDtl["No"].ToString();
            this.Foreground = new SolidColorBrush(Colors.Green);
            this.FontStyle = FontStyles.Normal;
            this.Width = 400;
            this.Height = 200;
            this.BorderThickness = new Thickness(5);

            MyDG = new BPDataGrid();
            MyDG.AddRow += new RowEventHandler(MyDG_AddRow);
            MyDG.DeleteRow += new RowEventHandler(MyDG_DeleteRow);
            MyDG.AutoGenerateColumns = false;
            MyDG.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            MyDG.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            MyDG.Name = "DG" + this.Name;
            int Index = 1;// Binding from 1 Column start （ The first column is automatically numbered ）
            ColumnsName = new List<string>();
            if (drDtl["IsShowIdx"].ToString() == "1")
            {
                DataGridTextColumn mycol = new DataGridTextColumn();
                mycol.Header = "IDX";
                Binding binding = new Binding("[0]");
                mycol.Binding = binding;
                MyDG.Columns.Add(mycol);
            }
            // Get the field attribute table .
            DataTable dtMapAttrs = ds.Tables["Sys_MapAttr"];
            // Each column cycle generation 
            foreach (DataRow dr in dtMapAttrs.Rows)
            {
                //  Because this table is also stored in the main table , Or any other list of   Field class control properties ,  So what you want to filter .
                if (dr["FK_MapData"].ToString() != this.Name)
                    continue;
                if (dr["UIVisible"].ToString() == "0")
                    continue;
                string myPk = dr["MyPK"];
                string FK_MapData = dr["FK_MapData"];
                string keyOfEn = dr["KeyOfEn"];
                string name = dr["Name"];          
                string defVal = dr["DefVal"];
                string UIContralType = dr["UIContralType"];
                string MyDataType = dr["MyDataType"];
                string lgType = dr["LGType"];
                bool isEnable = false;
                if (dr["UIIsEnable"].ToString() == "1")
                    isEnable = true;
                string UIBindKey = dr["UIBindKey"];
                ColumnsName.Add(keyOfEn);
                switch (UIContralType)
                {

                    case CtrlType.TextBox:

                        #region  Deal with textbox
                        switch (MyDataType)
                        {
                            case DataType.AppDateTime:
                                DataGridTemplateColumn dtTimePicker = GetDatePickerTemp(name, keyOfEn, Index);

                                MyDG.Columns.Add(dtTimePicker);
                                break;
                            case DataType.AppDate:
                                DataGridTemplateColumn dtPicker = GetDatePickerTemp(name, keyOfEn, Index);
                                MyDG.Columns.Add(dtPicker);
                                break;
                            default:
                                DataGridTemplateColumn dtTextBox = GetTextTemp(name, Index, MyDataType, keyOfEn, isEnable, dr);
                                MyDG.Columns.Add(dtTextBox);
                                break;
                        }

                        //BPTextBox tb = new BPTextBox( );
                        //tb.HisTBType = tp;
                        //tb.NameOfReal = keyOfEn;
                        //tb.Name = keyOfEn;
                        ////   tb.Text = this.GetValByKey(keyOfEn); // Assigned to the control .
                        //tb.Width = double.Parse(dr["UIWidth"]);

                        //if (tb.Height > 24)
                        //    tb.TextWrapping = TextWrapping.Wrap;

                        //tb.Height = double.Parse(dr["UIHeight"]);

                        //if (isEnable)
                        //    tb.IsEnabled = true;
                        //else
                        //    tb.IsEnabled = false;
                        ////  this.canvasMain.Children.Add(tb);
                        #endregion

                        break;
                    case CtrlType.DDL:
                        BPDDL ddl = new BPDDL();
                        //ddl.Name = keyOfEn;
                        //ddl.HisLGType = lgType;
                        //ddl.Width = double.Parse(dr["UIWidth"]);
                        //ddl.UIBindKey = UIBindKey;
                        //ddl.HisLGType = lgType;
                        //if (lgType == LGType.Enum)
                        //{
                        //    DataTable dtEnum =ds.Tables["Sys_Enum"];
                        //    foreach (DataRow drEnum in dtEnum.Rows)
                        //    {
                        //        if (drEnum["EnumKey"].ToString() != UIBindKey)
                        //            continue;

                        //        ListBoxItem li = new ListBoxItem();
                        //        li.Tag = drEnum["IntKey"].ToString();
                        //        li.Content = drEnum["Lab"].ToString();
                        //        ddl.Items.Add(li);
                        //    }
                        //    if (ddl.Items.Count == 0)
                        //        throw new Exception("@ No from Sys_Enum Found in numbered (" + UIBindKey + ") The enumeration value .");
                        //}
                        //else
                        //{
                        //    ddl.BindEns(UIBindKey);
                        //}

                        //ddl.SetValue(Canvas.LeftProperty, X);
                        //ddl.SetValue(Canvas.TopProperty, Y);

                        //// Assigned to the control .
                        //ddl.SetSelectVal(this.GetValByKey(keyOfEn));
                        //this.canvasMain.Children.Add(ddl);

                        double ddlWidth = double.Parse(dr["UIWidth"]);
                        DataRow[] drs = ds.Tables["Sys_Enum"].Rows.Where(p => p["EnumKey"] != UIBindKey).ToArray();
                        DataGridTemplateColumn dtDropDown = GetDropdownTemp(name, Index, keyOfEn, lgType, UIBindKey, ddlWidth, ds);
                        MyDG.Columns.Add(dtDropDown);
                        break;
                    case CtrlType.CheckBox:
                        //BPCheckBox cb = new BPCheckBox();
                        //cb.Name = keyOfEn;
                        //cb.Content = name;
                        //Label cbLab = new Label();
                        //cbLab.Name = "CBLab" + cb.Name;
                        //cbLab.Content = name;
                        //cbLab.Tag = keyOfEn;
                        //cb.Content = cbLab;
                        //cb.SetValue(Canvas.LeftProperty, X);
                        //cb.SetValue(Canvas.TopProperty, Y);
                        //if (this.GetValByKey(keyOfEn) == "1")
                        //    cb.IsChecked = true;
                        //else
                        //    cb.IsChecked = false;
                        //this.canvasMain.Children.Add(cb);
                        DataGridTemplateColumn dtCheckBox = GetCheckBoxTemp(name, Index);
                        MyDG.Columns.Add(dtCheckBox);
                        break;
                    case CtrlType.RB:
                        DataGridTemplateColumn dtRadioButton = GetCheckBoxTemp(name, Index);
                        MyDG.Columns.Add(dtRadioButton);
                        break;
                    default:
                        break;
                }
                Index++;
            }
            //List<TextClass> t = new List<TextClass>();
            //t.Add(new TextClass() { Text1 = "1111" });
            //t.Add(new TextClass() { Text1 = "1111" });
            //t.Add(new TextClass() { Text1 = "1111" });
            dt = ds[this.Name] as DataTable;
            //dt.
            //foreach (var item in d)
            //{
            //}
            //source.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(source_CollectionChanged);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    source[i] = new string[dt.Columns.Count + 1];
            //    source[i][0] = (i + 1).ToString();
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        source[i][j + 1] = dt.Rows[i][j];
            //    }
            //}
            //dg.ItemsSource = source;
            //this.Content = dg;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] strs = new string[Index];// More than the actual result 1列, Delete store , Save function 
                strs[0] = (i + 1).ToString();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int index = 0;
                    for (int k = 0; k < ColumnsName.Count; k++)
                    {
                        if (ColumnsName[k] == dt.Columns[j].ColumnName)
                        {
                            index = k;
                            strs[index + 1] = dt.Rows[i][j];
                            break;
                        }
                    }
                    //if (dt.Rows[i]["FK_MapData"].ToString() != this.Name)
                    //    continue;
                    //if (dt.Rows[i]["UIVisible"].ToString() == "0")
                    //    continue;
                }
                source.Add(strs);
            }
            // Plus an editable column 
            strs2 = new string[Index];
            source.Add(strs2);
            MyDG.ItemsSource = source;
            this.Content = MyDG;
        }

        //void MyDG_CurrentCellChanged(object sender, EventArgs e)
        //{
        //    if (this.MyDG.SelectedItem != null)
        //    {
        //        //BPDDL
        //        FrameworkElement fe = this.MyDG.CurrentColumn.GetCellContent(this.MyDG.SelectedItem);
        //        if (fe is BPTextBox)
        //        {
        //            var ctl = fe as BPTextBox;
        //            ctl.TextChanged -= new TextChangedEventHandler(ctl_TextChanged);
        //            ctl.TextChanged += new TextChangedEventHandler(ctl_TextChanged);
        //        }
        //        else if (fe is BPDDL)
        //        {
        //            var ctl = fe as BPDDL;
        //            ctl.SelectionChanged -= new SelectionChangedEventHandler(ctl_SelectionChanged);
        //            ctl.SelectionChanged += new SelectionChangedEventHandler(ctl_SelectionChanged);
        //        }

        //    }
        //}

        //void ctl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var ctl = sender as BPDDL;
        //    MessageBox.Show(ctl.SelectedIndex.ToString());
        //}

        //void ctl_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    TextBox tb = sender as TextBox;
        //    MessageBox.Show(tb.Text);
        //}

        public DataTable GetDataTable()
        {
            DataTable DT = new DataTable();
            DT.TableName = this.Name;
            for (int i = 0; i < ColumnsName.Count; i++)
            {
                DT.Columns.Add(new DataColumn(ColumnsName[i], typeof(string)));
            }
            for (int i = 0; i < source.Count; i++)
            {
                DataRow dr = DT.NewRow();
                for (int j = 0; j < ColumnsName.Count(); j++)
                {
                    if (string.IsNullOrEmpty(source[i][0]))
                    {
                        break;
                    }
                    else
                    {
                        dr[ColumnsName[j]] = source[i][j + 1];
                    }
                }
                DT.Rows.Add(dr);
            }
            return DT;
        }

        void MyDG_DeleteRow(object sender, object e)
        {
            Image img = e as Image;
            string id = Convert.ToString(img.Tag);
            var str = source.Where(p => p[0] == id).FirstOrDefault();
            source.Remove(str);
            for (int i = 0; i < source.Count - 1; i++)
            {
                source[i][0] = (i + 1).ToString();
            }
            MyDG.ItemsSource = null;
            MyDG.ItemsSource = source;
        }
        void MyDG_AddRow(object sender, object e)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i][0] = (i + 1).ToString();
            }
            strs2 = new string[dt.Columns.Count + 1];
            source.Add(strs2);
            MyDG.ItemsSource = null;
            MyDG.ItemsSource = source;
        }

        void source_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        ////  Implemented in the following code 
        //DataGridTextColumn txtColumn = new DataGridTextColumn();
        //txtColumn.Header = dr["Name"];
        //dg.Columns.Add(txtColumn);


        //dg.Width = this.Width;
        //dg.Height = this.Height;
        //this.Content = dg;
        //this.MyDG = dg;

        // Get list of data sources ,  The data presented to the user .
        //DataTable dtDtlData = ds.Tables[this.Name];



        public BPDataGrid MyDG = null;
        public void UpdatePos()
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                MyDG.Width = this.Width;
                MyDG.Height = this.Height;

            }
        }


        private DataGridTemplateColumn GetTextTemp(string name, int Index, string tp, string keyOfEn, bool IsEnable, DataRow dr)
        {

            //BPTextBox tb = new BPTextBox();
            //tb.HisTBType = tp;
            //tb.NameOfReal = keyOfEn;
            //tb.Name = keyOfEn;


            ////   tb.Text = this.GetValByKey(keyOfEn); // Assigned to the control .
            //tb.Width = double.Parse(dr["UIWidth"]);

            //if (tb.Height > 24)
            //    tb.TextWrapping = TextWrapping.Wrap;

            //tb.Height = double.Parse(dr["UIHeight"]);

            //if (isEnable)
            //    tb.IsEnabled = true;
            //else
            //    tb.IsEnabled = false;
            ////  this.canvasMain.Children.Add(tb);

            StringBuilder sb = new StringBuilder();
            sb.Append("<my:BPTextBox");
            sb.Append(" HisDataType='" + tp + "'");
            sb.Append(" NameOfReal='" + keyOfEn + "'");
            sb.Append(" Name='" + name + "'");
            sb.Append(" Width='" + dr["UIWidth"] + "'");
            sb.Append(" Height='" + dr["UIHeight"] + "'");
            sb.Append(" IsEnabled='" + IsEnable.ToString() + "'");
            sb.Append(" Text='{Binding [" + Index.ToString() + "],Mode=TwoWay}'");
            if (Height > 24)
            {
                sb.Append(" TextWrapping='Wrap'");
            }
            sb.Append(">");
            sb.Append("</my:BPTextBox>");

            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.Header = name;
            StringBuilder cellTemplateStr = new StringBuilder();
            cellTemplateStr.Append("<DataTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:my='clr-namespace:WorkNode;assembly=WorkNode'>");
            cellTemplateStr.Append(sb);
            cellTemplateStr.Append("</DataTemplate>");
            DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
            gridColumn.CellTemplate = cellTemplate;
            return gridColumn;
        }
        private DataGridTemplateColumn GetRadioButtonTemp(string HeaderText, int Index)
        {
            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.Header = HeaderText;
            StringBuilder cellTemplateStr = new StringBuilder();
            cellTemplateStr.Append("<DataTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>");
            cellTemplateStr.Append("<RadioButton IsSelected='{Binding [" + Index + "]}'></RadioButton>");
            cellTemplateStr.Append("</DataTemplate>");
            DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
            gridColumn.CellTemplate = cellTemplate;
            return gridColumn;
        }
        private DataGridTemplateColumn GetDropdownTemp(string HeaderText, int Index, string keyOfEn, string lgType, string UIBindKey, double Width, DataSet ds)
        {

            //BPDDL ddl = new BPDDL();
            //ddl.Name = keyOfEn;
            //ddl.HisLGType = lgType;
            //ddl.Width = Width;
            //ddl.UIBindKey = UIBindKey;
            //ddl.HisLGType = lgType;
            //if (lgType == LGType.Enum)
            //{
            //    DataTable dtEnum = ds.Tables["Sys_Enum"];
            //    foreach (DataRow drEnum in dtEnum.Rows)
            //    {
            //        if (drEnum["EnumKey"].ToString() != UIBindKey)
            //            continue;

            //        ListBoxItem li = new ListBoxItem();
            //        li.Tag = drEnum["IntKey"].ToString();
            //        li.Content = drEnum["Lab"].ToString();
            //        ddl.Items.Add(li);
            //    }
            //    if (ddl.Items.Count == 0)
            //        throw new Exception("@ No from Sys_Enum Found in numbered (" + UIBindKey + ") The enumeration value .");
            //}
            //else
            //{
            //    ddl.BindEns(UIBindKey);
            //}

            //ddl.SetValue(Canvas.LeftProperty, X);
            //ddl.SetValue(Canvas.TopProperty, Y);

            // Assigned to the control .
            //ddl.SetSelectVal(this.GetValByKey(keyOfEn));
            //this.canvasMain.Children.Add(ddl);

            StringBuilder sb = new StringBuilder();
            //{Binding [" + Index.ToString() + "]}'
            sb.Append("<my:BPDDL SelectedIndex='{Binding [" + Index.ToString() + "]}'");
            sb.Append(" x:Name='" + keyOfEn + "' ");
            sb.Append(" Width='" + Width.ToString() + "'");
            sb.Append(" UIBindKey='" + UIBindKey + "'");
            if (lgType != LGType.Enum)
            {
                //sb.Append(" UIBindKey='" + UIBindKey + "'");
                sb.Append(" HisLGType='" + lgType + "'");
            }

            sb.Append(">");
            if (lgType == LGType.Enum)
            {
                DataTable dtEnum = ds.Tables["Sys_Enum"];
                foreach (DataRow drEnum in dtEnum.Rows)
                {
                    if (drEnum["EnumKey"].ToString() != UIBindKey)
                        continue;
                    string strContent = "<ComboBoxItem Tag='{0}' Content='{1}'></ComboBoxItem>";
                    sb.Append(string.Format(strContent, drEnum["IntKey"].ToString(), drEnum["Lab"].ToString()));
                }
            }
            else
            {
                //sb.Append(" UIBindKey='" + UIBindKey + "'");
                //sb.Append(" HisLGType='" + lgType + "'");
            }
            sb.Append("</my:BPDDL>");

            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.Header = HeaderText;
            StringBuilder cellTemplateStr = new StringBuilder();
            cellTemplateStr.Append("<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:my='clr-namespace:WorkNode;assembly=WorkNode'>");
            cellTemplateStr.Append(sb);
            cellTemplateStr.Append("</DataTemplate>");
            DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
            gridColumn.CellTemplate = cellTemplate;
            return gridColumn;
        }
        private DataGridTemplateColumn GetCheckBoxTemp(string HeaderText, int Index)
        {
            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.Header = HeaderText;
            StringBuilder cellTemplateStr = new StringBuilder();
            cellTemplateStr.Append("<DataTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:my='clr-namespace:WorkNode;assembly=WorkNode'>");
            cellTemplateStr.Append("<my:BPCheckBox MyChecked='{Binding [" + Index + "],Mode=TwoWay}'></my:BPCheckBox>");
            cellTemplateStr.Append("</DataTemplate>");
            DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
            gridColumn.CellTemplate = cellTemplate;
            return gridColumn;
        }
        //private DataGridTemplateColumn GetImageTemp(string HeaderText, int Index, string imgName)
        //{
        //    DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
        //    gridColumn.Header = HeaderText;
        //    StringBuilder cellTemplateStr = new StringBuilder();
        //    cellTemplateStr.Append("<DataTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>");
        //    cellTemplateStr.Append("<Image Source='{Binding [" + Index.ToString() + "]}'></Image>");
        //    cellTemplateStr.Append("</DataTemplate>");
        //    DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
        //    DependencyObject de = cellTemplate.LoadContent();
        //    //(de as Image).MouseLeftButtonDown += new MouseButtonEventHandler(image_MouseLeftButtonDown);
        //    (de as Image).AddHandler(Image.MouseLeftButtonDownEvent, new MouseButtonEventHandler(image_MouseLeftButtonDown), false);
        //    gridColumn.CellTemplate = cellTemplate;
        //    return gridColumn;
        //}
        private DataGridTemplateColumn GetDatePickerTemp(string name, string KeyOfEn, int Index)
        {
            throw new Exception("textbox 的name 和 realname No settings need to have set up in order to use ");
            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.Header = name;
            StringBuilder cellTemplateStr = new StringBuilder();
            cellTemplateStr.Append("<DataTemplate  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:sdk='http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk'>");
            cellTemplateStr.Append("<sdk:DatePicker ");
            cellTemplateStr.Append(" Name='" + name + "'");
           cellTemplateStr.Append(" SelectedDate='{Binding [" + Index.ToString() + "]}'></sdk:DatePicker>");
          
            cellTemplateStr.Append("</DataTemplate>");
            DataTemplate cellTemplate = (DataTemplate)XamlReader.Load(cellTemplateStr.ToString());
            gridColumn.CellTemplate = cellTemplate;
            return gridColumn;
        }
        void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Delete");
        }
    }

    public class MyText : INotifyPropertyChanged
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
