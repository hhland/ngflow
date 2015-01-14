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
using WorkNode.Classes;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode
{
    public partial class FileRowControl : UserControl
    {

        private UserFile UserFile
        {
            get
            {
                if (this.DataContext != null)
                    return ((UserFile)this.DataContext);
                else
                    return null;

            }
        }

        public FileRowControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(FileRowControl_Loaded);
        }


        void FileRowControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Custom made UserFile的PropertyChanged  Property ,如BytesUploaded,Percentage,IsDeleted
            UserFile.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged);
     
        }

        void FileRowControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                // Gray character displayed after the current file uploaded 
                if (this.UserFile.State == Constants.FileStates.Finished)
                {
                    GreyOutText();
                    ShowValidIcon();
                }

                // Upload failed as an error message 
                if (this.UserFile.State == Constants.FileStates.Error)
                {
                    ErrorMsgTextBlock.Visibility = Visibility.Visible;
                }
            }

        }

        private void ShowValidIcon()
        {
            PercentageProgress.Visibility = Visibility.Collapsed;
            ValidUploadIcon.Visibility = Visibility.Visible;
        }

        private void GreyOutText()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);

            FileNameTextBlock.Foreground = grayBrush;
            StateTextBlock.Foreground = grayBrush;
            FileSizeTextBlock.Foreground = grayBrush;           
        }


        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UserFile file = (UserFile)((TextBlock)e.OriginalSource).DataContext;
            file.IsDeleted = true;

            this.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserFile file = (UserFile)((Button)e.OriginalSource).DataContext;
            file.IsDeleted = true;

            this.Visibility = Visibility.Collapsed;
        }


    }
}
