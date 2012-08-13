
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The GetBucketLocationRequest contains the parameters used for the GetBucketLocation operation.
    /// <br />Required Parameters: BucketName
    /// </summary>
    public class GetBucketLocationRequest : CSRequest
    {
        #region Private Members

        private string bucketName;

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
        /// This is the CS Bucket the request will get the location for.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>this instance</returns>
        public GetBucketLocationRequest WithBucketName(string bucketName)
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
    }
}