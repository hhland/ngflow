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

namespace WorkNode
{
    public partial class BPUpload : UserControl
    {
        private AttachmentUploadType _UploadType = AttachmentUploadType.Single;
        public AttachmentUploadType UploadType
        {
            get { return _UploadType; }
            set { _UploadType = value; }
        }

        public bool IsCanDownload
        {
            get
            {
                if (btnDownload.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    btnDownload.Visibility = Visibility.Visible;
                }
                else
                {
                    btnDownload.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IsCanDelete
        {
            get
            {
                if (btnDelete.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    btnDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btnDelete.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IsShowMemo
        {
            get
            {
                if (ColumnMemo.Width.Value > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    ColumnMemo.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    ColumnMemo.Width = new GridLength(0, GridUnitType.Star);
                }
            }
        }

        private bool _IsShowTitle = true;
        public bool IsShowTitle
        {
            get { return _IsShowTitle; }
            set { _IsShowTitle = value; }
        }

        private string _extension = "JPG  Picture  (*.jpg)|*.jpg|PNG  Picture  (*.png)|*.png";
        public string extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        private string _SaveAs = @"D:\ccflow\trunk\CCFlow\DataUser\UploadFile";
        public string SaveAs
        {
            get { return _SaveAs; }
            set { _SaveAs = value; }
        }

        public bool IsCanUpload
        {
            get
            {
                if (btnUpload.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    btnUpload.Visibility = Visibility.Visible;
                }
                else
                {
                    btnUpload.Visibility = Visibility.Collapsed;
                }
            }
        }

        public BPUpload()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (UploadType == AttachmentUploadType.Single)
            {
                ChildWindow cw = new ChildWindow();
                PageSingle page = new PageSingle();
                cw.Content = page;
                cw.Closed += (sender2, e2) =>
                {
                    lblFile.Text = page.FileName;
                    lblMemo.Text = page.Memo;
                };
                cw.Show();
            }
            else
            {
                throw new Exception("dd");
                //ChildWindow cw = new ChildWindow();
                //Page page = new Page();
                //cw.Content = page;
                //cw.Show();
            }
        }
    }
}
