using System;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.DAO;

namespace VirtualArtGalleryNew.Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            IUserService userService = new UserImpl();
            IArtworkService artworkService = new ArtworkImpl();
            IGalleryService galleryService = new GalleryImpl();
            IUserFavoriteService userFavoriteService = new UserFavoriteImpl();
            IArtistService artistService = new ArtistImpl();

            UserManagementUI userUI = new UserManagementUI(userService);
            ArtworkManagementUI artworkUI = new ArtworkManagementUI(artworkService);
            GalleryManagementUI galleryUI = new GalleryManagementUI(galleryService);
            UserFavoritesUI favoritesUI = new UserFavoritesUI(userFavoriteService);
            ArtistManagementUI artistUI = new ArtistManagementUI(artistService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("VIRTUAL ART GALLERY MANAGEMENT SYSTEM");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("1. Artist Management");
                Console.WriteLine("2. Artwork Management");
                Console.WriteLine("3. Gallery Management");
                Console.WriteLine("4. User Management");
                Console.WriteLine("5. User Favorites");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        artistUI.ShowMenu();
                        break;
                    case "2":
                        artworkUI.ShowMenu();
                        break;
                    case "3":
                        galleryUI.ShowMenu();
                        break;
                    case "4":
                        userUI.ShowMenu();
                        break;
                    case "5":
                        favoritesUI.ShowMenu();
                        break;
                    case "6":
                        Console.WriteLine("Thank you for using Virtual Art Gallery!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
    
}