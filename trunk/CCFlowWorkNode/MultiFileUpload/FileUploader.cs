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
using System.ServiceModel;
using mpost.SilverlightFramework;
using System.IO;
using WorkNode.UploadService;
/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode.Classes
{
    /// <summary>
    ///  File upload class 
    /// </summary>
    public class FileUploader
    {
        private UserFile _file;
        private long _dataLength;
        private long _dataSent;
       
        private SilverlightUploadServiceSoapClient _client;
        private string _initParams;
        private bool _firstChunk = true;
        private bool _lastChunk = false;
        

        public FileUploader(UserFile file)
        {
            _file = file;

            _dataLength = _file.FileStream.Length;
            _dataSent = 0;

            //Create WCF connection
            //BasicHttpBinding binding = new BasicHttpBinding();
            //EndpointAddress address = new EndpointAddress(new CustomUri("SilverlightUploadService.svc"));
            //_client = new UploadService.UploadServiceClient(binding, address);
            //_client = new UploadService.UploadServiceClient();
            //_client.InnerChannel.Closed += new EventHandler(InnerChannel_Closed);
            
            _client = new SilverlightUploadServiceSoapClient();
            // Event binding 
            _client.StoreFileAdvancedCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_StoreFileAdvancedCompleted);
            _client.CancelUploadCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(_client_CancelUploadCompleted);
            _client.ChannelFactory.Closed += new EventHandler(ChannelFactory_Closed);
        }

        #region
        /// <summary>
        ///  Shut down ChannelFactory Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelFactory_Closed(object sender, EventArgs e)
        {
            ChannelIsClosed();
        }

        void _client_CancelUploadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // When the upload is complete cancellation after closing Channel
            _client.ChannelFactory.Close();
        }

        /// <summary>
        /// Channel Is closed 
        /// </summary>
        private void ChannelIsClosed()
        {
            if (!_file.IsDeleted)
            {
                if (UploadFinished != null)
                    UploadFinished(this, null);
            }
        }

        /// <summary>
        ///  Cancel Upload 
        /// </summary>
        public void CancelUpload()
        {
            _client.CancelUploadAsync(_file.FileName);
        }
        #endregion

        /// <summary>
        ///  Upload complete event handler object declarations 
        /// </summary>
        public event EventHandler UploadFinished;

        public void UploadAdvanced(string initParams)
        {
            _initParams = initParams;

            UploadAdvanced();
        }

        /// <summary>
        ///  Upload file 
        /// </summary>
        private void UploadAdvanced()
        {
            
            byte[] buffer = new byte[4 * 4096];
            int bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length);

            // Whether the file upload is complete ?
            if (bytesRead != 0)
            {
                _dataSent += bytesRead;

                if (_dataSent == _dataLength)
                    _lastChunk = true;// Whether it is the last piece of data , This WCF Will be determined based on whether the information in the service side of the temporary file is renamed 

                // Upload current data block 
                _client.StoreFileAdvancedAsync(_file.FileName, buffer, bytesRead, _initParams, _firstChunk, _lastChunk);


                // After the first message has been false
                _firstChunk = false;

                // Modify notification upload progress 
                OnProgressChanged();
            }
            else
            {
                // When the upload is finished 
                _file.FileStream.Dispose();
                _file.FileStream.Close();

                _client.ChannelFactory.Close();          
            }

        }

        /// <summary>
        ///  Modify schedule property 
        /// </summary>
        private void OnProgressChanged()
        {
            _file.BytesUploaded = _dataSent;//注: Here will first call FileCollection The namesake property , Then is _file.BytesUploaded Binding Properties 
        }

        void _client_StoreFileAdvancedCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Inspection WEB Services for errors 
            if (e.Error != null)
            {
                // When an error uploading abandon 
                _file.State = Constants.FileStates.Error;
            }
            else
            {
                // If the file is not uploaded, then canceled , Continue to upload 
                if (!_file.IsDeleted)
                    UploadAdvanced();
            }
        }

    }
}
