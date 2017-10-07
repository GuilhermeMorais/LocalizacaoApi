using System.Collections.Generic;
using Services.Objects;

namespace Services.Interfaces.Repository
{
    /// <summary>
    /// Repository of Places.
    /// </summary>
    public interface IPlaceRepository
    {
        /// <summary>
        /// Find place by his Id.
        /// </summary>
        /// <param name="id"> Identification of the place  </param>
        /// <returns> Place found otherwise null. </returns>
        Place Find(int id);


        /// <summary>
        /// Get all Places on the base.
        /// </summary>
        /// <returns>List of every <see cref="Place"/>construction sites</returns> found.
        List<Place> GetAll();
    }
}