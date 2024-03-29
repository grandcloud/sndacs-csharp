
using System;

namespace GrandCloud.CS.Model
{
    #region Tuple

    /// <summary>
    /// Creates a Key Value pair relating the key and value of type
    /// T and U respectively
    /// </summary>
    /// <typeparam name="T">The type of the key</typeparam>
    /// <typeparam name="U">The type of the value</typeparam>
    public class Tuple<T, U> : IEquatable<Tuple<T, U>>
    {
        private T first;
        private U second;

        /// <summary>
        /// The default constructor
        /// </summary>
        public Tuple() { }

        /// <summary>
        /// Constructs a Tuple relating an instance
        /// of type T to an instance of type U
        /// </summary>
        /// <param name="first">The key</param>
        /// <param name="second">The value</param>
        public Tuple(T first, U second)
        {
            this.first = first;
            this.second = second;
        }

        /// <summary>
        ///  Returns the key of this Tuple
        /// </summary>
        public T First
        {
            get { return first; }
            set { this.first = value; }
        }

        /// <summary>
        /// Returns the value of this Tuple
        /// </summary>
        public U Second
        {
            get { return second; }
            set { this.second = value; }
        }

        /// <summary>
        /// Returns the hash code for this tuple
        /// </summary>
        /// <returns>A 32-bit signed integer hash code</returns>
        public override int GetHashCode()
        {
            if (this.first != null && this.second != null)
            {
                return first.GetHashCode() ^ second.GetHashCode();
            }

            return base.GetHashCode();
        }
        /// <summary>
        /// Determines whether two Tuple objects have the same value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if obj is a String  and its value is the same as this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Tuple<T, U>)obj);
        }

        /// <summary>
        /// Determines whether two Tuple objects have the same value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if obj is a String  and its value is the same as this instance; otherwise, false.</returns>
        public bool Equals(Tuple<T, U> other)
        {
            if ((other == null))
            {
                return false;
            }

            if (other.first == null && this.first == null &&
                other.second == null && this.second == null)
            {
                return true;
            }
            
            if((other.first == null && this.first != null) ||
                (other.second == null && this.second != null))
            {
                return false;
            }

            return other.first.Equals(this.first) && other.second.Equals(this.second);
        }
    }

    #endregion
}