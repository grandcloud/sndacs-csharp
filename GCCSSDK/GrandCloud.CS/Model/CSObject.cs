
using System;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;


using GrandCloud.CS.Util;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// Represents an CS Object. Contains all attributes that an CS Object has.
    /// </summary>
    public class CSObject
    {
        #region Private Members

        private string key;
        private DateTime? lastModified;
        private string eTag;
        private long size;
        private string storageClass;
        private string bucketName;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a System.String that represents the CSObject.
        /// </summary>
        /// <returns>A System.String representation of the CSObject.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Properties: {");
            if (IsSetKey())
            {
                sb.Append(String.Concat("Key:", Key));
            }
            sb.Append(String.Concat(", Bucket:", BucketName));
            sb.Append(String.Concat(", LastModified:", LastModified));
            sb.Append(String.Concat(", ETag:", ETag));
            sb.Append(String.Concat(", Size:", Size));
            sb.Append(String.Concat(", StorageClass:", StorageClass));
            sb.Append("}");

            return sb.ToString();
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
        /// Checks if Key property is set.
        /// </summary>
        /// <returns>true if Key property is set.</returns>
        internal bool IsSetKey()
        {
            return !System.String.IsNullOrEmpty(this.key);
        }

        #endregion

        #region BucketName

        /// <summary>
        /// Gets and sets the BucketName property.
        /// This is the name of the CS Bucket in which the
        /// key is stored.
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketName")]
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
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

        #region LastModified

        /// <summary>
        /// Gets and sets the LastModified property.
        /// Date retrieved from CS is in ISO8601 format.
        /// GMT formatted date is passed back to the user.
        /// </summary>
        [XmlElementAttribute(ElementName = "LastModified")]
        public string LastModified
        {
            get
            {
                return this.lastModified.GetValueOrDefault().ToString(
                    CSSDKUtils.GMTDateFormat
                    );
            }
            set
            {
                this.lastModified = DateTime.ParseExact(
                    value,
                    CSSDKUtils.ISO8601DateFormatWithUTCOffset,
                    CultureInfo.InvariantCulture
                    );
            }
        }

        /// <summary>
        /// Checks if LastModified property is set.
        /// </summary>
        /// <returns>true if LastModified property is set.</returns>
        internal bool IsSetLastModified()
        {
            return this.lastModified.HasValue;
        }

        #endregion

        #region ETag

        /// <summary>
        /// Gets and sets the ETag property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ETag")]
        public string ETag
        {
            get { return this.eTag; }
            set { this.eTag = value; }
        }

        /// <summary>
        /// Checks if ETag property is set.
        /// </summary>
        /// <returns>true if ETag property is set.</returns>
        internal bool IsSetETag()
        {
            return !System.String.IsNullOrEmpty(this.eTag);
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets and sets the Size property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Size")]
        public long Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        #endregion

        #region StorageClass

        /// <summary>
        /// Gets and sets the StorageClass property.
        /// </summary>
        [XmlElementAttribute(ElementName = "StorageClass")]
        public string StorageClass
        {
            get { return this.storageClass; }
            set { this.storageClass = value; }
        }

        /// <summary>
        /// Checks if StorageClass property is set.
        /// </summary>
        /// <returns>true if StorageClass property is set.</returns>
        internal bool IsSetStorageClass()
        {
            return !System.String.IsNullOrEmpty(this.storageClass);
        }

        #endregion
    }
}