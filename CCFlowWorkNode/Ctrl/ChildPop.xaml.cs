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
using Silverlight;

namespace WorkNode
{
    public partial class ChildPop : ChildWindow
    {
        #region

        private int _SelectionMode = 0;
        /// <summary>
        ///  Select mode  0: Multiple choice ,1: Radio 
        /// </summary>
        public int SelectionMode
        {
            get { return _SelectionMode; }
            set { _SelectionMode = value; }
        }

        private int _PopValFormat = 0;
        /// <summary>
        ///  The return value format ,0:No,1:Name,2:No,Name
        /// </summary>
        public int PopValFormat
        {
            get { return _PopValFormat; }
            set { _PopValFormat = value; }
        }

        private int _PopValWorkModel;
        public int PopValWorkModel
        {
            get { return _PopValWorkModel; }
            set { _PopValWorkModel = value; }
        }
        #endregion

        static FF.CCFlowAPISoapClient da1 = Glo.GetCCFlowAPISoapClientServiceInstance();
        static FF.CCFlowAPISoapClient da2 = Glo.GetCCFlowAPISoapClientServiceInstance();
        static ChildPop()
        {
            da1.GetNoNameCompleted += new EventHandler<FF.GetNoNameCompletedEventArgs>(da1_GetNoNameCompleted);
            da2.GetNoNameCompleted += new EventHandler<FF.GetNoNameCompletedEventArgs>(da2_GetNoNameCompleted);
        }
        static void da2_GetNoNameCompleted(object sender, FF.GetNoNameCompletedEventArgs e)
        {
            DS2 = new DataSet();
            DS2.FromXml(e.Result);
            CreateContent();
        }
        static void da1_GetNoNameCompleted(object sender, FF.GetNoNameCompletedEventArgs e)
        {
            DS1 = new DataSet();
            DS1.FromXml(e.Result);
            CreateContent();
        }
        public static string SQL1 { get; set; }
        public static string SQL2 { get; set; }
        private static DataSet DS1;
        private static DataSet DS2;

        private static ChildPop Instance;
        public static ChildPop CreateInstance(string Tag1, string Tag2)
        {
            Instance = new ChildPop(Tag1, Tag2);

            return Instance;

        }
        private ChildPop(string Tag1, string Tag2)
        {
            InitializeComponent();
            DS1 = null;
            DS2 = null;
            SQL1 = Tag1;
            SQL2 = Tag2;
            if (!string.IsNullOrEmpty(SQL1))
            {
                da1.GetNoNameAsync(SQL1);
            }
            if (!string.IsNullOrEmpty(SQL2))
            {
                da2.GetNoNameAsync(SQL2);
            }
            acc = new Accordion();
            acc.SelectionMode = AccordionSelectionMode.ZeroOrMore;
            acc.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.LayoutRoot.Content = acc;
            this.Closed += new EventHandler(ChildPop_Closed);
        }
        public static string ReturnValue = string.Empty;
        void ChildPop_Closed(object sender, EventArgs e)
        {
            ReturnValue = string.Empty;
            foreach (var item in acc.Items)
            {
                AccordionItem accItem = item as AccordionItem;
                Panel sp = accItem.Content as Panel;
                // Return No
                if (Instance.PopValFormat == 0)
                {
                    foreach (var ui in sp.Children)
                    {
                        CheckBox cb = ui as CheckBox;
                        if (cb != null && cb.IsChecked == true)
                        {
                            ReturnValue += (ui as CheckBox).Tag.ToString() + ",";
                        }
                    }
                }
                else if (Instance.PopValFormat == 1)
                {
                    foreach (var ui in sp.Children)
                    {
                        CheckBox cb = ui as CheckBox;
                        if (cb != null && cb.IsChecked == true)
                        {
                            ReturnValue += (ui as CheckBox).Content + ",";
                        }
                    }
                }
                else if (Instance.PopValFormat == 2)
                {
                    foreach (var ui in sp.Children)
                    {
                        CheckBox cb = ui as CheckBox;
                        if (cb != null && cb.IsChecked == true)
                        {
                            ReturnValue += cb.Content + "," + cb.Tag.ToString() + ",";
                        }
                    }
                }

            }
            ReturnValue = ReturnValue.TrimEnd(',');
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        static void CreateContent()
        {
            if (DS2 != null && string.IsNullOrEmpty(SQL1))
            {
                CreateContentWithoutGroup();
            }
            else if (DS1 != null && DS2 != null)
            {
                CreateContentWithGrop();
            }
        }

        static void CreateContentWithoutGroup()
        {
            acc.Items.Clear();
            AccordionItem item = new AccordionItem();
            item.IsSelected = true;

            if (Instance.SelectionMode == 1)
            {
                TextBlock tb = new TextBlock();
                tb.Tag = item;
                tb.Text = " Select ";
                item.Header = tb;
            }
            else
            {
                CheckBox cbHeader = new CheckBox();
                cbHeader.Tag = item;
                cbHeader.Content = " Select ";
                cbHeader.Click += new RoutedEventHandler(cbHeader_Click);
                item.Header = cbHeader;
            }

            WrapPanel sp = new WrapPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Margin = new Thickness(40, 0, 0, 0);
            sp.Width = acc.ActualWidth - 45;
            foreach (DataRow dr in DS2.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Content = dr["name"];
                cb.Tag = dr["no"];
                cb.Width = 100;
                if (ReturnValue.Contains(dr["name"]) || ReturnValue.Contains(dr["no"]))
                {
                    cb.IsChecked = true;
                }
                sp.Children.Add(cb);
                cb.AddHandler(CheckBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(cb_MouseLeftButtonDown), true);
            }
            if (sp.Children.Count > 0)
            {
                item.Content = sp;
                acc.Items.Add(item);
            }
        }

        static CheckBox LastCheckBox;
        static void cb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox cbContent = sender as CheckBox;

            if (Instance.SelectionMode == 1)
            {
                if (cbContent.IsChecked == false)
                {
                    if (LastCheckBox != null && LastCheckBox != cbContent)
                    {
                        LastCheckBox.IsChecked = false;
                    }
                    LastCheckBox = cbContent;
                }
            }
            else
            {
                int checkedNum = 0;
                Panel p = cbContent.Parent as Panel;
                AccordionItem aiHeader = p.Parent as AccordionItem;
                CheckBox cbHeader = aiHeader.Header as System.Windows.Controls.CheckBox;
                foreach (var item in p.Children)
                {
                    CheckBox cbOther = item as CheckBox;
                    if (cbOther != null && cbOther.IsChecked == true)
                    {
                        checkedNum++;
                    }
                }
                if (cbContent.IsChecked == false)
                {
                    checkedNum++;
                }
                else
                {
                    checkedNum--;
                }
                int childNum = p.Children.Count(a => a is CheckBox);
                if (checkedNum == 0)
                {
                    cbHeader.IsChecked = false;
                }
                else if (checkedNum == childNum)
                {
                    cbHeader.IsChecked = true;
                }
                else
                {
                    cbHeader.IsChecked = null;
                }
            }
        }
        static Accordion acc = new Accordion();

