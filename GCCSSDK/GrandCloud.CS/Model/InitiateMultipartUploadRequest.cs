
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The InitiateMultipartUploadRequest contains the parameters used for the InitiateMultipartUpload method.
    /// <br />Required Parameters: BucketName, Key
    /// </summary>
    public class InitiateMultipartUploadRequest : CSRequest
    {

        private string bucketName;
        private string key;
        private string contentType;
        internal NameValueCollection metaData;
        

        #region BucketName

        /// <summary>
        /// Gets and sets the BucketName property.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Sets the BucketName property for this request.
        /// This is the CS Bucket where the CS Object you are
        /// creating gets put.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>The request with the BucketName set</returns>
        public InitiateMultipartUploadRequest WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        /// <summary>
        /// Checks if BucketName property is set.
        /// </summary>
        /// <returns>true if BucketName property is set.</returns>
        internal bool IsSetBucketName()
        {
            return !System.String.IsNullOrEmpty(this.bucketName);
        }

        #endregion

        #region Key
        /// <summary>
        /// Gets and sets the Key property.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Sets the Key property for this request.
        /// This is the Key for the CS Object you create.
        /// </summary>
        /// <param name="key">The value that Key is set to</param>
        /// <returns>the request with the Key set</returns>
        public InitiateMultipartUploadRequest WithKey(string key)
        {
            this.key = key;
            return this;
        }

        /// <summary>
        /// Checks if Key property is set.
        /// </summary>
        /// <returns>true if Key property is set.</returns>
        internal bool IsSetKey()
        {
            return !System.String.IsNullOrEmpty(this.key);
        }

        #endregion

        #region ContentType
        /// <summary>
        /// Gets and sets the ContentType property.
        /// </summary>
        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        /// <summary>
        /// Sets the ContentType property for this request.
        /// This property defaults to "binary/octet-stream",
        /// but if you require something else you can set this property.
        /// </summary>
        /// <param name="contentType">the value the ContentType to be set to</param>
        /// <returns>The request with the ContentType set</returns>
        public InitiateMultipartUploadRequest WithContentType(string contentType)
        {
            this.contentType = contentType;
            return this;
        }

        /// <summary>
        /// Checks if ContentType property is set.
        /// </summary>
        /// <returns>true if ContentType property is set.</returns>
        internal bool IsSetContentType()
        {
            return !System.String.IsNullOrEmpty(this.contentType);
        }

        #endregion

        #region Metadata

        /// <summary>
        /// Adds a key/value pair to the Metadata property for this request.
        /// The CS Object that you create will have this metadata associated
        /// with it.
        /// </summary>
        /// <param name="key">The key to associate with the object</param>
        /// <param name="value">The value for the key</param>
        /// <returns>The response with Metadata set.</returns>
        public InitiateMultipartUploadRequest WithMetaData(string key, string value)
        {
            if (key == null || value == null)
            {
                return this;
            }

            if (metaData == null)
            {
                metaData = new NameValueCollection(5);
            }

            metaData.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds a set of key-value pairs to the request
        /// </summary>
        /// <param name="metaInfo">The set of key-value pairs that will eventually be
        /// associated with the CS Object</param>
        /// <returns></returns>
        public InitiateMultipartUploadRequest WithMetaData(NameValueCollection metaInfo)
        {
            if (metaInfo == null || metaInfo.Count == 0)
            {
                return this;
            }

            if (metaData == null)
            {
                metaData = new NameValueCollection(metaInfo.Count);
            }

            metaData.Add(metaInfo);

            return this;
        }

        /// <summary>
        /// Checks if Metadata property is set.
        /// </summary>
        /// <returns>true if Metadata property is set.</returns>
        internal bool IsSetMetaData()
        {
            return (metaData != null && metaData.Count > 0);
        }

        /// <summary>
        /// Removes a key from the Metadata list if it was
        /// added previously
        /// </summary>
        /// <param name="key">The key to remove</param>
        public void RemoveMetaData(string key)
        {
            if (metaData == null || metaData.Count == 0)
            {
                return;
            }

            metaData.Remove(key);
        }

        #endregion

    }
}
