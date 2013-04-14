using System;

namespace TinyNH.DemoStore.Core.Domain
{
	public abstract class Entity
	{
		protected Entity()
		{
			Id = Guid.Empty;
		}

		public virtual Guid Id { get; protected set; }

		#region Equals / hashcode

        /// <summary>
        /// Returns the actual type or the concrete type being proxied if the
        /// instance is a proxy
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetTypeUnproxied()
        {
            // NHibernate generated proxies shadow the GetType() method 
            // which returns the concrete type:
            // https://groups.google.com/forum/?fromgroups=#!topic/sharp-architecture/3dBfm67eAjo
            return GetType();
        }

		public override bool Equals(object obj)
		{
			var other = obj as Entity;

			// Other is null or not an Entity
			if (ReferenceEquals(null, other)) 
				return false;

			// Same object reference
			if (ReferenceEquals(this, other))
				return true;

			// Other must be same type
            if (GetTypeUnproxied() != other.GetTypeUnproxied()) 
				return false;
			
            // Different unsaved entities can't be equal
			if (Equals(Guid.Empty, this.Id) && Equals(Guid.Empty, other.Id))
				return false;

			return Equals(this.Id, other.Id);
		}

		public override int GetHashCode()
		{
            if (Id == Guid.Empty)
            {
                // New entity that has not been assigned an id, use base
                // implementation
                return base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    return (Id.GetHashCode() * 397) ^ GetTypeUnproxied().GetHashCode();
                }
            }
		}

		#endregion
	}
}