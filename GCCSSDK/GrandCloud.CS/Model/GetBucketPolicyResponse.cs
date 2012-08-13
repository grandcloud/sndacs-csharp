
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The GetBucketPolicyResponse contains the JSON string representation of the policy
    /// any headers returned by CS.
    /// </summary>
    public class GetBucketPolicyResponse : CSResponse
    {
        #region Private Members

        private string policy;

        #endregion


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
        /// The request to get the policy is return as the content
        /// body of the response.
        /// </summary>
        /// <param name="responseBody">The policy</param>
        internal override void ProcessResponseBody(string responseBody)
        {
            this.Policy = responseBody;
        }
    }
}
