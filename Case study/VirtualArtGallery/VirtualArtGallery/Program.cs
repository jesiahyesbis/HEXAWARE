////// See https://aka.ms/new-console-template for more information
////Console.WriteLine("Hello, World!");



//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using VirtualArtGallery.dao;
//using VirtualArtGallery.entity;
//using VirtualArtGallery.exception;

//namespace VirtualArtGalleryTests
//{
//    [TestClass]
//    public class ArtworkTests
//    {
//        private readonly IVirtualArtGallery galleryService = new VirtualArtGalleryImpl();

//        [TestMethod]
//        public void AddArtwork_ShouldReturnTrue_WhenArtworkIsValid()
//        {
//            // Arrange
//            Artwork artwork = new Artwork
//            {
//                Title = "Test Artwork",
//                Description = "Test Description",
//                CreationDate = DateTime.Now,
//                Medium = "Oil on canvas",
//                ArtistID = 1 // Assuming artist with ID 1 exists
//            };

//            // Act
//            bool result = galleryService.AddArtwork(artwork);

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArtWorkNotFoundException))]
//        public void GetArtworkById_ShouldThrowException_WhenArtworkDoesNotExist()
//        {
//            // Act
//            galleryService.GetArtworkById(9999); // Assuming this ID doesn't exist
//        }

//        [TestMethod]
//        public void SearchArtworks_ShouldReturnResults_WhenKeywordMatches()
//        {
//            // Arrange
//            string keyword = "night"; // Should match "Starry Night" in sample data

//            // Act
//            var results = galleryService.SearchArtworks(keyword);

//            // Assert
//            Assert.IsTrue(results.Count > 0);
//        }

//        [TestMethod]
//        public void AddArtworkToFavorite_ShouldReturnTrue_WhenUserAndArtworkExist()
//        {
//            // Arrange
//            int userId = 1; // Assuming user with ID 1 exists
//            int artworkId = 1; // Assuming artwork with ID 1 exists

//            // Act
//            bool result = galleryService.AddArtworkToFavorite(userId, artworkId);

//            // Assert
//            Assert.IsTrue(result);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(UserNotFoundException))]
//        public void AddArtworkToFavorite_ShouldThrowException_WhenUserDoesNotExist()
//        {
//            // Arrange
//            int userId = 9999; // Assuming this user doesn't exist
//            int artworkId = 1;

//            // Act
//            galleryService.AddArtworkToFavorite(userId, artworkId);
//        }



//using System.Data.SqlClient;
//using VirtualArtGallery.entity;

//[TestMethod]
//public void AddGallery_ShouldReturnTrue_WhenGalleryIsValid()
//{
//    Gallery gallery = new Gallery
//    {
//        Name = "Test Gallery",
//        Curator = 1 // Valid ArtistID
//    };
//    bool result = galleryService.AddGallery(gallery);
//    Assert.IsTrue(result);
//}

//[TestMethod]
//public void SearchGalleries_ShouldReturnResults_WhenKeywordMatches()
//{
//    var results = galleryService.SearchGalleries("Modern");
//    Assert.IsTrue(results.Count > 0);
//}

//[TestMethod]
//[ExpectedException(typeof(SqlException))]
//public void AddGallery_ShouldFail_WhenCuratorDoesNotExist()
//{
//    Gallery gallery = new Gallery
//    {
//        Name = "Invalid Gallery",
//        Curator = 9999 // Invalid ArtistID
//    };
//    galleryService.AddGallery(gallery);
//}



//    }
//}