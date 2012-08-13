
using System;
using System.Collections.Generic;
using System.Text;
using GrandCloud.CS.Util;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The InitiateMultipartUploadResponse contains all the information about the
    /// InitiateMultipartUpload method.
    /// </summary>
    public class InitiateMultipartUploadResponse : CSResponse
    {
        #region Private Members

        private string bucketName;
        private string key;
        private string uploadId;

        #endregion

        /// <summary>
        /// Gets and sets the name of the bucket.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Gets and sets the object key for which the multipart upload was initiated.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets and sets the initiated multipart upload id.
        /// </summary>
        public string UploadId
        {
            get { return this.uploadId; }
            set { this.uploadId = value; }
        }

        /// <summary>
        /// Gets and sets the Headers property.
        /// </summary>
        public override System.Net.WebHeaderCollection Headers
        {
            set
            {
                base.Headers = value;
            }
        }
    }
}
