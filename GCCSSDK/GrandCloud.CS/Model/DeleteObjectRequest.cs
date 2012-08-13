
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The DeleteObjectRequest contains the parameters used for the DeleteObject operation.
    /// <br />Required Parameters: BucketName, Key
    /// <br />The MfaCodes property is required if the bucket containing this object has been
    /// configured with the EnableMfaDelete property. For more information, please see:
    /// <see cref="P:GrandCloud.CS.Model.CSBucketVersioningConfig.EnableMfaDelete"/>.
    /// </summary>
    public class DeleteObjectRequest : CSRequest
    {
        #region Private Members

        private string bucketName;
        private string key;
        private string versionId;
        private Tuple<string, string> mfaCodes;

        #endregion

        #region BucketName
        /// <summary>
        /// Gets and sets the BucketName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketName")]
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Sets the BucketName property for this request.
        /// This is the CS Bucket that contains the CS Object
        /// you want to delete.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>the request with the BucketName set</returns>
        public DeleteObjectRequest WithBucketName(string bucketName)
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
        [XmlElementAttribute(ElementName = "Key")]
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Sets the Key property for this request.
        /// This is the Key for the CS Object you want to delete.
        /// </summary>
        /// <param name="key">The value that Key is set to</param>
        /// <returns>the request with the Key set</returns>
        public DeleteObjectRequest WithKey(string key)
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

    }
}