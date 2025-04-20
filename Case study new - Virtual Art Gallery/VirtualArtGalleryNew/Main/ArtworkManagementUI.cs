using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.Main
{
    public class ArtworkManagementUI
    {

        private readonly IArtworkService artwork_Service;

        public ArtworkManagementUI(IArtworkService artworkService)
        {
            artwork_Service = artworkService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ARTWORK MANAGEMENT");
                Console.WriteLine("------------------");
                Console.WriteLine("1. Add Artwork");
                Console.WriteLine("2. Update Artwork");
                Console.WriteLine("3. Remove Artwork");
                Console.WriteLine("4. View Artwork Details");
                Console.WriteLine("5. Search Artworks");
                Console.WriteLine("6. List All Artworks");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddArtwork();
                        break;
                    case "2":
                        UpdateArtwork();
                        break;
                    case "3":
                        RemoveArtwork();
                        break;
                    case "4":
                        ViewArtwork();
                        break;
                    case "5":
                        SearchArtworks();
                        break;
                    case "6":
                        ListAllArtworks();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddArtwork()
        {
            Console.Clear();
            Console.WriteLine("ADD NEW ARTWORK");
            Console.WriteLine("---------------");

            try
            {
                Artwork artwork = new Artwork();
                Console.Write("Artwork ID: ");
                artwork.ArtworkID = int.Parse(Console.ReadLine());
                Console.Write("Title: ");
                artwork.Title = Console.ReadLine();
                Console.Write("Description (optional): ");
                artwork.Description = Console.ReadLine();
                Console.Write("Creation Date (yyyy-mm-dd): ");
                artwork.CreationDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Medium: ");
                artwork.Medium = Console.ReadLine();
                Console.Write("Image URL: ");
                artwork.ImageURL = Console.ReadLine();
                Console.Write("Artist ID: ");
                artwork.ArtistID = int.Parse(Console.ReadLine());
                bool success = artwork_Service.AddArtwork(artwork);
                Console.WriteLine(success ? "Artwork added successfully!" : "Failed to add artwork.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void UpdateArtwork()
        {
            Console.Clear();
            Console.WriteLine("UPDATE ARTWORK");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter Artwork ID to update: ");
                int artworkId = int.Parse(Console.ReadLine());

                Artwork existing = artwork_Service.GetArtwork(artworkId);
                Artwork artwork = new Artwork
                {
                    ArtworkID = artworkId,
                    Title = existing.Title,
                    Description = existing.Description,
                    CreationDate = existing.CreationDate,
                    Medium = existing.Medium,
                    ImageURL = existing.ImageURL,
                    ArtistID = existing.ArtistID
                };

                Console.Write($"Title ({artwork.Title}): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.Title = input;

                Console.Write($"Description ({artwork.Description}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.Description = input;

                Console.Write($"Creation Date ({artwork.CreationDate:yyyy-MM-dd}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.CreationDate = DateTime.Parse(input);

                Console.Write($"Medium ({artwork.Medium}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.Medium = input;

                Console.Write($"Image URL ({artwork.ImageURL}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.ImageURL = input;

                Console.Write($"Artist ID ({artwork.ArtistID}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artwork.ArtistID = int.Parse(input);

                bool success = artwork_Service.UpdateArtwork(artwork);
                Console.WriteLine(success ? "Artwork updated successfully!" : "Failed to update artwork.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void RemoveArtwork()
        {
            Console.Clear();
            Console.WriteLine("REMOVE ARTWORK");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter Artwork ID to remove: ");
                int artworkId = int.Parse(Console.ReadLine());

                bool success = artwork_Service.RemoveArtwork(artworkId);
                Console.WriteLine(success ? "Artwork removed successfully!" : "Failed to remove artwork.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewArtwork()
        {
            Console.Clear();
            Console.WriteLine("VIEW ARTWORK DETAILS");
            Console.WriteLine("--------------------");

            try
            {
                Console.Write("Enter Artwork ID: ");
                int artworkId = int.Parse(Console.ReadLine());

                Artwork artwork = artwork_Service.GetArtwork(artworkId);
                DisplayArtwork(artwork);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void SearchArtworks()
        {
            Console.Clear();
            Console.WriteLine("SEARCH ARTWORKS");
            Console.WriteLine("---------------");

            try
            {
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                List<Artwork> artworks = artwork_Service.SearchArtwork(keyword);
                Console.WriteLine($"Found {artworks.Count} artworks:");
                foreach (var artwork in artworks)
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

        private void ListAllArtworks()
        {
            Console.Clear();
            Console.WriteLine("LIST ALL ARTWORKS");
            Console.WriteLine("----------------");

            try
            {
                List<Artwork> artworks = artwork_Service.ListAllArtworks();
                Console.WriteLine($"Total artworks: {artworks.Count}");
                foreach (var artwork in artworks)
                {
                    Console.WriteLine($"ID: {artwork.ArtworkID}");
                    Console.WriteLine($"Title: {artwork.Title}");
                    Console.WriteLine($"Artist ID: {artwork.ArtistID}");
                    Console.WriteLine($"Medium: {artwork.Medium}");
                    Console.WriteLine($"Creation Date: {artwork.CreationDate:yyyy-MM-dd}");
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
