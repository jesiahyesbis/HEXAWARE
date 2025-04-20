using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.DAO.Interfaces
{
    public interface IGalleryService
    {
        bool AddGallery(Gallery gallery);
        bool UpdateGallery(Gallery gallery);
        bool RemoveGallery(int galleryID);
        Gallery GetGalleryById(int galleryID);
        List<Gallery> SearchGalleries(string keyword);
        List<Gallery> ListAllGalleries();
    }
}
