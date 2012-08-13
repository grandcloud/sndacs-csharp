
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The GetBucketLocationResponse contains the GetBucketLocationResult and 
    /// any headers returned by CS.
    /// </summary>    
    public class GetBucketLocationResponse : CSResponse
    {
        #region Private Members

        private string location;

        #endregion

        #region Location

        /// <summary>
        /// Gets and sets the Location property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Location")]
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        #endregion
    }
}