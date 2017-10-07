using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

using Dapper;

using Services.Interfaces.Repository;
using Services.Objects;

namespace Services.Repository
{
    /// <summary>
    /// Repository of Constructions Sites.
    /// </summary>
    public class PlaceRepository : IPlaceRepository
    {
        private const string DefaultSql = "SELECT ID, NOME_ENTIDADE AS EntityName, LATITUDE, LONGITUDE, LOCAL, DESCRICAO AS Description FROM OBRAS ";
        private readonly IDbConnection db;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlaceRepository()
        {
            db = new MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlString"].ConnectionString);
        }

        /// <summary>
        /// Find place by his Id.
        /// </summary>
        /// <param name="id"> Identification of the place  </param>
        /// <returns> Place found otherwise null. </returns>
        public Place Find(int id)
        {
            var where = "WHERE ID = @idPlace";
            return db.Query<Place>(DefaultSql + where, new { idPlace = id }).FirstOrDefault();
        }

        /// <summary>
        /// Get all Places on the base.
        /// </summary>
        /// <returns>List of every <see cref="Place"/>construction sites</returns> found.
        public List<Place> GetAll()
        {
            return db.Query<Place>(DefaultSql).ToList();
        }
    }
}