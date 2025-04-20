using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.DAO;
using VirtualArtGalleryNew.Entities;
using VirtualArtGalleryNew.Exceptions;
using VirtualArtGalleryNew;

namespace VArtGalleryTestProject
{
    [TestFixture]
    public class ArtworkManagementTests
    {
        private ArtworkImpl artworkService;

        [SetUp]
        public void Setup()
        {
            artworkService = new ArtworkImpl();
        }

        [Test]
        public void AddArtworkShouldAddArtworkWhenValidData()
        {
            var artwork = new Artwork
            {
                ArtworkID = 2001,
                Title = "The Persistence of Memory",
                Description = "A surreal painting by Salvador Dalí",
                CreationDate = new DateTime(1931, 4, 1),
                Medium = "Oil on canvas",
                ImageURL = "https://example.com/dali.jpg",
                ArtistID = 1
            };

            var result = artworkService.AddArtwork(artwork);
            Assert.IsTrue(result);

            // Cleanup
            artworkService.RemoveArtwork(artwork.ArtworkID);
        }

        [Test]
        public void UpdateArtworkShouldUpdateDetailsWhenArtworkExists()
        {
            var initialArtwork = new Artwork
            {
                ArtworkID = 2002,
                Title = "Original Title",
                Description = "Original Description",
                CreationDate = DateTime.Now.AddYears(-10),
                Medium = "Acrylic",
                ImageURL = "https://original-image.com/image.jpg",
                ArtistID = 1
            };

            artworkService.AddArtwork(initialArtwork);

            var updatedArtwork = new Artwork
            {
                ArtworkID = 2002,
                Title = "Updated Title",
                Description = "Updated Description",
                CreationDate = DateTime.Now,
                Medium = "Updated Medium",
                ImageURL = "https://updated-image.com/image.jpg",
                ArtistID = 1
            };

            var result = artworkService.UpdateArtwork(updatedArtwork);
            Assert.IsTrue(result);

            // Cleanup
            artworkService.RemoveArtwork(updatedArtwork.ArtworkID);
        }

        [Test]
        public void RemoveArtworkShouldDeleteArtworkWhenArtworkExists()
        {
            var artwork = new Artwork
            {
                ArtworkID = 2003,
                Title = "To Be Deleted",
                Description = "This will be removed",
                CreationDate = DateTime.Now,
                Medium = "Mixed media",
                ImageURL = "https://example.com/delete.jpg",
                ArtistID = 1
            };

            artworkService.AddArtwork(artwork);
            var result = artworkService.RemoveArtwork(artwork.ArtworkID);
            Assert.IsTrue(result);

        }

        [Test]
        public void SearchArtworkShouldReturnMatchingResultsWhenKeywordExists()
        {
            var artwork = new Artwork
            {
                ArtworkID = 2004,
                Title = "Sunset in Venice",
                Description = "A stunning sunset over canals",
                CreationDate = DateTime.Now,
                Medium = "Oil",
                ImageURL = "https://example.com/sunset.jpg",
                ArtistID = 1
            };

            artworkService.AddArtwork(artwork);

            string keyword = "Sunset";
            List<Artwork> results = artworkService.SearchArtwork(keyword);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Exists(a => a.Title.Contains("Sunset") || a.Description.Contains("Sunset")));

            // Cleanup
            artworkService.RemoveArtwork(artwork.ArtworkID);
        }

        [Test]
        public void AddArtworkShouldThrowExceptionWhenArtworkIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => artworkService.AddArtwork(null));
        }

        [Test]
        public void UpdateArtworkShouldThrowExceptionWhenArtworkDoesNotExist()
        {
            var artwork = new Artwork
            {
                ArtworkID = 9999,
                Title = "Ghost Artwork",
                Description = "Does not exist",
                CreationDate = DateTime.Now,
                Medium = "Ink",
                ImageURL = "https://example.com/ghost.jpg",
                ArtistID = 1
            };

            Assert.Throws<ArtworkNotFoundException>(() => artworkService.UpdateArtwork(artwork));
        }

        [Test]
        public void RemoveArtworkShouldThrowExceptionWhenArtworkNotFound()
        {
            Assert.Throws<ArtworkNotFoundException>(() => artworkService.RemoveArtwork(9999));
        }

        [Test]
        public void SearchArtworkShouldThrowExceptionWhenKeywordIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => artworkService.SearchArtwork(""));
        }
    }
}
