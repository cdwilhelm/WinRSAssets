using System;
using System.IO;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MileageStats.Data.SqlServer.Initializers
{
    public abstract class SqlServerInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
    }
}
