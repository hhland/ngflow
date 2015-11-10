using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode.Classes
{
    /// <summary>
    ///  Document collections management classes 
    /// 注:ObservableCollection Is a generic collection classes , When to add or remove the entry （ Or one of the entries to achieve a INotifyPropertyChanged If , When property changes ）,
    ///  It will issue a change notification event （ Perform the collection of the same name in the class attribute ）. This is done in data binding will be very convenient , Because UI Controls can use these notifications to know to automatically refresh their values , Rather than developers to write code to 
    ///  Explicitly do .
    /// </summary>
    public class FileCollection : ObservableCollection<UserFile>
    {
        /// <summary>
        ///  Accumulated uploaded （ Multi-file ） Byte count 
        /// </summary>
        private double _bytesUploaded = 0;
        /// <summary>
        ///  The percentage of the number of characters representing the number of uploaded all the bytes 
        /// </summary>
        private int _percentage = 0;
        /// <summary>
        ///  Current file number being uploaded 
        /// </summary>
        private int _currentUpload = 0;
        /// <summary>
        ///  Upload initialization parameters , Details are as follows :
        /// MaxFileSizeKB: 	File size in KBs.
        /// MaxUploads: 	Maximum number of simultaneous uploads
        /// FileFilter:	File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg
        /// CustomParam: Your custom parameter, anything here will be available in the WCF webservice
        /// DefaultColor: The default color for the control, for example: LightBlue
        /// </summary>
        private string _customParams;
        /// <summary>
        ///  The maximum number of bytes uploaded 
        /// </summary>
        private int _maxUpload;
        
        /// <summary>
        ///  Accumulated uploaded （ Multi-file ） Byte count , Modify event notifications will be sent to the field page.xmal The TotalKB
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));
            }
        }

        /// <summary>
        ///  The percentage of the number of characters representing the number of uploaded all the bytes , Modify event notifications will be sent to the field page.xmal The TotalProgress
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }

        /// <summary>
        ///  Constructor 
        /// </summary>
        /// <param name="customParams"></param>
        /// <param name="maxUploads"></param>
        public FileCollection(string customParams, int maxUploads)
        {
            _customParams = customParams;
            _maxUpload = maxUploads;

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FileCollection_CollectionChanged);
        }

       
        /// <summary>
        ///  Followed by adding the selected file to upload information 
        /// </summary>
        /// <param name="item"></param>
        public new void Add(UserFile item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);            
            base.Add(item);
        }

        /// <summary>
        ///  When uploading a single file attributes change 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // When property changes [ Upload removed from the list ]
            if (e.PropertyName == "IsDeleted")
            {
                UserFile file = (UserFile)sender;

                if (file.IsDeleted)
                {
                    if (file.State == Constants.FileStates.Uploading)
                    {
                        _currentUpload--;
                        UploadFiles();
                    }

                    this.Remove(file);

                    file = null;
                }
            }
            // When property changes [ Start uploading ]
            else if (e.PropertyName == "State")
            {
                UserFile file = (UserFile)sender;
                // At this time file.State Status ploading
                if (file.State == Constants.FileStates.Finished || file.State == Constants.FileStates.Error)
                {
                    _currentUpload--;
                    UploadFiles();
                }
            }
            // When property changes [ Upload in progress ]
            else if (e.PropertyName == "BytesUploaded")
            {
                // Recalculated upload data 
                RecountTotal();
            }
        }
     

        /// <summary>
        ///  Upload file 
        /// </summary>
        public void UploadFiles()
        {
            lock (this)
            {
                foreach (UserFile file in this)
                {   // When uploading files are not removed （IsDeleted） Or suspended 
                    if (!file.IsDeleted && file.State == Constants.FileStates.Pending && _currentUpload < _maxUpload)
                    {
                        file.Upload(_customParams);
                        _currentUpload++;
                    }
                }
            }

        }

        /// <summary>
        ///  Recalculate data 
        /// </summary>
        private void RecountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (UserFile file in this)
            {
                totalSize += file.FileSize;
                totalSizeDone += file.BytesUploaded;
            }

            double percentage = 0;

            if (totalSize > 0)
                percentage = 100 * totalSizeDone / totalSize;

            BytesUploaded = totalSizeDone; 

            Percentage = (int)percentage;
        }

        /// <summary>
        ///  When you add or cancel upload files trigger 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // When the collection of information changes （ Add or delete items ）时, The recalculated data  
            RecountTotal();
        }

    
    }
}
