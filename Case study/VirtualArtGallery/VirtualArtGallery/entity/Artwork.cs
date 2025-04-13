using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.entity
{
    public class Artwork
    {
        public int ArtworkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Medium { get; set; }
        public string ImageURL { get; set; }
        public int ArtistID { get; set; }

        public Artwork() { }

        public Artwork(int artworkID, string title, string description, DateTime creationDate,
                      string medium, string imageURL, int artistID)
        {
            ArtworkID = artworkID;
            Title = title;
            Description = description;
            CreationDate = creationDate;
            Medium = medium;
            ImageURL = imageURL;
            ArtistID = artistID;
        }
    }
}
