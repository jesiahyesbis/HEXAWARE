using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;
using VirtualArtGalleryNew.Util;
using VirtualArtGalleryNew.Exceptions;
using System.Data.SqlClient;

namespace VirtualArtGalleryNew.DAO
{
    public class UserImpl:IUserService
    {
        public bool CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User cannot be null");
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required");
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email is required");
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"INSERT INTO [User] (Username, Password, Email, FirstName, LastName, DateOfBirth, ProfilePicture) 
                                VALUES (@Username, @Password, @Email, @FirstName, @LastName, @DateOfBirth, @ProfilePicture)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName",user.FirstName != null ? user.FirstName : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName",
                        user.LastName != null ? user.LastName : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture ?? (object)DBNull.Value);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
                    {
                        throw new DuplicateEntryException("Username or email already exists");
                    }
                }
            }
        }

        public bool UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User cannot be null");
            if (user.UserID <= 0)
                throw new ArgumentException("Valid User ID is required");
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required");
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"UPDATE [User] SET 
                                Username = @Username, 
                                Password = @Password, 
                                Email = @Email, 
                                FirstName = @FirstName, 
                                LastName = @LastName, 
                                DateOfBirth = @DateOfBirth, 
                                ProfilePicture = @ProfilePicture
                                WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.UserID);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName != null ? user.FirstName : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName",
                        user.LastName != null ? user.LastName : (object)DBNull.Value);

                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture ?? (object)DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new UserNotFoundException($"User with ID {user.UserID} not found");
                    return rowsAffected > 0;
                }
            }
        }

        public bool RemoveUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Valid User ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                // First remove favorites associations
                string removeFavorites = "DELETE FROM User_Favorite_Artwork WHERE UserID = @UserID";
                using (SqlCommand favCommand = new SqlCommand(removeFavorites, connection))
                {
                    favCommand.Parameters.AddWithValue("@UserID", userId);
                    favCommand.ExecuteNonQuery();
                }

                // Then remove the user
                string query = "DELETE FROM [User] WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new UserNotFoundException($"User with ID {userId} not found");
                    return rowsAffected > 0;
                }
            }
        }

        public User GetUserById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Valid User ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM [User] WHERE UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User(
                                (int)reader["UserID"],
                                reader["Username"].ToString(),
                                reader["Password"].ToString(),
                                reader["Email"].ToString(),
                                reader["FirstName"] is DBNull ? null : reader["FirstName"].ToString(),
                                reader["LastName"] is DBNull ? null : reader["LastName"].ToString(),
                                (DateTime)reader["DateOfBirth"],
                                reader["ProfilePicture"] is DBNull ? null : reader["ProfilePicture"].ToString()
                            );
                        }
                        else
                        {
                            throw new UserNotFoundException($"User with ID {userId} not found");
                        }
                    }
                }
            }
        }

        
        public List<User> SearchUsers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Search keyword is required");

            List<User> users = new List<User>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"SELECT * FROM [User] 
                                WHERE Username LIKE @Keyword 
                                OR Email LIKE @Keyword 
                                OR FirstName LIKE @Keyword 
                                OR LastName LIKE @Keyword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User(
                                (int)reader["UserID"],
                                reader["Username"].ToString(),
                                reader["Password"].ToString(),
                                reader["Email"].ToString(),
                                reader["FirstName"] is DBNull ? null : reader["FirstName"].ToString(),
                                reader["LastName"] is DBNull ? null : reader["LastName"].ToString(),
                                (DateTime)reader["DateOfBirth"],
                                reader["ProfilePicture"] is DBNull ? null : reader["ProfilePicture"].ToString()
                            ));
                        }
                    }
                }
            }
            return users;
        }

        public List<User> ListAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM [User]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User(
                                (int)reader["UserID"],
                                reader["Username"].ToString(),
                                reader["Password"].ToString(),
                                reader["Email"].ToString(),
                                reader["FirstName"] is DBNull ? null : reader["FirstName"].ToString(),
                                reader["LastName"] is DBNull ? null : reader["LastName"].ToString(),
                                (DateTime)reader["DateOfBirth"],
                                reader["ProfilePicture"] is DBNull ? null : reader["ProfilePicture"].ToString()
                            ));
                        }
                    }
                }
            }
            return users;
        }
    }
}
