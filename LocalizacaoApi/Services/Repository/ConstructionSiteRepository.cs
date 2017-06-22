using Dapper;
using Services.Interfaces.Repository;
using Services.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Services.Repository
{
    /// <summary>
    /// Repository of constructions sites. 
    /// </summary>
    public class ConstructionSiteRepository : IConstructionSiteRepository, IDisposable
    {
        private const string DefaultSql = "SELECT ID as Id, ENTIDADE, LATITUDE, LONGITUDE, DESCRICAO, LOCAL FROM OBRAS";
        private IDbConnection db;

        /// <summary>
        /// Constructor of Repository. 
        /// </summary>
        public ConstructionSiteRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["mysqlString"];
            if (connectionString == null)
            {
                throw new ArgumentNullException("mysqlString", "Connection Parameters not defined.");
            }

            db = new MySql.Data.MySqlClient.MySqlConnection(connectionString.ConnectionString);
        }

        /// <summary>
        /// Get All Constructions Sites.
        /// </summary>
        /// <returns>List of Constructions.</returns>
        public IList<ConstructionSite> GetAll()
        {
            return db.Query<ConstructionSite>(DefaultSql).ToList();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            db?.Dispose();
            db = null;
        }
    }
}