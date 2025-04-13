using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGallery.entity;

namespace VirtualArtGallery.dao
{
    public interface IVirtualArtGallery
    {
        // Artwork Management
        bool AddArtwork(Artwork artwork);
        bool UpdateArtwork(Artwork artwork);
        bool RemoveArtwork(int artworkId);
        Artwork GetArtworkById(int artworkId);
        List<Artwork> SearchArtworks(string keyword);

        // User Favorites
        bool AddArtworkToFavorite(int userId, int artworkId);
        bool RemoveArtworkFromFavorite(int userId, int artworkId);
        List<Artwork> GetUserFavoriteArtworks(int userId);

        // Additional methods
        List<Artist> GetAllArtists();
        List<Gallery> GetAllGalleries();


        //*********************************
         // Gallery Management
        bool AddGallery(Gallery gallery);
        bool UpdateGallery(Gallery gallery);
        bool RemoveGallery(int galleryId);
        List<Gallery> SearchGalleries(string keyword);
         //************************************************



    }
}
