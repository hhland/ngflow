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
using System.ComponentModel;
using System.IO;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode.Classes
{
    /// <summary>
    ///  Upload file information class 
    /// </summary>
    public class UserFile : INotifyPropertyChanged
    {
        /// <summary>
        ///  Upload file name 
        /// </summary>
        private string _fileName;
        /// <summary>
        ///  Whether to cancel the upload the file 
        /// </summary>
        private bool _isDeleted = false;
        /// <summary>
        ///  Flow of information uploaded files 
        /// </summary>
        private Stream _fileStream;
        /// <summary>
        ///  The current state of the uploaded file 
        /// </summary>
        private Constants.FileStates _state = Constants.FileStates.Pending;
        /// <summary>
        ///  The current number of bytes uploaded （ Here and FileCollection The significance of the different attributes of the same name ,FileCollection The total number of bytes of all files uploaded ）
        /// </summary>
        private double _bytesUploaded = 0;
        /// <summary>
        ///  Current file size 
        /// </summary>
        private double _fileSize = 0;
        /// <summary>
        ///  Percentage uploaded files 
        /// </summary>
        private int _percentage = 0;
        /// <summary>
        ///  Upload a file type 
        /// </summary>
        private FileUploader _fileUploader;

        /// <summary>
        ///  Upload file name 
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        /// <summary>
        ///  The current status of the uploaded file , Note then use the NotifyPropertyChanged To inform FileRowControl Control of FileRowControl_PropertyChanged Event 
        /// </summary>
        public Constants.FileStates State
        {
            get { return _state; }
            set
            {
                _state = value;

                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        ///  Whether the current upload files have been removed , Note then use the NotifyPropertyChanged To inform FileCollection Class item_PropertyChanged Event 
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;

                if (_isDeleted)
                    CancelUpload();

                NotifyPropertyChanged("IsDeleted");
            }
        }

        /// <summary>
        ///  Flow of information uploaded files 
        /// </summary>
        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;

                if (_fileStream != null)
                    _fileSize = _fileStream.Length;

            }
        }

        /// <summary>
        ///  Current file size 
        /// </summary>
        public double FileSize
        {
            get
            {
                return _fileSize;
            }
        }

        /// <summary>
        ///  The current number of bytes uploaded （ Here and FileCollection The significance of the different attributes of the same name ,FileCollection The total number of bytes of all files uploaded ）
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                NotifyPropertyChanged("BytesUploaded");

                Percentage = (int)((value * 100) / _fileStream.Length);

            }
        }

        /// <summary>
        ///  Percentage uploaded files （ Here and FileCollection The significance of the different attributes of the same name ,FileCollection In the percentage of the number of characters representing the number of uploaded all bytes , Modify event notifications will be sent to the field page.xmal The TotalProgress）
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                NotifyPropertyChanged("Percentage");
            }
        }


        /// <summary>
        ///  Upload the current file 
        /// </summary>
        /// <param name="initParams"></param>
        public void Upload(string initParams)
        {
            this.State = Constants.FileStates.Uploading;
            _fileUploader = new FileUploader(this);
            _fileUploader.UploadAdvanced(initParams);
            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);
        }

        /// <summary>
        ///  Cancel Upload ,注: This file is only in this class IsDeleted Property use 
        /// </summary>
        public void CancelUpload()
        {
            if (_fileUploader != null && this.State == Constants.FileStates.Uploading)
            {
                _fileUploader.CancelUpload();
            }
        }

        /// <summary>
        ///  The current file upload is complete 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileUploader_UploadFinished(object sender, EventArgs e)
        {
            _fileUploader = null;

            this.State = Constants.FileStates.Finished;
        }

        private string _Memo = string.Empty;
        /// <summary>
        ///  Remark 
        /// </summary>
        public string Memo
        {
            get { return _Memo; }
            set
            {
                _Memo = value;
                NotifyPropertyChanged("Memo");

            }
        }

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
