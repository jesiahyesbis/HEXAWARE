using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Entities
{
     public class Gallery
     {
            public int GalleryID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public int Curator { get; set; }
            public string OpeningHours { get; set; }

            public Gallery() { }

            public Gallery(int galleryID, string name, string description, string location,
                           int curator, string openingHours)
            {
                GalleryID = galleryID;
                Name = name;
                Description = description;
                Location = location;
                Curator = curator;
                OpeningHours = openingHours;
            }
       
     }
}
