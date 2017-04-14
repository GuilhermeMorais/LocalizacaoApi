using Services.Interfaces.Repository;
using Services.Interfaces.Service;
using Services.Objects;
using Services.Repository;

namespace Services.Service
{
    /// <summary>
    /// Service of User.
    /// </summary>
    public class ServiceOfUser : IServiceOfUser
    {
        private readonly IUserRepository repository;

        /// <summary>
        /// Create Instance of the service.
        /// </summary>
        public ServiceOfUser()
        {
            repository = new UserRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ServiceOfUser(IUserRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Get the Id by User Name and his Password.
        /// </summary>
        /// <param name="user">User object filled with User Name and his Password</param>
        /// <returns>Return the Id of the <see cref="User"/> or -1 if not found.</returns>
        public User Verify(User user)
        {
            return repository.FindByUserNameAndPassword(user);
        }

        /// <summary>
        /// Find the <see cref="User"/> by his <param name="id">Id</param>.
        /// </summary>
        /// <returns>The user if found.</returns>
        public User Find(int id)
        {
            return repository.Find(id);
        }
    }
}