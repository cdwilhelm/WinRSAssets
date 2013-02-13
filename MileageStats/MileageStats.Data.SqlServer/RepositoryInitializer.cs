using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using MileageStats.Data.SqlServer.Initializers;
using MileageStats.Model;
using System.Data.Sql;
using System.Collections;
using System.Configuration;

namespace MileageStats.Data.SqlServer
{
    public class RepositoryInitializer : IRepositoryInitializer
    {
        private IUnitOfWork unitOfWork;

        public RepositoryInitializer(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            this.unitOfWork = unitOfWork;

            Database.DefaultConnectionFactory = new SqlConnectionFactory(ConfigurationManager.ConnectionStrings["MileageStatsDbContext"].ConnectionString);

            Database.SetInitializer(new RelyOnBackupRestoreSqlServerInitializer<MileageStatsDbContext>());
        }

        protected MileageStatsDbContext Context
        {
            get { return (MileageStatsDbContext)this.unitOfWork; }
        }

        public void Initialize()
        {
            this.Context.Set<Country>().ToList().Count();

            var indexes = this.Context.Database.SqlQuery<string>("SELECT name FROM sys.INDEXES;");

            if (!indexes.Contains("IDX_FillupEntries_FillupEntryId"))
            {
                this.Context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_FillupEntries_FillupEntryId ON FillupEntries (FillupEntryId);");
            }

            if (!indexes.Contains("IDX_Reminders_ReminderId"))
            {
                this.Context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_Reminders_ReminderId ON Reminders (ReminderId);");
            }

            if (!indexes.Contains("IDX_VehiclePhotos_VehiclePhotoId"))
            {
                this.Context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IDX_VehiclePhotos_VehiclePhotoId ON VehiclePhotos (VehiclePhotoId);");
            }
        }
    }
}
