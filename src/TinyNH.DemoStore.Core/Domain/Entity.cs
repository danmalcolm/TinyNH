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

		public virtual bool Equals(Entity other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return Equals(other.Id, this.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (!(obj is Entity)) return false;
			return Equals((Entity)obj);
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