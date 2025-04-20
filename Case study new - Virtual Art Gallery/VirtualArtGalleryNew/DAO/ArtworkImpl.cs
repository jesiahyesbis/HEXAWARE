using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using VirtualArtGalleryNew.Entities;
using VirtualArtGalleryNew.DAO.Interfaces;
using VirtualArtGalleryNew.Util;
using VirtualArtGalleryNew.Exceptions;

namespace VirtualArtGalleryNew.DAO
{
    

    public class ArtworkImpl:IArtworkService
    {
        public bool AddArtwork(Artwork artwork)
        {
            if (artwork == null){ throw new ArgumentNullException("Artwork cannot be null");}
            if (string.IsNullOrEmpty(artwork.Title)){throw new ArgumentException("Artwork title cannot be empty");}
            if (artwork.ArtistID <= 0){throw new ArgumentException("Invalid Artist ID");}
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                    string checkQuery = "SELECT COUNT(*) FROM Artwork WHERE ArtworkID = @ArtworkID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ArtworkID", artwork.ArtworkID);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new ArgumentException($"Artwork ID {artwork.ArtworkID} already exists. IDs must be unique.");
                        }
                    }

                string checkArtistQuery = "SELECT COUNT(*) FROM Artist WHERE ArtistID = @ArtistID";
                using (SqlCommand artistCheckCommand = new SqlCommand(checkArtistQuery, connection))
                {
                    artistCheckCommand.Parameters.AddWithValue("@ArtistID", artwork.ArtistID);
                    int artistCount = (int)artistCheckCommand.ExecuteScalar();
                    if (artistCount == 0)
                        throw new ArgumentException($"Artist with ID {artwork.ArtistID} does not exist.");
                }
                string query = "INSERT INTO Artwork (ArtworkID, Title, Description, CreationDate, Medium, ImageURL, ArtistID) " +
                              "VALUES (@ArtworkID, @Title, @Description, @CreationDate, @Medium, @ImageURL, @ArtistID)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArtworkID", artwork.ArtworkID);
                        command.Parameters.AddWithValue("@Title", artwork.Title);
                        command.Parameters.AddWithValue("@Description", artwork.Description!= null ? artwork.Description : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreationDate", artwork.CreationDate);
                        command.Parameters.AddWithValue("@Medium", artwork.Medium);
                        command.Parameters.AddWithValue("@ImageURL", artwork.ImageURL);
                        command.Parameters.AddWithValue("@ArtistID", artwork.ArtistID);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }

                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new ArgumentException($"Artwork ID {artwork.ArtworkID} already exists. IDs must be unique.");
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database error occurred: {ex.Message}");
                }
            }
        }


        public bool UpdateArtwork(Artwork artwork)
        {
            if (artwork == null){throw new ArgumentNullException("Artwork cannot be null"); }
            if (artwork.ArtworkID <= 0){throw new ArgumentException("Invalid Artwork ID");}
            if (string.IsNullOrEmpty(artwork.Title)){throw new ArgumentException("Artwork title cannot be empty");}

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "UPDATE Artwork SET Title = @Title, Description = @Description, " +
                              "CreationDate = @CreationDate, Medium = @Medium, ImageURL = @ImageURL, " +
                              "ArtistID = @ArtistID WHERE ArtworkID = @ArtworkID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artwork.ArtworkID);
                    command.Parameters.AddWithValue("@Title", artwork.Title);
                    command.Parameters.AddWithValue("@Description", artwork.Description);
                    command.Parameters.AddWithValue("@CreationDate", artwork.CreationDate);
                    command.Parameters.AddWithValue("@Medium", artwork.Medium);
                    command.Parameters.AddWithValue("@ImageURL", artwork.ImageURL);
                    command.Parameters.AddWithValue("@ArtistID", artwork.ArtistID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new ArtworkNotFoundException($"Artwork with ID {artwork.ArtworkID} not found");
                    }
                    return rowsAffected > 0;
                }
            }
        }

        public bool RemoveArtwork(int artworkID)
        {
            if (artworkID <= 0){ throw new ArgumentException("Invalid Artwork ID");}

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                // First remove from junction tables
                string removeFavorites = "DELETE FROM User_Favorite_Artwork WHERE ArtworkID = @ArtworkID";
                using (SqlCommand favCommand = new SqlCommand(removeFavorites, connection))
                {
                    favCommand.Parameters.AddWithValue("@ArtworkID", artworkID);
                    favCommand.ExecuteNonQuery();
                }

                string removeGallery = "DELETE FROM Artwork_Gallery WHERE ArtworkID = @ArtworkID";
                using (SqlCommand galleryCommand = new SqlCommand(removeGallery, connection))
                {
                    galleryCommand.Parameters.AddWithValue("@ArtworkID", artworkID);
                    galleryCommand.ExecuteNonQuery();
                }

                // Then remove the artwork
                string query = "DELETE FROM Artwork WHERE ArtworkID = @ArtworkID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artworkID);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new ArtworkNotFoundException($"Artwork with ID {artworkID} not found");
                    }
                    return rowsAffected > 0;
                }
            }
        }

        public Artwork GetArtwork(int artworkID)
        {
            if (artworkID <= 0){ throw new ArgumentException("Invalid Artwork ID");}

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Artwork WHERE ArtworkID = @ArtworkID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artworkID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Artwork(
                                (int)reader["ArtworkID"],
                                reader["Title"].ToString(),
                                reader["Description"].ToString(),
                                (DateTime)reader["CreationDate"],
                                reader["Medium"].ToString(),
                                reader["ImageURL"].ToString(),
                                (int)reader["ArtistID"]
                            );
                        }
                        else
                        {
                            throw new ArtworkNotFoundException($"Artwork with ID {artworkID} not found");
                        }
                    }
                }
            }
        }

        public List<Artwork> SearchArtwork(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)){throw new ArgumentException("Search keyword cannot be empty"); }

            List<Artwork> artworks = new List<Artwork>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Artwork WHERE Title LIKE @Keyword OR Description LIKE @Keyword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artworks.Add(new Artwork(
                                (int)reader["ArtworkID"],
                                reader["Title"].ToString(),
                                reader["Description"].ToString(),
                                (DateTime)reader["CreationDate"],
                                reader["Medium"].ToString(),
                                reader["ImageURL"].ToString(),
                                (int)reader["ArtistID"]
                            ));
                        }
                    }
                }
                return artworks;
            }
        }

        public List<Artwork> ListAllArtworks()
        {
            List<Artwork> artworks = new List<Artwork>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Artwork";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artworks.Add(new Artwork(
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
            return artworks;
        }







    }
}
