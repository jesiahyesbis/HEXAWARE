using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.Main
{
    public class UserFavoritesUI
    {

        private readonly IUserFavoriteService userFavorite_Service;

        public UserFavoritesUI(IUserFavoriteService userFavoriteService)
        {
            userFavorite_Service = userFavoriteService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("USER FAVORITES MANAGEMENT");
                Console.WriteLine("-------------------------");
                Console.WriteLine("1. Add Artwork to Favorites");
                Console.WriteLine("2. Remove Artwork from Favorites");
                Console.WriteLine("3. View User's Favorite Artworks");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddToFavorites();
                        break;
                    case "2":
                        RemoveFromFavorites();
                        break;
                    case "3":
                        ViewFavorites();
                        break; 
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddToFavorites()
        {
            Console.Clear();
            Console.WriteLine("ADD ARTWORK TO FAVORITES");
            Console.WriteLine("-------------------------");

            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                Console.Write("Enter Artwork ID: ");
                int artworkId = int.Parse(Console.ReadLine());

                bool success = userFavorite_Service.AddArtworkToFavorite(userId, artworkId);
                Console.WriteLine(success ? "Artwork added to favorites!" : "Failed to add to favorites.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void RemoveFromFavorites()
        {
            Console.Clear();
            Console.WriteLine("REMOVE ARTWORK FROM FAVORITES");
            Console.WriteLine("------------------------------");

            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                Console.Write("Enter Artwork ID: ");
                int artworkId = int.Parse(Console.ReadLine());

                bool success = userFavorite_Service.RemoveArtworkFromFavorite(userId, artworkId);
                Console.WriteLine(success ? "Artwork removed from favorites!" : "Failed to remove from favorites.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewFavorites()
        {
            Console.Clear();
            Console.WriteLine("VIEW USER'S FAVORITE ARTWORKS");
            Console.WriteLine("-----------------------------");

            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                List<Artwork> favorites = userFavorite_Service.GetUserFavoriteArtworks(userId);
                Console.WriteLine($"Found {favorites.Count} favorite artworks:");
                foreach (var artwork in favorites)
                {
                    DisplayArtwork(artwork);
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        
        private void DisplayArtwork(Artwork artwork)
        {
            Console.WriteLine($"ID: {artwork.ArtworkID}");
            Console.WriteLine($"Title: {artwork.Title}");
            Console.WriteLine($"Description: {artwork.Description ?? "N/A"}");
            Console.WriteLine($"Creation Date: {artwork.CreationDate:yyyy-MM-dd}");
            Console.WriteLine($"Medium: {artwork.Medium}");
            Console.WriteLine($"Image URL: {artwork.ImageURL}");
            Console.WriteLine($"Artist ID: {artwork.ArtistID}");
        }

    }
}
