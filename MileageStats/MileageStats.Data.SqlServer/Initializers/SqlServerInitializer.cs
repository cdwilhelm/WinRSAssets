using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

namespace MileageStats.Data.SqlServer.Initializers
{
    internal abstract class SqlServerInitializer <T> : IDatabaseInitializer<T> where T : DbContext
    {
        public abstract void InitializeDatabase(T context);

        protected virtual void Seed(T context)
        {
            ISeedDatabase seeder = context as ISeedDatabase;
            if (seeder != null)
            {
                seeder.Seed();
            }
        }
    }
}
