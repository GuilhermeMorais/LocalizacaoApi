using System.Collections.Generic;
using Services.Objects;

namespace Services.Interfaces.Repository
{
    /// <summary>
    /// Repository of Users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Find user by his <param name="id">Identification</param>.
        /// </summary>
        /// <returns>User found otherwise null. </returns>
        User Find(int id);
        
        /// <summary>
        /// Find user by his UserName and password inside <param name="user">User Object</param>.
        /// </summary>
        /// <returns>User found otherwise null. </returns>
        User FindByUserNameAndPassword(User user);

        /// <summary>
        /// Get all Users on the base.
        /// </summary>
        /// <returns>List of every <see cref="User"/>user</returns> found.
        List<User> GetAll();

        /// <summary>
        /// Add one user.
        /// </summary>
        /// <param name="user">User which will be included</param>
        /// <returns>The same user plus his <see cref="User.Id"/>.</returns>.
        User Add(User user);

        /// <summary>
        /// Update one user.
        /// </summary>
        /// <param name="user">User which will be updated</param>
        /// <returns>Echo the User.</returns>
        User Update(User user);

        /// <summary>
        /// check if given UserName and Password are found in the database.
        /// </summary>
        /// <param name="user">User, with user name and password filled.</param>
        /// <returns>true is the user exists</returns>
        bool CheckUserPassword(User user);
    }
}