using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGallery.dao;
using VirtualArtGallery.entity;
using VirtualArtGallery.exception;
using VirtualArtGallery.util;

namespace VirtualArtGallery.main
{
    public class MainModule
    {
        private static IVirtualArtGallery galleryService = new VirtualArtGalleryImpl();

        public static void Main(string[] args)
        {
            try
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nVirtual Art Gallery System");
                    Console.WriteLine("1. Artwork Management");
                    Console.WriteLine("2. User Favorites");
                    Console.WriteLine("3. View Artists");
                    //Console.WriteLine("4. View Galleries");
                    Console.WriteLine("4. Gallery Management");

                    Console.WriteLine("5. Exit");
                    Console.Write("Enter your choice: ");

                    if (int.TryParse(Console.ReadLine(), out int mainChoice))
                    {
                        switch (mainChoice)
                        {
                            case 1:
                                ArtworkManagementMenu();
                                break;
                            case 2:
                                UserFavoritesMenu();
                                break;
                            case 3:
                                ViewArtists();
                                break;
                            //case 4:
                            //    ViewGalleries();
                            //    break;

                            case 4:  // New case
                                GalleryManagementMenu();
                                break;

                            case 5:
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        //*********************

        private static void GalleryManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== Gallery Management ===");
                Console.WriteLine("1. Add New Gallery");
                Console.WriteLine("2. Update Gallery");
                Console.WriteLine("3. Remove Gallery");
                Console.WriteLine("4. View All Galleries");
                Console.WriteLine("5. Search Galleries");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddGallery();
                                break;
                            case 2:
                                UpdateGallery();
                                break;
                            case 3:
                                RemoveGallery();
                                break;
                            case 4:
                                ViewAllGalleries();
                                break;
                            case 5:
                                SearchGalleries();
                                break;
                            case 6:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        private static void AddGallery()
        {
            Console.WriteLine("\nAdd New Gallery");

            Console.Write("Gallery Name: ");
            string name = Console.ReadLine();

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Location (optional): ");
            string location = Console.ReadLine();

            Console.Write("Curator (Artist ID): ");
            int curator = int.Parse(Console.ReadLine());

            Console.Write("Opening Hours (optional): ");
            string hours = Console.ReadLine();

            Gallery gallery = new Gallery
            {
                Name = name,
                Description = description,
                Location = location,
                Curator = curator,
                OpeningHours = hours
            };

            bool success = galleryService.AddGallery(gallery);
            Console.WriteLine(success ? "Gallery added successfully!" : "Failed to add gallery.");
        }



        //private static void UpdateGallery()
        //{
        //    Console.WriteLine("\nUpdate Gallery");
        //    Console.Write("Enter Gallery ID to update: ");
        //    int galleryId = int.Parse(Console.ReadLine());

        //    Gallery existing = galleryService.GetAllGalleries().FirstOrDefault(g => g.GalleryID == galleryId);
        //    if (existing == null)
        //    {
        //        Console.WriteLine("Gallery not found!");
        //        return;
        //    }

        //    Console.WriteLine($"Current Name: {existing.Name}");
        //    Console.Write("New Name (leave blank to keep current): ");
        //    string name = Console.ReadLine();

        //    // Repeat for other fields (description, location, etc.)

        //    Gallery updated = new Gallery
        //    {
        //        GalleryID = galleryId,
        //        Name = string.IsNullOrEmpty(name) ? existing.Name : name,
        //        // Update other fields similarly
        //    };

        //    bool success = galleryService.UpdateGallery(updated);
        //    Console.WriteLine(success ? "Gallery updated successfully!" : "Failed to update gallery.");
        //}


        private static void UpdateGallery()
        {
            Console.WriteLine("\n=== Update Gallery ===");

            // Get gallery ID
            Console.Write("Enter Gallery ID to update: ");
            int galleryId = int.Parse(Console.ReadLine());

            // Fetch existing gallery
            Gallery existing = galleryService.GetAllGalleries()
                .FirstOrDefault(g => g.GalleryID == galleryId);

            if (existing == null)
            {
                Console.WriteLine("Error: Gallery not found!");
                return;
            }

            // Display current values and get updates
            Console.WriteLine("\nCurrent Values:");
            Console.WriteLine($"1. Name: {existing.Name}");
            Console.WriteLine($"2. Description: {existing.Description ?? "[null]"}");
            Console.WriteLine($"3. Location: {existing.Location ?? "[null]"}");
            Console.WriteLine($"4. Curator (ArtistID): {existing.Curator}");
            Console.WriteLine($"5. Opening Hours: {existing.OpeningHours ?? "[null]"}");

            Console.WriteLine("\nEnter new values (press Enter to keep current):");

            // Name
            Console.Write("New Name: ");
            string name = Console.ReadLine();

            // Description
            Console.Write("New Description: ");
            string description = Console.ReadLine();

            // Location
            Console.Write("New Location: ");
            string location = Console.ReadLine();

            // Curator
            Console.Write("New Curator (ArtistID): ");
            string curatorInput = Console.ReadLine();

            // Opening Hours
            Console.Write("New Opening Hours: ");
            string hours = Console.ReadLine();

            // Build updated gallery
            Gallery updated = new Gallery
            {
                GalleryID = galleryId,
                Name = string.IsNullOrWhiteSpace(name) ? existing.Name : name,
                Description = string.IsNullOrWhiteSpace(description) ? existing.Description : description,
                Location = string.IsNullOrWhiteSpace(location) ? existing.Location : location,
                Curator = string.IsNullOrWhiteSpace(curatorInput) ? existing.Curator : int.Parse(curatorInput),
                OpeningHours = string.IsNullOrWhiteSpace(hours) ? existing.OpeningHours : hours
            };

            // Save changes
            bool success = galleryService.UpdateGallery(updated);
            Console.WriteLine(success ? "\nGallery updated successfully!" : "\nFailed to update gallery.");
            Console.WriteLine("\nUpdated Gallery Details:");
            Console.WriteLine($"Name: {updated.Name}");
            Console.WriteLine($"Description: {updated.Description ?? "[null]"}");
            Console.WriteLine($"Location: {updated.Location ?? "[null]"}");
            Console.WriteLine($"Curator: {updated.Curator}");
            Console.WriteLine($"Hours: {updated.OpeningHours ?? "[null]"}");
        }



        private static void RemoveGallery()
        {
            Console.WriteLine("\nRemove Gallery");
            Console.Write("Enter Gallery ID to remove: ");
            int galleryId = int.Parse(Console.ReadLine());

            bool success = galleryService.RemoveGallery(galleryId);
            Console.WriteLine(success ? "Gallery removed successfully!" : "Failed to remove gallery.");
        }




        private static void ViewAllGalleries()
        {
            var galleries = galleryService.GetAllGalleries();
            Console.WriteLine("\nAll Galleries:");
            foreach (var g in galleries)
            {
                Console.WriteLine($"[{g.GalleryID}] {g.Name} (Curator: {g.Curator})");
                Console.WriteLine($"   Location: {g.Location} | Hours: {g.OpeningHours}");
            }
        }




        private static void SearchGalleries()
        {
            Console.WriteLine("\nSearch Galleries");
            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine();

            var results = galleryService.SearchGalleries(keyword);
            if (results.Count == 0)
            {
                Console.WriteLine("No galleries found matching your search.");
            }
            else
            {
                Console.WriteLine("Search Results:");
                foreach (var g in results)
                {
                    Console.WriteLine($"[{g.GalleryID}] {g.Name}");
                }
            }
        }
        //***************************







        private static void ArtworkManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nArtwork Management");
                Console.WriteLine("1. Add Artwork");
                Console.WriteLine("2. Update Artwork");
                Console.WriteLine("3. Remove Artwork");
                Console.WriteLine("4. View Artwork by ID");
                Console.WriteLine("5. Search Artworks");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddArtwork();
                                break;
                            case 2:
                                UpdateArtwork();
                                break;
                            case 3:
                                RemoveArtwork();
                                break;
                            case 4:
                                ViewArtworkById();
                                break;
                            case 5:
                                SearchArtworks();
                                break;
                            case 6:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (ArtWorkNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private static void AddArtwork()
        {
            Console.WriteLine("\nAdd New Artwork");

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Creation Date (YYYY-MM-DD): ");
            DateTime creationDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Medium (optional): ");
            string medium = Console.ReadLine();

            Console.Write("Image URL (optional): ");
            string imageUrl = Console.ReadLine();

            Console.Write("Artist ID: ");
            int artistId = int.Parse(Console.ReadLine());

            Artwork artwork = new Artwork
            {
                Title = title,
                Description = description,
                CreationDate = creationDate,
                Medium = medium,
                ImageURL = imageUrl,
                ArtistID = artistId
            };

            bool success = galleryService.AddArtwork(artwork);
            if (success)
            {
                Console.WriteLine("Artwork added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add artwork.");
            }
        }

        private static void UpdateArtwork()
        {
            Console.WriteLine("\nUpdate Artwork");

            Console.Write("Enter Artwork ID to update: ");
            int artworkId = int.Parse(Console.ReadLine());

            // Get existing artwork to show current values
            Artwork existingArtwork = galleryService.GetArtworkById(artworkId);
            Console.WriteLine($"Current Title: {existingArtwork.Title}");
            Console.WriteLine($"Current Description: {existingArtwork.Description}");
            Console.WriteLine($"Current Creation Date: {existingArtwork.CreationDate.ToShortDateString()}");
            Console.WriteLine($"Current Medium: {existingArtwork.Medium}");
            Console.WriteLine($"Current Image URL: {existingArtwork.ImageURL}");
            Console.WriteLine($"Current Artist ID: {existingArtwork.ArtistID}");

            Console.Write("New Title (leave blank to keep current): ");
            string title = Console.ReadLine();

            Console.Write("New Description (leave blank to keep current): ");
            string description = Console.ReadLine();

            Console.Write("New Creation Date (YYYY-MM-DD, leave blank to keep current): ");
            string creationDateStr = Console.ReadLine();

            Console.Write("New Medium (leave blank to keep current): ");
            string medium = Console.ReadLine();

            Console.Write("New Image URL (leave blank to keep current): ");
            string imageUrl = Console.ReadLine();

            Console.Write("New Artist ID (leave blank to keep current): ");
            string artistIdStr = Console.ReadLine();

            // Update only fields that were provided
            Artwork updatedArtwork = new Artwork
            {
                ArtworkID = artworkId,
                Title = string.IsNullOrEmpty(title) ? existingArtwork.Title : title,
                Description = string.IsNullOrEmpty(description) ? existingArtwork.Description : description,
                CreationDate = string.IsNullOrEmpty(creationDateStr) ? existingArtwork.CreationDate : DateTime.Parse(creationDateStr),
                Medium = string.IsNullOrEmpty(medium) ? existingArtwork.Medium : medium,
                ImageURL = string.IsNullOrEmpty(imageUrl) ? existingArtwork.ImageURL : imageUrl,
                ArtistID = string.IsNullOrEmpty(artistIdStr) ? existingArtwork.ArtistID : int.Parse(artistIdStr)
            };

            bool success = galleryService.UpdateArtwork(updatedArtwork);
            if (success)
            {
                Console.WriteLine("Artwork updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update artwork.");
            }
        }

        private static void RemoveArtwork()
        {
            Console.WriteLine("\nRemove Artwork");

            Console.Write("Enter Artwork ID to remove: ");
            int artworkId = int.Parse(Console.ReadLine());

            bool success = galleryService.RemoveArtwork(artworkId);
            if (success)
            {
                Console.WriteLine("Artwork removed successfully!");
            }
            else
            {
                Console.WriteLine("Failed to remove artwork.");
            }
        }

        private static void ViewArtworkById()
        {
            Console.WriteLine("\nView Artwork by ID");

            Console.Write("Enter Artwork ID: ");
            int artworkId = int.Parse(Console.ReadLine());

            Artwork artwork = galleryService.GetArtworkById(artworkId);
            Console.WriteLine("\nArtwork Details:");
            Console.WriteLine($"ID: {artwork.ArtworkID}");
            Console.WriteLine($"Title: {artwork.Title}");
            Console.WriteLine($"Description: {artwork.Description}");
            Console.WriteLine($"Creation Date: {artwork.CreationDate.ToShortDateString()}");
            Console.WriteLine($"Medium: {artwork.Medium}");
            Console.WriteLine($"Image URL: {artwork.ImageURL}");
            Console.WriteLine($"Artist ID: {artwork.ArtistID}");
        }

        private static void SearchArtworks()
        {
            Console.WriteLine("\nSearch Artworks");

            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine();

            List<Artwork> artworks = galleryService.SearchArtworks(keyword);

            if (artworks.Count == 0)
            {
                Console.WriteLine("No artworks found matching your search.");
            }
            else
            {
                Console.WriteLine("\nSearch Results:");
                foreach (var artwork in artworks)
                {
                    Console.WriteLine($"ID: {artwork.ArtworkID}, Title: {artwork.Title}, Artist ID: {artwork.ArtistID}");
                }
            }
        }

        private static void UserFavoritesMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nUser Favorites Management");
                Console.WriteLine("1. Add Artwork to Favorites");
                Console.WriteLine("2. Remove Artwork from Favorites");
                Console.WriteLine("3. View User's Favorite Artworks");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddArtworkToFavorites();
                                break;
                            case 2:
                                RemoveArtworkFromFavorites();
                                break;
                            case 3:
                                ViewUserFavorites();
                                break;
                            case 4:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (UserNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (ArtWorkNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private static void AddArtworkToFavorites()
        {
            Console.WriteLine("\nAdd Artwork to Favorites");

            Console.Write("User ID: ");
            int userId = int.Parse(Console.ReadLine());

            Console.Write("Artwork ID: ");
            int artworkId = int.Parse(Console.ReadLine());

            bool success = galleryService.AddArtworkToFavorite(userId, artworkId);
            if (success)
            {
                Console.WriteLine("Artwork added to favorites successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add artwork to favorites.");
            }
        }

        private static void RemoveArtworkFromFavorites()
        {
            Console.WriteLine("\nRemove Artwork from Favorites");

            Console.Write("User ID: ");
            int userId = int.Parse(Console.ReadLine());

            Console.Write("Artwork ID: ");
            int artworkId = int.Parse(Console.ReadLine());

            bool success = galleryService.RemoveArtworkFromFavorite(userId, artworkId);
            if (success)
            {
                Console.WriteLine("Artwork removed from favorites successfully!");
            }
            else
            {
                Console.WriteLine("Failed to remove artwork from favorites.");
            }
        }

        private static void ViewUserFavorites()
        {
            Console.WriteLine("\nView User's Favorite Artworks");

            Console.Write("User ID: ");
            int userId = int.Parse(Console.ReadLine());

            List<Artwork> favorites = galleryService.GetUserFavoriteArtworks(userId);

            if (favorites.Count == 0)
            {
                Console.WriteLine("No favorite artworks found for this user.");
            }
            else
            {
                Console.WriteLine("\nFavorite Artworks:");
                foreach (var artwork in favorites)
                {
                    Console.WriteLine($"ID: {artwork.ArtworkID}, Title: {artwork.Title}, Artist ID: {artwork.ArtistID}");
                }
            }
        }

        private static void ViewArtists()
        {
            Console.WriteLine("\nList of Artists");

            List<Artist> artists = galleryService.GetAllArtists();

            if (artists.Count == 0)
            {
                Console.WriteLine("No artists found.");
            }
            else
            {
                foreach (var artist in artists)
                {
                    Console.WriteLine($"ID: {artist.ArtistID}, Name: {artist.Name}, Nationality: {artist.Nationality}");
                }
            }
        }

        //private static void ViewGalleries()
        //{
        //    Console.WriteLine("\nList of Galleries");

        //    List<Gallery> galleries = galleryService.GetAllGalleries();

        //    if (galleries.Count == 0)
        //    {
        //        Console.WriteLine("No galleries found.");
        //    }
        //    else
        //    {
        //        foreach (var gallery in galleries)
        //        {
        //            Console.WriteLine($"ID: {gallery.GalleryID}, Name: {gallery.Name}, Location: {gallery.Location}");
        //        }
        //    }
        //}
    }
}
