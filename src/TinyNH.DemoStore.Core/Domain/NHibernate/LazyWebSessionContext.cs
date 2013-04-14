using System;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace TinyNH.DemoStore.Core.NHibernate
{
    /// <summary>
    /// An implementation of ICurrentSessionContext that stores NHibernate session in 
    /// the current HttpContext. It is initialized with a Lazy instance responsible 
    /// for creating the session, which ensures that the session will only be started 
    /// if CurrentSession method is invoked. SessionPerRequestModule is responsible 
    /// for starting session and coordinating rollbacks / commits
    /// Based on an example at http://joseoncode.com/2011/03/03/effective-nhibernate-session-management-for-web-apps/
    /// without support for multiple session factories
    /// </summary>
    public class LazyWebSessionContext : ICurrentSessionContext
    {
        private const string SessionInitializerKey = "LazyWebSessionContext.SessionInitializerKey";

        // Required constructor signature
        public LazyWebSessionContext(ISessionFactoryImplementor factory)
        {
        }

        public ISession CurrentSession()
        {
            var initializer = GetCurrentInitializer();
            if (initializer == null)
            {
                return null;
            }
            return initializer.Value;
        }

        /// <summary>
        /// Sets up session creation for the current HttpContext
        /// </summary>
        /// <param name="start">Function to create and initialise the current session</param>
        public static void BindToCurrentRequest(Func<ISession> start)
        {
            var initializer = new Lazy<ISession>(start);
            HttpContext.Current.Items[SessionInitializerKey] = initializer;
        }

        private static Lazy<ISession> GetCurrentInitializer()
        {
            return HttpContext.Current.Items[SessionInitializerKey] as Lazy<ISession>;
        }

        /// <summary>
        /// Unbinds the session from the current HttpContext and returns the ISession instance 
        /// if it has been initialized
        /// </summary>
        /// <returns></returns>
        public static ISession UnbindFromCurrentRequest()
        {
            var initializer = GetCurrentInitializer();
            if (initializer == null || !initializer.IsValueCreated) return null;
            return initializer.Value;
        }
    }
}