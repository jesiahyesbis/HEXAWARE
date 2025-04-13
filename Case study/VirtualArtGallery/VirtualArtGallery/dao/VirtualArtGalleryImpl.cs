using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGallery.entity;
using VirtualArtGallery.exception;
using VirtualArtGallery.util;
using System.Data.SqlClient;

namespace VirtualArtGallery.dao
{
    public class VirtualArtGalleryImpl : IVirtualArtGallery
    {
        private readonly SqlConnection connection;

        public VirtualArtGalleryImpl()
        {
            connection = DBConnUtil.GetConnection();
        }

        // Artwork Management
        public bool AddArtwork(Artwork artwork)
        {
            try
            {
                string query = "INSERT INTO Artwork (Title, Description, CreationDate, Medium, ImageURL, ArtistID) " +
                              "VALUES (@Title, @Description, @CreationDate, @Medium, @ImageURL, @ArtistID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", artwork.Title);
                    command.Parameters.AddWithValue("@Description", artwork.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreationDate", artwork.CreationDate);
                    command.Parameters.AddWithValue("@Medium", artwork.Medium ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImageURL", artwork.ImageURL ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ArtistID", artwork.ArtistID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error adding artwork: " + ex.Message);
            }
        }

        public bool UpdateArtwork(Artwork artwork)
        {
            try
            {
                string query = "UPDATE Artwork SET Title = @Title, Description = @Description, " +
                              "CreationDate = @CreationDate, Medium = @Medium, ImageURL = @ImageURL, " +
                              "ArtistID = @ArtistID WHERE ArtworkID = @ArtworkID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artwork.ArtworkID);
                    command.Parameters.AddWithValue("@Title", artwork.Title);
                    command.Parameters.AddWithValue("@Description", artwork.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreationDate", artwork.CreationDate);
                    command.Parameters.AddWithValue("@Medium", artwork.Medium ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImageURL", artwork.ImageURL ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ArtistID", artwork.ArtistID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected == 0)
                        throw new ArtWorkNotFoundException($"Artwork with ID {artwork.ArtworkID} not found");

                    return rowsAffected > 0;
                }
            }
            catch (ArtWorkNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error updating artwork: " + ex.Message);
            }
        }

        public bool RemoveArtwork(int artworkId)
        {
            try
            {
                // First remove from junction tables
                string removeFavoritesQuery = "DELETE FROM User_Favorite_Artwork WHERE ArtworkID = @ArtworkID";
                string removeGalleryQuery = "DELETE FROM Artwork_Gallery WHERE ArtworkID = @ArtworkID";
                string removeArtworkQuery = "DELETE FROM Artwork WHERE ArtworkID = @ArtworkID";

                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Remove from favorites
                        using (SqlCommand command = new SqlCommand(removeFavoritesQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ArtworkID", artworkId);
                            command.ExecuteNonQuery();
                        }

                        // Remove from galleries
                        using (SqlCommand command = new SqlCommand(removeGalleryQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ArtworkID", artworkId);
                            command.ExecuteNonQuery();
                        }

                        // Remove artwork
                        using (SqlCommand command = new SqlCommand(removeArtworkQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ArtworkID", artworkId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                throw new ArtWorkNotFoundException($"Artwork with ID {artworkId} not found");
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (ArtWorkNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing artwork: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Artwork GetArtworkById(int artworkId)
        {
            try
            {
                string query = "SELECT * FROM Artwork WHERE ArtworkID = @ArtworkID";
                Artwork artwork = null;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artworkId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            artwork = new Artwork
                            {
                                ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : null,
                                ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : null,
                                ArtistID = Convert.ToInt32(reader["ArtistID"])
                            };
                        }
                    }
                    connection.Close();
                }

                if (artwork == null)
                    throw new ArtWorkNotFoundException($"Artwork with ID {artworkId} not found");

                return artwork;
            }
            catch (ArtWorkNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error retrieving artwork: " + ex.Message);
            }
        }

        public List<Artwork> SearchArtworks(string keyword)
        {
            List<Artwork> artworks = new List<Artwork>();

            try
            {
                string query = "SELECT * FROM Artwork WHERE Title LIKE @Keyword OR Description LIKE @Keyword";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artworks.Add(new Artwork
                            {
                                ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : null,
                                ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : null,
                                ArtistID = Convert.ToInt32(reader["ArtistID"])
                            });
                        }
                    }
                    connection.Close();
                }

                return artworks;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error searching artworks: " + ex.Message);
            }
        }

        // User Favorites
        public bool AddArtworkToFavorite(int userId, int artworkId)
        {
            try
            {
                // Check if user exists
                string userCheckQuery = "SELECT COUNT(*) FROM [User] WHERE UserID = @UserID";
                string artworkCheckQuery = "SELECT COUNT(*) FROM Artwork WHERE ArtworkID = @ArtworkID";
                string insertQuery = "INSERT INTO User_Favorite_Artwork (UserID, ArtworkID) VALUES (@UserID, @ArtworkID)";

                connection.Open();

                // Check user exists
                using (SqlCommand command = new SqlCommand(userCheckQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    if (userCount == 0)
                    {
                        connection.Close();
                        throw new UserNotFoundException($"User with ID {userId} not found");
                    }
                }

                // Check artwork exists
                using (SqlCommand command = new SqlCommand(artworkCheckQuery, connection))
                {
                    command.Parameters.AddWithValue("@ArtworkID", artworkId);
                    int artworkCount = Convert.ToInt32(command.ExecuteScalar());
                    if (artworkCount == 0)
                    {
                        connection.Close();
                        throw new ArtWorkNotFoundException($"Artwork with ID {artworkId} not found");
                    }
                }

                // Add to favorites
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
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
                        throw new Exception("This artwork is already in the user's favorites");
                    }
                }
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (ArtWorkNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding artwork to favorites: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool RemoveArtworkFromFavorite(int userId, int artworkId)
        {
            try
            {
                string query = "DELETE FROM User_Favorite_Artwork WHERE UserID = @UserID AND ArtworkID = @ArtworkID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@ArtworkID", artworkId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected == 0)
                        throw new Exception("Artwork not found in user's favorites");

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error removing artwork from favorites: " + ex.Message);
            }
        }

        public List<Artwork> GetUserFavoriteArtworks(int userId)
        {
            List<Artwork> artworks = new List<Artwork>();

            try
            {
                string query = "SELECT a.* FROM Artwork a " +
                               "INNER JOIN User_Favorite_Artwork ufa ON a.ArtworkID = ufa.ArtworkID " +
                               "WHERE ufa.UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artworks.Add(new Artwork
                            {
                                ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                                Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : null,
                                ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : null,
                                ArtistID = Convert.ToInt32(reader["ArtistID"])
                            });
                        }
                    }
                    connection.Close();
                }

                if (artworks.Count == 0)
                    throw new Exception("No favorite artworks found for this user");

                return artworks;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error retrieving favorite artworks: " + ex.Message);
            }
        }

