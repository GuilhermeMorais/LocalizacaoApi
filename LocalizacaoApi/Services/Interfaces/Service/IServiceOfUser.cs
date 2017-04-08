using Services.Objects;

namespace Services.Interfaces.Service
{
    /// <summary>
    /// Service of User.
    /// </summary>
    public interface IServiceOfUser
    {
        /// <summary>
        /// Get the Id by <param name="user">User Name</param> and his Password.
        /// </summary>
        /// <returns>Return the Id of the <see cref="User"/> or -1 if not found.</returns>
        User Verify(User user);

        /// <summary>
        /// Find the <see cref="User"/> by his <param name="id">Id</param>.
        /// </summary>
        /// <returns>The user if found.</returns>
        User Find(int id);
    }
}