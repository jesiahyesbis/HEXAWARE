using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Entities;
using VirtualArtGalleryNew.Exceptions;
using VirtualArtGalleryNew.Util;

namespace VirtualArtGalleryNew.DAO
{
    public class UserFavoriteImpl:IUserFavoriteService
    {

        public bool AddArtworkToFavorite(int userId, int artworkId)
        {
            if (userId <= 0)
                throw new ArgumentException("Valid user ID is required");
            if (artworkId <= 0)
                throw new ArgumentException("Valid artwork ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string checkUserQuery = "SELECT COUNT(*) FROM [User] WHERE UserID = @UserID";
                using (SqlCommand userCmd = new SqlCommand(checkUserQuery, connection))
                {
                    userCmd.Parameters.AddWithValue("@UserID", userId);
                    int userCount = (int)userCmd.ExecuteScalar();
                    if (userCount == 0)
                        throw new ArgumentException($"User with ID {userId} does not exist.");
                }

                string checkArtworkQuery = "SELECT COUNT(*) FROM Artwork WHERE ArtworkID = @ArtworkID";
                using (SqlCommand artCmd = new SqlCommand(checkArtworkQuery, connection))
                {
                    artCmd.Parameters.AddWithValue("@ArtworkID", artworkId);
                    int artCount = (int)artCmd.ExecuteScalar();
                    if (artCount == 0)
                        throw new ArgumentException($"Artwork with ID {artworkId} does not exist.");
                }

                string query = @"INSERT INTO User_Favorite_Artwork (UserID, ArtworkID) 
                                VALUES (@UserID, @ArtworkID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@ArtworkID", artworkId);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex) when (ex.Number == 2627) // Primary key violation
                    {
                        throw new DuplicateEntryException("Artwork is already in favorites");
                    }
                }
            }
        }

        public bool RemoveArtworkFromFavorite(int userId, int artworkId)
        {
            if (userId <= 0)
                throw new ArgumentException("Valid user ID is required");
            if (artworkId <= 0)
                throw new ArgumentException("Valid artwork ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string checkUserQuery = "SELECT COUNT(*) FROM [User] WHERE UserID = @UserID";
                using (SqlCommand userCmd = new SqlCommand(checkUserQuery, connection))
                {
                    userCmd.Parameters.AddWithValue("@UserID", userId);
                    int userCount = (int)userCmd.ExecuteScalar();
                    if (userCount == 0)
                        throw new ArgumentException($"User with ID {userId} does not exist.");
                }

                string checkArtworkQuery = "SELECT COUNT(*) FROM Artwork WHERE ArtworkID = @ArtworkID";
                using (SqlCommand artCmd = new SqlCommand(checkArtworkQuery, connection))
                {
                    artCmd.Parameters.AddWithValue("@ArtworkID", artworkId);
                    int artCount = (int)artCmd.ExecuteScalar();
                    if (artCount == 0)
                        throw new ArgumentException($"Artwork with ID {artworkId} does not exist.");
                }

                string query = @"DELETE FROM User_Favorite_Artwork 
                                WHERE UserID = @UserID AND ArtworkID = @ArtworkID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@ArtworkID", artworkId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new ArtworkNotFoundException("Artwork not found in user's favorites");
                    return rowsAffected > 0;
                }
            }
        }

        public List<Artwork> GetUserFavoriteArtworks(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Valid user ID is required");

            List<Artwork> favoriteArtworks = new List<Artwork>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string checkUserQuery = "SELECT COUNT(*) FROM [User] WHERE UserID = @UserID";
                using (SqlCommand userCmd = new SqlCommand(checkUserQuery, connection))
                {
                    userCmd.Parameters.AddWithValue("@UserID", userId);
                    int userCount = (int)userCmd.ExecuteScalar();
                    if (userCount == 0)
                        throw new ArgumentException($"User with ID {userId} does not exist.");
                }

                
                string query = @"SELECT a.* FROM Artwork a
                                JOIN User_Favorite_Artwork ufa ON a.ArtworkID = ufa.ArtworkID
                                WHERE ufa.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            favoriteArtworks.Add(new Artwork(
                                (int)reader["ArtworkID"],
                                reader["Title"].ToString(),
                                reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                (DateTime)reader["CreationDate"],
                                reader["Medium"].ToString(),
                                reader["ImageURL"].ToString(),
                                (int)reader["ArtistID"]
                            ));
                        }
                    }
                }
            }
            return favoriteArtworks;
        }


    }
}
