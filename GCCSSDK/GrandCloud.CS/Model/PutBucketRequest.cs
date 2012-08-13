
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The PutBucketRequest contains the parameters used for the CreateBucket operation.
    /// The BucketRegion parameter is used if you wish to specify the bucket locality.
    /// <br />Required Parameters: BucketName
    /// <br />Optional Parameters: BucketRegion, Default - CSRegion.HUADONG1
    /// </summary>
    public class PutBucketRequest : CSRequest
    {
        #region Private Member

        private string bucketName;
        private CSRegion bucketRegion;
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
        /// Sets the BucketName property for this request.
        /// This is the CS Bucket that will be created by this request.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>the request with the BucketName set</returns>
        public PutBucketRequest WithBucketName(string bucketName)
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

        #region BucketRegion

        /// <summary>
        /// Gets and sets the BucketRegion property.
        /// Refer <see cref="T:GrandCloud.CS.Model.CSRegion"/>
        /// for a list of possible values.
        /// Default: CSRegion.HUADONG1
        /// <see cref="T:GrandCloud.CS.Model.CSRegion" />
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketRegion")]
        public CSRegion BucketRegion
        {
            get { return this.bucketRegion; }
            set { this.bucketRegion = value; }
        }

        /// <summary>
        /// Sets the BucketRegion property for this request.
        /// When set, this will determine where your data will
        /// reside in CS. Refer <see cref="T:GrandCloud.CS.Model.CSRegion"/>
        /// for a list of possible values.
        /// </summary>
        /// <param name="bucketRegion">The value that BucketRegion is set to</param>
        /// <returns>the request with the BucketRegion set</returns>
        public PutBucketRequest WithBucketRegion(CSRegion bucketRegion)
        {
            BucketRegion = bucketRegion;
            return this;
        }

        /// <summary>
        /// Alternative to setting bucket region by using the region's name.
        /// When set, this will determine where your data will
        /// reside in CS.
        /// Valid values: huabei-1, huadong-1
        /// </summary>
        public string BucketRegionName
        {
            get { return this.bucketRegionName; }
            set
            {
                this.bucketRegionName = value;
            }
        }

        /// <summary>
        /// Alternative to setting bucket region by using the region's name.
        /// When set, this will determine where your data will
        /// reside in CS.
        /// Valid values: huabei-1, huadong-1
        /// </summary>
        /// <param name="bucketRegionName">The value that BucketRegionName is set to</param>
        /// <returns>the request with the BucketRegionName set</returns>
        public PutBucketRequest WithBucketRegionName(string bucketRegionName)
        {
            BucketRegionName = bucketRegionName;
            return this;
        }

        #endregion

        //#region Grants

        ///// <summary>
        ///// Adds Custom Access Control Lists to this request.
        ///// Please refer to <see cref="T:GrandCloud.CS.Model.CSGrant"/> for information on
        ///// CS Grants.
        ///// </summary>
        ///// <param name="grants">One or more CS Grants.</param>
        ///// <returns>The request with the Grants set.</returns>
        //public PutBucketRequest WithGrants(params CSGrant[] grants)
        //{
        //    this.Grants.AddRange(grants);
        //    return this;
        //}

        //#endregion
    }
}