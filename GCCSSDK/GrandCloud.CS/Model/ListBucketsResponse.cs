
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The ListBucketsResponse contains the ListBucketsResult and
    /// any headers or metadata returned by CS.
    /// </summary>    
    public class ListBucketsResponse : CSResponse
    {
        #region Private Members

        private List<CSBucket> buckets = new List<CSBucket>();

        #endregion

        #region Bucket

        /// <summary>
        /// Gets the Bucket property. This property has been deprecated -
        /// please use the Buckets property of ListBucketsResponse.
        /// <see cref="P:GrandCloud.CS.Model.ListBucketsResponse.Buckets"/>
        /// </summary>
        [XmlIgnore]
        [System.Obsolete("Use the Buckets property instead")]
        public List<CSBucket> Bucket
        {
            get { return this.Buckets; }
        }

        #endregion

        #region Buckets

        /// <summary>
        /// Gets the Buckets property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Buckets")]
        public List<CSBucket> Buckets
        {
            get { return this.buckets; }
        }

        #endregion

    }
}