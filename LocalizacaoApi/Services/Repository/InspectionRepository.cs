using System;
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
    /// Repository for Inspections.
    /// </summary>
    public class InspectionRepository : IInspectionRepository
    {
        private const string DefaultSql = "SELECT FISLOCA_ID AS Id, LATITUDE, LONGITUDE, PRECISAO, DATA_CRIACAO AS 'Create', DESC_LOCAL AS 'Local', DESC_OBSERVACAO AS Observacao, INDR_TIPOLANCAMENTO AS TipoLancamento, USUARIO_ID AS UsuarioId FROM FIS_LOCALIZACOES ";
        private const string InsertSql = "INSERT INTO FIS_LOCALIZACOES (LATITUDE,LONGITUDE,PRECISAO, DATA_CRIACAO, DESC_LOCAL, DESC_OBSERVACAO, INDR_TIPOLANCAMENTO, USUARIO_ID) VALUES(@Latitude, @Longitude, @Precisao, @Create, @Local, @Observacao, @TipoLancamento, @UsuarioId); SELECT LAST_INSERT_ID() ";
        private const string UpdateSql = "UPDATE FIS_LOCALIZACOES SET FISLOCA_ID = @Id, LATITUDE = @Latitude, LONGITUDE = @Longitude, PRECISAO = @precisao, DATA_CRIACAO = @Create, DESC_LOCAL = @Local, DESC_OBSERVACAO = @Observacao, INDR_TIPOLANCAMENTO = @TipoLancamento, USUARIO_ID = @UsuarioId WHERE FISLOCA_ID = @Id ";
        private const string DeleteSql = "DELETE FROM FIS_LOCALIZACOES WHERE FISLOCA_ID = @Id";
        
        private IDbConnection db;
        /// <summary>
        /// Constructor.
        /// </summary>
        public InspectionRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["mysqlString"];
            if (connectionString == null)
            {
                throw new ArgumentNullException("mysqlString", "Connection Parameters not defined.");
            }

            db = new MySql.Data.MySqlClient.MySqlConnection(connectionString.ConnectionString);
        }

        /// <summary>
        /// Get all <see cref="Inspection"/> by given <see cref="User"/>.
        /// </summary>
        /// <param name="user">User Author.</param>
        /// <returns>List of Inspections founded.</returns>
        public IList<Inspection> GetAllByUser(User user)
        {
            var where = "WHERE USUARIO_ID = @Id";
            return db.Query<Inspection>(DefaultSql + where, user).ToList();
        }

        /// <summary>
        /// Get all <see cref="Inspection"/> between <param name="initial">Initial Date</param> and <param name="final">Final date</param>
        /// by the <param name="user">User Id</param>.
        /// </summary>
        /// <returns>List of Inspections founded.</returns>
        public IList<Inspection> GetAllByDates(User user, DateTime initial, DateTime final)
        {
            var where = "WHERE DATA_CRIACAO >= TIMESTAMP(@initial) AND DATA_CRIACAO <= TIMESTAMP(@final) AND USUARIO_ID=@userId";
            return db.Query<Inspection>(DefaultSql + where, new {initial = initial.ToString("yyyy-MM-dd HH:mm:ss"), final = final.ToString("yyyy-MM-dd HH:mm:ss"), userId = user.Id }).ToList();
        }

        /// <summary>
        /// Find by id
        /// </summary>
        /// <param name="id">Id of inspection</param>
        /// <returns>return founded inspection otherwise null.</returns>
        public Inspection Find(int id)
        {
            var where = "WHERE FISLOCA_ID = @id";
            return db.QuerySingleOrDefault<Inspection>(DefaultSql + where, new {id});
        }

        /// <summary>
        /// Add one inspection.
        /// </summary>
        /// <param name="inspec">Inspection which will be included.</param>
        /// <returns>The same inspection plus his <see cref="Inspection.Id"/>.</returns>
        public Inspection Add(Inspection inspec)
        {
            inspec.Id = db.Query<int>(InsertSql, inspec).Single();
            return inspec;
        }

        /// <summary>
        /// Update the inspection
        /// </summary>
        /// <param name="inspec">Inspection which will be updated.</param>
        /// <returns>Echo the inspection.</returns>
        public Inspection Update(Inspection inspec)
        {
            db.Execute(UpdateSql, inspec);
            return inspec;
        }

        /// <summary>
        /// Remove the inspection by his <see cref="Inspection.Id"/>.
        /// </summary>
        /// <param name="id">Inspection Id.</param>
        /// <returns>true if succeed otherwise false.</returns>
        public bool Remove(int id)
        {
            var qtdLines = db.Execute(DeleteSql, new {id});
            return qtdLines > 0;
        }
    }
}