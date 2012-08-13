
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// Container for elements related to a particular multipart upload.
    /// </summary>
    public class MultipartUpload
    {
        private string key;
        private string uploadId;
        private string storageClass;
        private DateTime initiated;

        /// <summary>
        /// Gets and sets the Key of the object for which the multipart upload was initiated.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets and sets the Upload ID that identifies the multipart upload.
        /// </summary>
        public string UploadId
        {
            get { return this.uploadId; }
            set { this.uploadId = value; }
        }

        /// <summary>
        /// Gets and sets the class of storage that will be used to store the object when multipart
        /// upload is complete.
        /// </summary>
        public string StorageClass
        {
            get { return this.storageClass; }
            set { this.storageClass = value; }
        }

        /// <summary>
        /// Gets and sets the date and time at which the multipart upload was initiated.
        /// </summary>
        public DateTime Initiated
        {
            get { return this.initiated; }
            set { this.initiated = value; }
        }

    }
}
