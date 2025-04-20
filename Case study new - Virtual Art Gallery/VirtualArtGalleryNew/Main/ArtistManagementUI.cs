using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.Main
{
    public class ArtistManagementUI
    {
        private readonly IArtistService artist_Service;

        public ArtistManagementUI(IArtistService artistService)
        {
            artist_Service = artistService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ARTIST MANAGEMENT");
                Console.WriteLine("-----------------");
                Console.WriteLine("1. Add Artist");
                Console.WriteLine("2. Update Artist");
                Console.WriteLine("3. Remove Artist");
                Console.WriteLine("4. View Artist Details");
                Console.WriteLine("5. Search Artists");
                Console.WriteLine("6. List All Artists");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddArtist();
                        break;
                    case "2":
                        UpdateArtist();
                        break;
                    case "3":
                        RemoveArtist();
                        break;
                    case "4":
                        ViewArtist();
                        break;
                    case "5":
                        SearchArtists();
                        break;
                    case "6":
                        ListAllArtists();
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

        private void AddArtist()
        {
            Console.Clear();
            Console.WriteLine("ADD NEW ARTIST");
            Console.WriteLine("--------------");

            try
            {
                Artist artist = new Artist();

                Console.Write("Artist ID: ");
                artist.ArtistID = int.Parse(Console.ReadLine());

                Console.Write("Name: ");
                artist.Name = Console.ReadLine();

                Console.Write("Biography (optional): ");
                artist.Biography = Console.ReadLine();

                Console.Write("Birth Date (yyyy-mm-dd): ");
                artist.BirthDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Nationality: ");
                artist.Nationality = Console.ReadLine();

                Console.Write("Website (optional): ");
                artist.Website = Console.ReadLine();

                Console.Write("Contact Information: ");
                artist.ContactInformation = Console.ReadLine();

                bool success = artist_Service.AddArtist(artist);
                Console.WriteLine(success ? "Artist added successfully!" : "Failed to add artist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void UpdateArtist()
        {
            Console.Clear();
            Console.WriteLine("UPDATE ARTIST");
            Console.WriteLine("-------------");

            try
            {
                Console.Write("Enter Artist ID to update: ");
                int artistId = int.Parse(Console.ReadLine());

                Artist existing = artist_Service.GetArtistById(artistId);
                Artist artist = new Artist
                {
                    ArtistID = artistId,
                    Name = existing.Name,
                    Biography = existing.Biography,
                    BirthDate = existing.BirthDate,
                    Nationality = existing.Nationality,
                    Website = existing.Website,
                    ContactInformation = existing.ContactInformation
                };

                Console.Write($"Name ({artist.Name}): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.Name = input;

                Console.Write($"Biography ({artist.Biography}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.Biography = input;

                Console.Write($"Birth Date ({artist.BirthDate:yyyy-MM-dd}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.BirthDate = DateTime.Parse(input);

                Console.Write($"Nationality ({artist.Nationality}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.Nationality = input;

                Console.Write($"Website ({artist.Website}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.Website = input;

                Console.Write($"Contact Information ({artist.ContactInformation}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) artist.ContactInformation = input;

                bool success = artist_Service.UpdateArtist(artist);
                Console.WriteLine(success ? "Artist updated successfully!" : "Failed to update artist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void RemoveArtist()
        {
            Console.Clear();
            Console.WriteLine("REMOVE ARTIST");
            Console.WriteLine("-------------");

            try
            {
                Console.Write("Enter Artist ID to remove: ");
                int artistId = int.Parse(Console.ReadLine());

                bool success = artist_Service.RemoveArtist(artistId);
                Console.WriteLine(success ? "Artist removed successfully!" : "Failed to remove artist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewArtist()
        {
            Console.Clear();
            Console.WriteLine("VIEW ARTIST DETAILS");
            Console.WriteLine("-------------------");

            try
            {
                Console.Write("Enter Artist ID: ");
                int artistId = int.Parse(Console.ReadLine());

                Artist artist = artist_Service.GetArtistById(artistId);
                DisplayArtist(artist);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void SearchArtists()
        {
            Console.Clear();
            Console.WriteLine("SEARCH ARTISTS");
            Console.WriteLine("--------------");

            try
            {
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                List<Artist> artists = artist_Service.SearchArtists(keyword);
                Console.WriteLine($"Found {artists.Count} artists:");
                foreach (var artist in artists)
                {
                    DisplayArtist(artist);
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ListAllArtists()
        {
            Console.Clear();
            Console.WriteLine("LIST ALL ARTISTS");
            Console.WriteLine("----------------");

            try
            {
                List<Artist> artists = artist_Service.ListAllArtists();
                Console.WriteLine($"Total artists: {artists.Count}");
                foreach (var artist in artists)
                {
                    Console.WriteLine($"ID: {artist.ArtistID}");
                    Console.WriteLine($"Name: {artist.Name}");
                    Console.WriteLine($"Nationality: {artist.Nationality}");
                    Console.WriteLine($"Birth Date: {artist.BirthDate:yyyy-MM-dd}");
                    Console.WriteLine($"Website: {artist.Website ?? "N/A"}");
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }
        private void DisplayArtist(Artist artist)
        {
            Console.WriteLine($"ID: {artist.ArtistID}");
            Console.WriteLine($"Name: {artist.Name}");
            Console.WriteLine($"Biography: {artist.Biography ?? "N/A"}");
            Console.WriteLine($"Birth Date: {artist.BirthDate:yyyy-MM-dd}");
            Console.WriteLine($"Nationality: {artist.Nationality}");
            Console.WriteLine($"Website: {artist.Website ?? "N/A"}");
            Console.WriteLine($"Contact Information: {artist.ContactInformation ?? "N/A"}");
        }

    }
}
