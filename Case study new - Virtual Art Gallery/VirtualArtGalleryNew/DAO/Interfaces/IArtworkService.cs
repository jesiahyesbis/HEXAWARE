using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.DAO.Interfaces
{
    public interface IArtworkService
    {
        bool AddArtwork(Artwork artwork);
        bool UpdateArtwork(Artwork artwork);
        bool RemoveArtwork(int artworkID);
        Artwork GetArtwork(int artworkID);
        List<Artwork> SearchArtwork(string keyword);
        List<Artwork> ListAllArtworks();

    }
}
