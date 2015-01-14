using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Silverlight;

namespace WorkNode
{
    public partial class UCPop : UserControl
    {
        public UCPop()
        {
            InitializeComponent();
        }
        static FF.CCFlowAPISoapClient da1 = Glo.GetCCFlowAPISoapClientServiceInstance();
        static FF.CCFlowAPISoapClient da2 = Glo.GetCCFlowAPISoapClientServiceInstance();
        static UCPop()
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
        public UCPop(string Tag1, string Tag2)
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
            CheckBox cbHeader = new CheckBox();
            cbHeader.Tag = item;
            cbHeader.Content = " Select ";
            cbHeader.Click += new RoutedEventHandler(cbHeader_Click);
            item.Header = cbHeader;
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
                sp.Children.Add(cb);
            }
            if (sp.Children.Count > 0)
            {
                item.Content = sp;
                acc.Items.Add(item);
            }
        }
        static Accordion acc = new Accordion();
        static void CreateContentWithGrop()
        {
            acc.Items.Clear();
            foreach (DataRow dept in DS1.Tables[0].Rows)
            {
                AccordionItem item = new AccordionItem();
                CheckBox cbHeader = new CheckBox();
                cbHeader.Tag = item;
                cbHeader.Content = dept[1];
                cbHeader.Click += new RoutedEventHandler(cbHeader_Click);
                item.Header = cbHeader;
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
                    sp.Children.Add(cb);
                }
                if (sp.Children.Count > 0)
                {
                    item.Content = sp;
                    acc.Items.Add(item);
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
