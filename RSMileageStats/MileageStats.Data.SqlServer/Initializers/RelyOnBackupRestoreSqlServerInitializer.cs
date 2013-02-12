using System;
using System.Data.Entity;
using System.Transactions;

namespace MileageStats.Data.SqlServer.Initializers
{
    internal class RelyOnBackupRestoreSqlServerInitializer<TContext> : SqlServerInitializer<TContext> where TContext : DbContext
    {
        public override void InitializeDatabase(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.Database.Connection.Open();
            Console.WriteLine("No need to initialize object as the db is being restored behind the scenes");
        }
    }
}
