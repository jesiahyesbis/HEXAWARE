using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.DAO.Interfaces
{
    public interface IArtistService
    {
        bool AddArtist(Artist artist);
        bool UpdateArtist(Artist artist);
        bool RemoveArtist(int artistId);
        Artist GetArtistById(int artistId);
        List<Artist> SearchArtists(string keyword);
        List<Artist> ListAllArtists();
    }
}
