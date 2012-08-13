
using System;
using System.Globalization;
using System.Xml.Serialization;


using GrandCloud.CS.Util;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// Represents an CS Bucket. 
    /// Contains a Bucket Name which is the name of the CS Bucket. 
    /// And a Creation Date which is the date that the CS Bucket was created.
    /// </summary>
    [Serializable()]
    public class CSBucket
    {
        #region Private Members

        private string bucketName;
        private DateTime? creationDate;
        private string bucketRegionName;

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
        /// Checks if BucketName property is set
        /// </summary>
        /// <returns>true if BucketName property is set</returns>
        internal bool IsSetBucketName()
        {
            return !System.String.IsNullOrEmpty(this.bucketName);
        }

        #endregion

        #region CreationDate

        /// <summary>
        /// Gets and sets the CreationDate property.
        /// The date conforms to the ISO8601 date format.
        /// </summary>
        [XmlElementAttribute(ElementName = "CreationDate")]
        public string CreationDate
        {
            get
            {
                return this.creationDate.GetValueOrDefault().ToString(
                    CSSDKUtils.GMTDateFormat
                    );
            }
            set
            {
                this.creationDate = DateTime.ParseExact(
                    value,
                    CSSDKUtils.ISO8601DateFormatWithUTCOffset,
                    CultureInfo.InvariantCulture
                    );
            }
        }

        /// <summary>
        /// Checks if CreationDate property is set
        /// </summary>
        /// <returns>true if CreationDate property is set</returns>
        internal bool IsSetCreationDate()
        {
            return this.creationDate.HasValue;
        }

        #endregion

        #region BucketRegionName

        /// <summary>
        /// Gets and sets the BucketRegionName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketRegionName")]
        public string BucketRegionName
        {
            get { return this.bucketRegionName; }
            set
            {
                if (!string.IsNullOrEmpty(value) &&
                    (string.Equals(value, CSConstants.REGION_HUABEI_1) ||
                    string.Equals(value, CSConstants.REGION_HUADONG_1))
                    )
                {
                    this.bucketRegionName = value;
                }
                else
                {
                    this.bucketRegionName = "";
                }
            }
        }

        /// <summary>
        /// Checks if BucketRegionName property is set
        /// </summary>
        /// <returns>true if BucketRegionName property is set</returns>
        internal bool IsSetBucketRegionName()
        {
            return !string.IsNullOrEmpty(this.bucketRegionName);
        }

        #endregion
    }
}