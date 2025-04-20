using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.Main
{
    public class GalleryManagementUI
    {

        private readonly IGalleryService gallery_Service;

        public GalleryManagementUI(IGalleryService galleryService)
        {
            gallery_Service = galleryService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("GALLERY MANAGEMENT");
                Console.WriteLine("------------------");
                Console.WriteLine("1. Add Gallery");
                Console.WriteLine("2. Update Gallery");
                Console.WriteLine("3. Remove Gallery");
                Console.WriteLine("4. View Gallery Details");
                Console.WriteLine("5. Search Galleries");
                Console.WriteLine("6. List All Galleries");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddGallery();
                        break;
                    case "2":
                        UpdateGallery();
                        break;
                    case "3":
                        RemoveGallery();
                        break;
                    case "4":
                        ViewGallery();
                        break;
                    case "5":
                        SearchGalleries();
                        break;
                    case "6":
                        ListAllGalleries();
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

        private void AddGallery()
        {
            Console.Clear();
            Console.WriteLine("ADD NEW GALLERY");
            Console.WriteLine("---------------");

            try
            {
                Gallery gallery = new Gallery();
                Console.Write("Gallery ID: ");
                gallery.GalleryID = int.Parse(Console.ReadLine());
                Console.Write("Name: ");
                gallery.Name = Console.ReadLine();

                Console.Write("Description (optional): ");
                gallery.Description = Console.ReadLine();

                Console.Write("Location: ");
                gallery.Location = Console.ReadLine();

                Console.Write("Curator ID: ");
                gallery.Curator = int.Parse(Console.ReadLine());

                Console.Write("Opening Hours (optional): ");
                gallery.OpeningHours = Console.ReadLine();

                bool success = gallery_Service.AddGallery(gallery);
                Console.WriteLine(success ? "Gallery added successfully!" : "Failed to add gallery.");
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 547) // Foreign key violation
            {
                Console.WriteLine("Error: The specified Curator ID does not exist. Please provide a valid Artist ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void UpdateGallery()
        {
            Console.Clear();
            Console.WriteLine("UPDATE GALLERY");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter Gallery ID to update: ");
                int galleryId = int.Parse(Console.ReadLine());

                Gallery existing = gallery_Service.GetGalleryById(galleryId);
                Gallery gallery = new Gallery
                {
                    GalleryID = galleryId,
                    Name = existing.Name,
                    Description = existing.Description,
                    Location = existing.Location,
                    Curator = existing.Curator,
                    OpeningHours = existing.OpeningHours
                };

                Console.Write($"Name ({gallery.Name}): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) gallery.Name = input;

                Console.Write($"Description ({gallery.Description}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) gallery.Description = input;

                Console.Write($"Location ({gallery.Location}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) gallery.Location = input;

                Console.Write($"Curator ID ({gallery.Curator}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) gallery.Curator = int.Parse(input);

                Console.Write($"Opening Hours ({gallery.OpeningHours}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) gallery.OpeningHours = input;

                bool success = gallery_Service.UpdateGallery(gallery);
                Console.WriteLine(success ? "Gallery updated successfully!" : "Failed to update gallery.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void RemoveGallery()
        {
            Console.Clear();
            Console.WriteLine("REMOVE GALLERY");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter Gallery ID to remove: ");
                int galleryId = int.Parse(Console.ReadLine());

                bool success = gallery_Service.RemoveGallery(galleryId);
                Console.WriteLine(success ? "Gallery removed successfully!" : "Failed to remove gallery.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewGallery()
        {
            Console.Clear();
            Console.WriteLine("VIEW GALLERY DETAILS");
            Console.WriteLine("--------------------");

            try
            {
                Console.Write("Enter Gallery ID: ");
                int galleryId = int.Parse(Console.ReadLine());

                Gallery gallery = gallery_Service.GetGalleryById(galleryId);
                DisplayGallery(gallery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void SearchGalleries()
        {
            Console.Clear();
            Console.WriteLine("SEARCH GALLERIES");
            Console.WriteLine("----------------");

            try
            {
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                List<Gallery> galleries = gallery_Service.SearchGalleries(keyword);
                Console.WriteLine($"Found {galleries.Count} galleries:");
                foreach (var gallery in galleries)
                {
                    DisplayGallery(gallery);
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ListAllGalleries()
        {
            Console.Clear();
            Console.WriteLine("LIST ALL GALLERIES");
            Console.WriteLine("-----------------");

            try
            {
                List<Gallery> galleries = gallery_Service.ListAllGalleries();
                Console.WriteLine($"Total galleries: {galleries.Count}");
                foreach (var gallery in galleries)
                {
                    Console.WriteLine($"ID: {gallery.GalleryID}");
                    Console.WriteLine($"Name: {gallery.Name}");
                    Console.WriteLine($"Location: {gallery.Location}");
                    Console.WriteLine($"Curator ID: {gallery.Curator}");
                    Console.WriteLine($"Opening Hours: {gallery.OpeningHours ?? "N/A"}");
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DisplayGallery(Gallery gallery)
        {
            Console.WriteLine($"ID: {gallery.GalleryID}");
            Console.WriteLine($"Name: {gallery.Name}");
            Console.WriteLine($"Description: {gallery.Description ?? "N/A"}");
            Console.WriteLine($"Location: {gallery.Location}");
            Console.WriteLine($"Curator ID: {gallery.Curator}");
            Console.WriteLine($"Opening Hours: {gallery.OpeningHours ?? "N/A"}");
        }

    }
}
