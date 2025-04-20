using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;
using VirtualArtGalleryNew.Exceptions;
using VirtualArtGalleryNew;

namespace VArtGalleryTestProject
{
    [TestFixture]
    public class GalleryManagementTests
    {
        private GalleryImpl galleryService;

        [SetUp]
        public void SetUp()
        {
            galleryService = new GalleryImpl();
        }

        [Test]
        public void AddGalleryShouldAddGalleryWhenValidData()
        {
            // Arrange
            var gallery = new Gallery(30, "Test Gallery1", "Test Description1", "Test Location1", 1, "9AM-5PM");

            // Act
            var result = galleryService.AddGallery(gallery);

            // Assert
            Assert.IsTrue(result);

            // Cleanup
            galleryService.RemoveGallery(gallery.GalleryID);
        }

        [Test]
        public void UpdateGalleryShouldUpdateFieldsWhenValidData()
        {
            // Arrange
            var gallery = new Gallery(29, "Initial Name", "Initial Description", "Initial Location", 1, "10AM-6PM");
            galleryService.AddGallery(gallery);

            gallery.Name = "Updated Name";
            gallery.Description = "Updated Description";
            gallery.Location = "Updated Location";

            // Act
            var result = galleryService.UpdateGallery(gallery);

            // Assert
            Assert.IsTrue(result);
            var updatedGallery = galleryService.GetGalleryById(29);
            Assert.AreEqual("Updated Name", updatedGallery.Name);

            // Cleanup
            galleryService.RemoveGallery(29);
        }

        [Test]
        public void RemoveGalleryShouldDeleteWhenGalleryExists()
        {
            // Arrange
            var gallery = new Gallery(28, "To Delete", "Desc", "Loc", 1, "11AM-4PM");
            galleryService.AddGallery(gallery);

            // Act
            var result = galleryService.RemoveGallery(28);

            // Assert
            Assert.IsTrue(result);
            Assert.Throws<GalleryNotFoundException>(() => galleryService.GetGalleryById(28));
        }

        [Test]
        public void SearchGalleriesShouldReturnResultsWhenKeywordMatches()
        {
            // Arrange
            var gallery = new Gallery(27, "Keyword Gallery", "Awesome art place", "New York", 1, "9AM-6PM");
            galleryService.AddGallery(gallery);

            // Act
            var results = galleryService.SearchGalleries("Keyword");

            // Assert
            Assert.IsTrue(results.Exists(g => g.GalleryID == 27));

            // Cleanup
            galleryService.RemoveGallery(27);
        }

        [Test]
        public void AddGalleryShouldThrowWhenDuplicateID()
        {
            // Arrange
            var gallery = new Gallery(26, "Duplicate Test", "Desc", "Loc", 1, "10AM-5PM");
            galleryService.AddGallery(gallery);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => galleryService.AddGallery(gallery));
            Assert.That(ex.Message, Does.Contain("already exists"));

            // Cleanup
            galleryService.RemoveGallery(26);
        }

        [Test]
        public void SearchGalleriesShouldThrowWhenKeywordIsNull()
        {
            Assert.Throws<ArgumentException>(() => galleryService.SearchGalleries(null));
        }


    }
}
