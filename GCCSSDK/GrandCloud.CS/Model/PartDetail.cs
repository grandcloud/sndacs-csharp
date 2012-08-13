
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// PartDetails is a container for elements related to a particular part. A response can contain
    /// zero or more Part elements.
    /// </summary>
    public class PartDetail
    {
        private int partNumber;
        private DateTime lastModified;
        private string eTag;
        private long size;

        /// <summary>
        /// Gets and sets the part number identifying the part.
        /// </summary>
        public int PartNumber
        {
            get { return this.partNumber; }
            set { this.partNumber = value; }
        }

        /// <summary>
        /// Gets and sets the date and time at which the part was uploaded.
        /// </summary>
        public DateTime LastModified
        {
            get { return this.lastModified; }
            set { this.lastModified = value; }
        }

        /// <summary>
        /// Gets and sets the entity tag returned when the part was uploaded.
        /// </summary>
        public string ETag
        {
            get { return this.eTag; }
            set { this.eTag = value; }
        }

        /// <summary>
        /// Gets and sets the size of the uploaded part data.
        /// </summary>
        public long Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

    }
}
