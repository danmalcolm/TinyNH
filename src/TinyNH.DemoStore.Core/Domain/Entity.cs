using System;

namespace TinyNH.DemoStore.Core.Domain
{
	public abstract class Entity
	{
	    private int? hashCode = null;

		protected Entity()
		{
			Id = Guid.Empty;
		}

		public virtual Guid Id { get; protected set; }

		#region Equals / hashcode

		public override bool Equals(object obj)
		{
			var other = obj as Entity;

			// Other is null or not an Entity
			if (ReferenceEquals(null, other)) 
				return false;

			// Same object reference
			if (ReferenceEquals(this, other))
				return true;

			// Other not of same type (we include derived types to allow for ORM proxy classes)
			if (!this.GetType().IsInstanceOfType(other)) 
				return false;
			
			if (Equals(Guid.Empty, this.Id) && Equals(Guid.Empty, other.Id))
				return false;

			return Equals(this.Id, other.Id);
		}

		public override int GetHashCode()
		{
// ReSharper disable NonReadonlyFieldInGetHashCode
            if (!hashCode.HasValue)
            {
                if (Id == Guid.Empty)
                {
                    hashCode = base.GetHashCode();
                }
                else
                {
                    hashCode = Id.GetHashCode();
                }
            }
		    return hashCode.Value;
// ReSharper restore NonReadonlyFieldInGetHashCode
		}

	    public static bool operator ==(Entity left, Entity right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Entity left, Entity right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}