using System;
using System.Web;
using NHibernate;
using TinyNH.DemoStore.Core.NHibernate;

namespace TinyNH.DemoStore.Admin.Modules
{
	/// <summary>
	/// Coordinates NHibernate session and transaction lifecycle during each web request
	/// </summary>
	public class SessionPerRequestModule : IHttpModule
	{
		public void Init(HttpApplication httpApplication)
		{
			httpApplication.BeginRequest += BeginRequest;
			httpApplication.EndRequest += EndRequest;
			httpApplication.Error += Error;
		}

		private void BeginRequest(object sender, EventArgs e)
		{
            LazyWebSessionContext.BindToCurrentRequest(StartSession);
		}

        private ISession StartSession()
        {
            var factory = MvcApplication.ConfigurationStore.SessionFactory;
            var session = factory.OpenSession();
            session.BeginTransaction();
            return session;
        }

		void Error(object sender, EventArgs e)
		{
			UnbindSessionAndRollback();
		}

		private void EndRequest(object sender, EventArgs e)
		{
			UnbindSessionAndCommit();
		}
        
		private void UnbindSessionAndRollback()
		{
			var session = LazyWebSessionContext.UnbindFromCurrentRequest();
			if (session != null)
			{
				if (session.Transaction != null && session.Transaction.IsActive)
				{
					session.Transaction.Rollback();
				}
			}
		}

		private void UnbindSessionAndCommit()
		{
			var session = LazyWebSessionContext.UnbindFromCurrentRequest();
			if (session != null)
			{
				if (session.Transaction != null && session.Transaction.IsActive)
				{
					session.Transaction.Commit();
				}
				session.Dispose();
			}
		}

		public void Dispose()
		{

		}
	}
}