        // Additional methods
        public List<Artist> GetAllArtists()
        {
            List<Artist> artists = new List<Artist>();

            try
            {
                string query = "SELECT * FROM Artist";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artists.Add(new Artist
                            {
                                ArtistID = Convert.ToInt32(reader["ArtistID"]),
                                Name = reader["Name"].ToString(),
                                Biography = reader["Biography"] != DBNull.Value ? reader["Biography"].ToString() : null,
                                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                                Nationality = reader["Nationality"] != DBNull.Value ? reader["Nationality"].ToString() : null,
                                Website = reader["Website"] != DBNull.Value ? reader["Website"].ToString() : null,
                                ContactInformation = reader["ContactInformation"] != DBNull.Value ? reader["ContactInformation"].ToString() : null
                            });
                        }
                    }
                    connection.Close();
                }

                return artists;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error retrieving artists: " + ex.Message);
            }
        }

        public List<Gallery> GetAllGalleries()
        {
            List<Gallery> galleries = new List<Gallery>();

            try
            {
                string query = "SELECT * FROM Gallery";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            galleries.Add(new Gallery
                            {
                                GalleryID = Convert.ToInt32(reader["GalleryID"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                                Curator = Convert.ToInt32(reader["Curator"]),
                                OpeningHours = reader["OpeningHours"] != DBNull.Value ? reader["OpeningHours"].ToString() : null
                            });
                        }
                    }
                    connection.Close();
                }

                return galleries;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("Error retrieving galleries: " + ex.Message);
            }
        }


        //*********************************************************
         public bool AddGallery(Gallery gallery)
{
    using (SqlConnection conn = DBConnUtil.GetConnection())
    {
        string query = @"INSERT INTO Gallery (Name, Description, Location, Curator, OpeningHours) 
                         VALUES (@Name, @Description, @Location, @Curator, @OpeningHours)";
        
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Name", gallery.Name);
        cmd.Parameters.AddWithValue("@Description", gallery.Description ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Location", gallery.Location ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Curator", gallery.Curator);
        cmd.Parameters.AddWithValue("@OpeningHours", gallery.OpeningHours ?? (object)DBNull.Value);

        conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }
}

