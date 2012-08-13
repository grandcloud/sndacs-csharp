
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The ListObjectsRequest contains the parameters used for the ListObjects operation.
    /// <br />Required Parameters: BucketName
    /// <br />Optional Parameters: Prefix, Marker, MaxKeys, Delimiter
    /// </summary>
    public class ListObjectsRequest : CSRequest
    {
        #region Private Members

        private string bucketName;
        private string prefix;
        private string marker;
        private int maxKeys = -1;
        private string delimiter;

        #endregion

        #region BucketName

        /// <summary>
        /// Gets and sets the BucketName property.
        /// This is the name of the CS Bucket to list keys from.
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketName")]
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Sets the BucketName property for this request.
        /// This is the name of the CS Bucket to list keys from.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>this instance</returns>
        public ListObjectsRequest WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        /// <summary>
        /// Checks if BucketName property is set
        /// </summary>
        /// <returns>true if BucketName property is set</returns>
        internal bool IsSetBucketName()
        {
            return !System.String.IsNullOrEmpty(this.bucketName);
        }

        #endregion

        #region Prefix

        /// <summary>
        /// Gets and sets the Prefix property.
        /// All keys matched will have this prefix.
        /// </summary>
        [XmlElementAttribute(ElementName = "Prefix")]
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix=value; }
        }

        /// <summary>
        /// Sets the Prefix property for this request.
        /// All keys matched will have this prefix.
        /// </summary>
        /// <param name="prefix">The value that Prefix is set to</param>
        /// <returns>this instance</returns>
        public ListObjectsRequest WithPrefix(string prefix)
        {
            this.prefix = prefix;
            return this;
        }

        /// <summary>
        /// Checks if Prefix property is set
        /// </summary>
        /// <returns>true if Prefix property is set</returns>
        internal bool IsSetPrefix()
        {
            return !System.String.IsNullOrEmpty(this.prefix);
        }

        #endregion

        #region Marker

        /// <summary>
        /// Gets and sets the Marker property.
        /// All keys returned will be lexiographically after the marker.
        /// </summary>
        [XmlElementAttribute(ElementName = "Marker")]
        public string Marker
        {
            get { return this.marker; }
            set { this.marker = value; }
        }

        /// <summary>
        /// Sets the Marker property for this request.
        /// All keys returned will be lexiographically after the marker.
        /// </summary>
        /// <param name="marker">the value that Marker is set to</param>
        /// <returns>this instance</returns>
        public ListObjectsRequest WithMarker(string marker)
        {
            this.marker = marker;
            return this;
        }

        /// <summary>
        /// Checks if Marker property is set
        /// </summary>
        /// <returns>true if Marker property is set</returns>
        internal bool IsSetMarker()
        {
            return !System.String.IsNullOrEmpty(this.marker);
        }

        #endregion

        #region MaxKeys

        /// <summary>
        /// Gets and sets the MaxKeys property.
        /// Limits the result set of keys to MaxKeys.
        /// </summary>
        [XmlElementAttribute(ElementName = "MaxKeys")]
        public int MaxKeys
        {
            get { return this.maxKeys; }
            set { this.maxKeys = value; }
        }

        /// <summary>
        /// Sets the MaxKeys property for this request.
        /// Limits the result set of keys to MaxKeys.
        /// </summary>
        /// <param name="maxKeys">the value that MaxKeys is set to</param>
        /// <returns>this instance</returns>
        public ListObjectsRequest WithMaxKeys(int maxKeys)
        {
            this.maxKeys = maxKeys;
            return this;
        }

        /// <summary>
        /// Checks if MaxKeys property is set
        /// </summary>
        /// <returns>true if MaxKeys property is set</returns>
        internal bool IsSetMaxKeys()
        {
            return this.maxKeys >= 0;
        }

        #endregion

        #region Delimiter

        /// <summary>
        /// Gets and sets the Delimiter property.
        /// Causes keys that contain the same string between the prefix and the 
        /// first occurrence of the delimiter to be rolled up into a single result 
        /// element in the CommonPrefixes collection.
        /// </summary>
        /// <remarks>
        /// These rolled-up keys are not returned elsewhere in the response.
        /// </remarks>
        [XmlElementAttribute(ElementName = "Delimiter")]
        public string Delimiter
        {
            get { return this.delimiter; }
            set { this.delimiter = value; }
        }

        /// <summary>
        /// Sets the Delimiter property for this request.
        /// Causes keys that contain the same string between the prefix and the 
        /// first occurrence of the delimiter to be rolled up into a single result 
        /// element in the CommonPrefixes collection. 
        /// These rolled-up keys are not returned elsewhere in the response.
        /// </summary>
        /// <param name="delimiter">the value that Delimiter is set to.</param>
        /// <returns>this instance</returns>
        public ListObjectsRequest WithDelimiter(string delimiter)
        {
            this.delimiter = delimiter;
            return this;
        }

        /// <summary>
        /// Checks if Delimiter property is set
        /// </summary>
        /// <returns>true if Delimiter property is set</returns>
        internal bool IsSetDelimiter()
        {
            return !System.String.IsNullOrEmpty(this.delimiter);
        }

        #endregion
    }
}