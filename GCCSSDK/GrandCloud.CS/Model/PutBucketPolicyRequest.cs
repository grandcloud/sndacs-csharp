

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The PutBucketPolicyRequest contains the parameters used for the SetBucketPolicy operation.
    /// <br />Required Parameters: BucketName, Policy
    /// </summary>
    public class PutBucketPolicyRequest : CSRequest
    {
        #region Private Members

        private string bucketName;
        private string policy;

        #endregion

        #region Properties

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
        /// This is the CS Bucket the request will get the location for.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>this instance</returns>
        public PutBucketPolicyRequest WithBucketName(string bucketName)
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


        /// <summary>
        /// Gets and sets the Policy property.
        /// </summary>
        public String Policy
        {
            get
            {
                return this.policy;
            }
            set
            {
                this.policy = value;
            }
        }

        /// <summary>
        /// Sets the Policy property for this request.
        /// This is the JSON string representing the policy that will be applied to the CS Bucket.
        /// </summary>
        /// <param name="policy">The JSON string for the policy</param>
        /// <returns>this instance</returns>
        public PutBucketPolicyRequest WithPolicy(string policy)
        {
            this.Policy = policy;
            return this;
        }

        /// <summary>
        /// Checks if policy property is set.
        /// </summary>
        /// <returns>true if Policy property is set.</returns>
        internal bool IsSetPolicy()
        {
            return !System.String.IsNullOrEmpty(this.Policy);
        }

        #endregion
    }
}