public bool UpdateGallery(Gallery gallery)
{
    using (SqlConnection conn = DBConnUtil.GetConnection())
    {
        string query = @"UPDATE Gallery SET 
                        Name = @Name,
                        Description = @Description,
                        Location = @Location,
                        Curator = @Curator,
                        OpeningHours = @OpeningHours
                        WHERE GalleryID = @GalleryID";

        SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GalleryID", gallery.GalleryID);
                cmd.Parameters.AddWithValue("@Name", gallery.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", gallery.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Location", gallery.Location ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Curator", gallery.Curator);
                cmd.Parameters.AddWithValue("@OpeningHours", gallery.OpeningHours ?? (object)DBNull.Value);

                conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }
}

public bool RemoveGallery(int galleryId)
{
    using (SqlConnection conn = DBConnUtil.GetConnection())
    {
        // First remove from junction table
        string removeArtworksQuery = "DELETE FROM Artwork_Gallery WHERE GalleryID = @GalleryID";
        SqlCommand cmd1 = new SqlCommand(removeArtworksQuery, conn);
        cmd1.Parameters.AddWithValue("@GalleryID", galleryId);

        // Then remove gallery
        string removeGalleryQuery = "DELETE FROM Gallery WHERE GalleryID = @GalleryID";
        SqlCommand cmd2 = new SqlCommand(removeGalleryQuery, conn);
        cmd2.Parameters.AddWithValue("@GalleryID", galleryId);

        conn.Open();
        using (SqlTransaction transaction = conn.BeginTransaction())
        {
            try
            {
                cmd1.Transaction = transaction;
                cmd2.Transaction = transaction;

                cmd1.ExecuteNonQuery();
                int rowsAffected = cmd2.ExecuteNonQuery();
                
                transaction.Commit();
                return rowsAffected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

public List<Gallery> SearchGalleries(string keyword)
{
    List<Gallery> galleries = new List<Gallery>();
    using (SqlConnection conn = DBConnUtil.GetConnection())
    {
        string query = @"SELECT * FROM Gallery 
                        WHERE Name LIKE @Keyword 
                        OR Description LIKE @Keyword
                        OR Location LIKE @Keyword";
        
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

        conn.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                galleries.Add(new Gallery
                {
                    GalleryID = Convert.ToInt32(reader["GalleryID"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                    Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                    Curator = Convert.ToInt32(reader["Curator"]),
                    OpeningHours = reader["OpeningHours"] != DBNull.Value ? reader["OpeningHours"].ToString() : null
                });
            }
        }
    }
    return galleries;
}
         //*************************************************



    }
}
