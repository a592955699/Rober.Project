using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rober.Core
{
    //public abstract partial class BaseEntity : BaseEntity<int>
    public partial class BaseEntity : BaseEntity<int>
    {

    }

    /// <summary>
    /// Base class for entities
    /// </summary>
    //public partial class BaseEntity<TKeyType>
    public class BaseEntity<TKeyType>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKeyType Id { get; set; }

        /// <summary>
        /// Is transient
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Result</returns>
        private static bool IsTransient(BaseEntity<TKeyType> obj)
        {
            return obj != null && Equals(obj.Id, default(TKeyType));
        }

        /// <summary>
        /// Get unproxied type
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Result</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity<TKeyType>);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other">other entity</param>
        /// <returns>Result</returns>
        public virtual bool Equals(BaseEntity<TKeyType> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(Id, default(TKeyType)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator ==(BaseEntity<TKeyType> x, BaseEntity<TKeyType> y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Not equal
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Result</returns>
        public static bool operator !=(BaseEntity<TKeyType> x, BaseEntity<TKeyType> y)
        {
            return !(x == y);
        }
    }
}
