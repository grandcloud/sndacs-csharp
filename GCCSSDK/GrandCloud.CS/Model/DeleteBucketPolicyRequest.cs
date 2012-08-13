
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The DeleteBucketPolicyRequest contains the parameters used for the DeleteBucketPolicy operation.
    /// <br />Required Parameters: BucketName
    /// </summary>
    public class DeleteBucketPolicyRequest : CSRequest
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
            get
            {
                return this.bucketName;
            }
            set
            {
                this.bucketName = value;
            }
        }

        /// <summary>
        /// Sets the BucketName property for this request.
        /// This is the CS Bucket where the policy will be deleted from.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>this instance</returns>
        public DeleteBucketPolicyRequest WithBucketName(string bucketName)
        {
            this.BucketName = bucketName;
            return this;
        }

        /// <summary>
        /// Checks if BucketName property is set.
        /// </summary>
        /// <returns>true if BucketName property is set.</returns>
        internal bool IsSetBucketName()
        {
            return !System.String.IsNullOrEmpty(this.BucketName);
        }

        #endregion
    }
}
