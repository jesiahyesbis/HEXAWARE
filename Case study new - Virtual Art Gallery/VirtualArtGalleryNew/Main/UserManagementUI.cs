using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;

namespace VirtualArtGalleryNew.Main
{
    public class UserManagementUI
    {
        private readonly IUserService user_Service;

        public UserManagementUI(IUserService userService)
        {
            user_Service = userService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("USER MANAGEMENT");
                Console.WriteLine("---------------");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Update User");
                Console.WriteLine("3. Remove User");
                Console.WriteLine("4. View User Details By ID");
                Console.WriteLine("5. Search Users");
                Console.WriteLine("6. List All Users");
                Console.WriteLine("7. Back to Main Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateUser();
                        break;
                    case "2":
                        UpdateUser();
                        break;
                    case "3":
                        RemoveUser();
                        break;
                    case "4":
                        ViewUser();
                        break;
                    case "5":
                        SearchUsers();
                        break;
                    case "6":
                        ListAllUsers();
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

        private void CreateUser()
        {
            Console.Clear();
            Console.WriteLine("CREATE NEW USER");
            Console.WriteLine("---------------");

            try
            {
                User user = new User();

                Console.Write("Username: ");
                user.Username = Console.ReadLine();

                Console.Write("Password: ");
                user.Password = Console.ReadLine();

                Console.Write("Email: ");
                user.Email = Console.ReadLine();

                Console.Write("First Name (optional): ");
                user.FirstName = Console.ReadLine();

                Console.Write("Last Name (optional): ");
                user.LastName = Console.ReadLine();

                Console.Write("Date of Birth (yyyy-mm-dd): ");
                user.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Profile Picture URL (optional): ");
                user.ProfilePicture = Console.ReadLine();

                bool success = user_Service.CreateUser(user);
                Console.WriteLine(success ? "User created successfully!" : "Failed to create user.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("UPDATE USER");
            Console.WriteLine("-----------");

            try
            {
                Console.Write("Enter User ID to update: ");
                int userId = int.Parse(Console.ReadLine());

                User existing = user_Service.GetUserById(userId);
                User user = new User
                {
                    UserID = userId,
                    Username = existing.Username,
                    Password = existing.Password,
                    Email = existing.Email,
                    FirstName = existing.FirstName,
                    LastName = existing.LastName,
                    DateOfBirth = existing.DateOfBirth,
                    ProfilePicture = existing.ProfilePicture
                };

                Console.Write($"Username ({user.Username}): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.Username = input;

                Console.Write($"Password ({user.Password}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.Password = input;

                Console.Write($"Email ({user.Email}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.Email = input;

                Console.Write($"First Name ({user.FirstName}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.FirstName = input;

                Console.Write($"Last Name ({user.LastName}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.LastName = input;

                Console.Write($"Date of Birth ({user.DateOfBirth:yyyy-MM-dd}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.DateOfBirth = DateTime.Parse(input);

                Console.Write($"Profile Picture URL ({user.ProfilePicture}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) user.ProfilePicture = input;

                bool success = user_Service.UpdateUser(user);
                Console.WriteLine(success ? "User updated successfully!" : "Failed to update user.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void RemoveUser()
        {
            Console.Clear();
            Console.WriteLine("REMOVE USER");
            Console.WriteLine("-----------");

            try
            {
                Console.Write("Enter User ID to remove: ");
                int userId = int.Parse(Console.ReadLine());

                bool success = user_Service.RemoveUser(userId);
                Console.WriteLine(success ? "User removed successfully!" : "Failed to remove user.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ViewUser()
        {
            Console.Clear();
            Console.WriteLine("VIEW USER DETAILS");
            Console.WriteLine("-----------------");

            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                User user = user_Service.GetUserById(userId);
                DisplayUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void SearchUsers()
        {
            Console.Clear();
            Console.WriteLine("SEARCH USERS");
            Console.WriteLine("------------");

            try
            {
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                List<User> users = user_Service.SearchUsers(keyword);
                Console.WriteLine($"Found {users.Count} users:");
                foreach (var user in users)
                {
                    DisplayUser(user);
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void ListAllUsers()
        {
            Console.Clear();
            Console.WriteLine("LIST ALL USERS");
            Console.WriteLine("--------------");

            try
            {
                List<User> users = user_Service.ListAllUsers();
                Console.WriteLine($"Total users: {users.Count}");
                foreach (var user in users)
                {
                    DisplayUser(user);
                    Console.WriteLine("---------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DisplayUser(User user)
        {
            Console.WriteLine($"ID: {user.UserID}");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Password: [PROTECTED]");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"First Name: {user.FirstName ?? "N/A"}");
            Console.WriteLine($"Last Name: {user.LastName ?? "N/A"}");
            Console.WriteLine($"Date of Birth: {user.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Profile Picture: {user.ProfilePicture ?? "N/A"}");
        }
    }
}