        static void CreateContentWithGrop()
        {
            acc.Items.Clear();
            foreach (DataRow dept in DS1.Tables[0].Rows)
            {
                AccordionItem item = new AccordionItem();

                if (Instance.SelectionMode == 1)
                {
                    TextBlock tb = new TextBlock();
                    tb.Tag = item;
                    tb.Text = dept[1];
                    item.Header = tb;
                }
                else
                {
                    CheckBox cbHeader = new CheckBox();
                    cbHeader.Tag = item;
                    cbHeader.Content = dept[1];
                    cbHeader.Click += new RoutedEventHandler(cbHeader_Click);
                    item.Header = cbHeader;
                }
                var drs = DS2.Tables[0].Rows.Where(p => p[2] == dept[0]);
                WrapPanel sp = new WrapPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.VerticalAlignment = VerticalAlignment.Stretch;
                sp.Margin = new Thickness(40, 0, 0, 0);
                sp.Width = acc.ActualWidth - 45;
                foreach (var dr in drs)
                {
                    CheckBox cb = new CheckBox();
                    cb.Content = dr["name"];
                    cb.Tag = dr["no"];
                    cb.Width = 100;
                    if (ReturnValue.Contains(dr["name"]) || ReturnValue.Contains(dr["no"]))
                    {
                        cb.IsChecked = true;
                    }
                    sp.Children.Add(cb);
                    cb.AddHandler(CheckBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(cb_MouseLeftButtonDown), true);
                }
                if (sp.Children.Count > 0)
                {
                    item.Content = sp;
                    acc.Items.Add(item);
                }
            }
            if (Instance.SelectionMode == 1)
            {
                return;// If the radio mode exit 
            }
            foreach (var item in acc.Items)
            {
                AccordionItem accItem = item as AccordionItem;
                CheckBox cbHeader = accItem.Header as System.Windows.Controls.CheckBox;
                Panel p = accItem.Content as Panel;
                int i = p.Children.Count(a => a is CheckBox && (a as CheckBox).IsChecked == true);
                if (i > 0 && i < p.Children.Count)
                {
                    cbHeader.IsChecked = null;
                }
                else if (i == p.Children.Count)
                {
                    cbHeader.IsChecked = true;
                }
            }
        }

        static void cbHeader_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbHeader2 = sender as CheckBox;
            AccordionItem accItem = cbHeader2.Tag as AccordionItem;
            WrapPanel spItem = accItem.Content as WrapPanel;
            if (spItem != null)
            {
                foreach (var subItem in spItem.Children)
                {
                    CheckBox cbItem = subItem as CheckBox;
                    if (cbItem != null)
                    {
                        cbItem.IsChecked = cbHeader2.IsChecked;
                    }
                }
            }
        }
    }
}

