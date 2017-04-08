using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Services.Interfaces.Repository;
using Services.Objects;
using Dapper;

namespace Services.Repository
{
    /// <summary>
    /// Repository of Users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private const string DefaultSql = "SELECT USUARIO_ID AS ID, USER_NAME AS USERNAME, SENHA AS PASSWORD FROM USUARIOS ";
        private const string InsertSql = "INSERT INTO USUARIOS (USER_NAME, SENHA) VALUES (@UserName, @Password); SELECT LAST_INSERT_ID() ";
        private const string UpdateSql = "UPDATE USUARIOS SET USER_NAME = @UserName, SENHA = @Password WHERE USUARIO_ID = @Id ";
        private readonly IDbConnection db;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserRepository()
        {
            db = new MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlString"].ConnectionString);
        }

        /// <summary>
        /// Find user by his <param name="id">Identification</param>.
        /// </summary>
        /// <returns>User found otherwise null. </returns>
        public User Find(int id)
        {
            var where = "WHERE USUARIO_ID = @idUser";
            return db.Query<User>(DefaultSql + where, new { idUser = id }).SingleOrDefault();
        }

        /// <summary>
        /// Get all Users on the base.
        /// </summary>
        /// <returns>List of every <see cref="User"/>user</returns> found.
        public List<User> GetAll()
        {
            return db.Query<User>(DefaultSql).ToList();
        }

        /// <summary>
        /// Add one user.
        /// </summary>
        /// <param name="user">User which will be included</param>
        /// <returns>The same user plus his <see cref="User.Id"/>.</returns>.
        public User Add(User user)
        {
            user.Id = db.Query<int>(InsertSql, user).Single();
            return user;
        }

        /// <summary>
        /// Update one user.
        /// </summary>
        /// <param name="user">User which will be updated</param>
        /// <returns>Echo the User.</returns>
        public User Update(User user)
        {
            db.Execute(UpdateSql, user);
            return user;
        }

        /// <summary>
        /// check if given UserName and Password are found in the database.
        /// </summary>
        /// <param name="user">User, with user name and password filled.</param>
        /// <returns>true is the user exists</returns>
        public bool CheckUserPassword(User user)
        {
            var found = FindByUserNameAndPassword(user);

            return found != null;
        }

        /// <summary>
        /// Find user by his UserName and password inside <param name="user">User Object</param>.
        /// </summary>
        /// <returns>User found otherwise null. </returns>
        public User FindByUserNameAndPassword(User user)
        {
            var where = " WHERE USER_NAME = @UserName AND SENHA = @Password";
            return db.Query<User>(DefaultSql + where, user).SingleOrDefault();
        }
    }
}