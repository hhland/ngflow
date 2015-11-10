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
using System.Collections.ObjectModel;
using mpost.SilverlightFramework;
using System.IO;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode
{
    public partial class Page : UserControl
    {
        private int _maxFileSize = int.MaxValue;

        public FileCollection _files;
        private int _maxUpload = 2;
        private string _customParams;
        private string _fileFilter;


        //public Page(IDictionary<string, string> initParams)
        //{            
        //    InitializeComponent();

        //    LoadConfiguration(initParams);

        //    _files = new FileCollection(_customParams, _maxUpload);


        //    FileList.ItemsSource = _files;
        //    FilesCount.DataContext = _files;
        //    TotalProgress.DataContext = _files;
        //    TotalKB.DataContext = _files;

        //}
        private static Page _Instance;
        public static Page Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Page();
                }
                return _Instance;
            }
        }

        private Page()
        {
            InitializeComponent();

            LoadConfiguration();

            _files = new FileCollection(_customParams, _maxUpload);


            FileList.ItemsSource = _files;
            FilesCount.DataContext = _files;
            TotalProgress.DataContext = _files;
            TotalKB.DataContext = _files;

        }
        private void LoadConfiguration()
        {
            string tryTest = string.Empty;


            // Obtain relevant information from the configuration file 
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxFileSizeKB"]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings["MaxFileSizeKB"], out _maxFileSize))
                    _maxFileSize = _maxFileSize * 1024;
            }


            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxUploads"]))
                int.TryParse(ConfigurationManager.AppSettings["MaxUploads"], out _maxUpload);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FileFilter"]))
                _fileFilter = ConfigurationManager.AppSettings["FileFilter"];

        }
        /// <summary>
        ///  Load configuration parameters  then from .Config file
        /// </summary>
        /// <param name="initParams"></param>
        //private void LoadConfiguration(IDictionary<string, string> initParams)
        //{
        //    string tryTest = string.Empty;

        //    // Load custom configuration information string 
        //    if (initParams.ContainsKey("CustomParam") && !string.IsNullOrEmpty(initParams["CustomParam"]))
        //        _customParams = initParams["CustomParam"];

        //    if (initParams.ContainsKey("MaxUploads") && !string.IsNullOrEmpty(initParams["MaxUploads"]))
        //    {
        //        int.TryParse(initParams["MaxUploads"], out _maxUpload);            
        //    }

        //    if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
        //    {
        //        if (int.TryParse(initParams["MaxFileSizeKB"], out _maxFileSize))
        //            _maxFileSize = _maxFileSize * 1024;
        //    }

        //    if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
        //        _fileFilter = initParams["FileFilter"];



        //    // Obtain relevant information from the configuration file 
        //    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxFileSizeKB"]))
        //    {
        //        if (int.TryParse(ConfigurationManager.AppSettings["MaxFileSizeKB"], out _maxFileSize))
        //            _maxFileSize = _maxFileSize * 1024;
        //    }


        //    if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxUploads"]))
        //        int.TryParse(ConfigurationManager.AppSettings["MaxUploads"], out _maxUpload);

        //    if(!string.IsNullOrEmpty( ConfigurationManager.AppSettings["FileFilter"]))
        //        _fileFilter = ConfigurationManager.AppSettings["FileFilter"];

        //}


        /// <summary>
        ///  Select File dialog box event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            try
            {
                if (!string.IsNullOrEmpty(_fileFilter))
                    ofd.Filter = _fileFilter;
            }
            catch (ArgumentException ex)
            {
                //User supplied a wrong configuration file
                throw new Exception("Wrong file filter configuration.", ex);
            }

            if (ofd.ShowDialog() == true)
            {

                foreach (FileInfo file in ofd.Files)
                {
                    string fileName = file.Name;
                    UserFile userFile = new UserFile();
                    userFile.FileName = file.Name;
                    userFile.FileStream = file.OpenRead();

                    if (userFile.FileStream.Length <= _maxFileSize)
                    {
                        // Adding file information to the file list 
                        _files.Add(userFile);
                    }
                    else
                    {
                        MessageBoxControl.Message = "Maximum file size is: " + (_maxFileSize / 1024).ToString() + "KB.";
                        MessageBoxControl.Visibility = Visibility.Visible;
                    }
                }
            }
        }



        /// <summary>
        ///  Start uploading 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (_files.Count == 0)
            {
                MessageBoxControl.Message = "No files to upload. Please select one or more files first.";
                MessageBoxControl.Visibility = Visibility.Visible;
            }
            else
            {
                _files.UploadFiles();
            }
        }

        /// <summary>
        ///  Empty the list of files to upload 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _files.Clear();
        }
    }
}
