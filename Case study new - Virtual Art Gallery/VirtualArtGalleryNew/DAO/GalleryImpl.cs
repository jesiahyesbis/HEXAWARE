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
    public class GalleryImpl:IGalleryService
    {
        public bool AddGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery cannot be null");
            if (string.IsNullOrWhiteSpace(gallery.Name))
                throw new ArgumentException("Gallery name is required");
            if (string.IsNullOrWhiteSpace(gallery.Location))
                throw new ArgumentException("Gallery location is required");
            if (gallery.Curator <= 0)
                throw new ArgumentException("Valid curator ID is required");
            
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string checkQuery = "SELECT COUNT(*) FROM Gallery WHERE GalleryID = @GalleryID";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@GalleryID", gallery.GalleryID);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new ArgumentException($"Gallery ID {gallery.GalleryID} already exists. IDs must be unique.");
                    }
                }
                string checkArtistQuery = "SELECT COUNT(*) FROM Artist WHERE ArtistID = @Curator";
                using (SqlCommand artistCheckCommand = new SqlCommand(checkArtistQuery, connection))
                {
                    artistCheckCommand.Parameters.AddWithValue("@Curator", gallery.Curator);
                    int artistCount = (int)artistCheckCommand.ExecuteScalar();
                    if (artistCount == 0)
                        throw new ArgumentException($"Artist with ID {gallery.Curator} does not exist.");
                }
                string query = @"INSERT INTO Gallery (GalleryID, Name, Description, Location, Curator, OpeningHours) 
                                VALUES (@GalleryID, @Name, @Description, @Location, @Curator, @OpeningHours)";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GalleryID", gallery.GalleryID);
                        command.Parameters.AddWithValue("@Name", gallery.Name);
                        command.Parameters.AddWithValue("@Description",
                            gallery.Description == null ? DBNull.Value : gallery.Description);
                        command.Parameters.AddWithValue("@Location", gallery.Location);
                        command.Parameters.AddWithValue("@Curator", gallery.Curator);
                        command.Parameters.AddWithValue("@OpeningHours",gallery.OpeningHours==null?DBNull.Value:gallery.OpeningHours);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }


                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new ArgumentException($"Gallery ID {gallery.GalleryID} already exists. IDs must be unique.");
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database error occurred: {ex.Message}");
                }

            }
        }

        public bool UpdateGallery(Gallery gallery)
        {
            if (gallery == null)
                throw new ArgumentNullException("Gallery cannot be null");
            if (gallery.GalleryID <= 0)
                throw new ArgumentException("Valid gallery ID is required");
            if (string.IsNullOrWhiteSpace(gallery.Name))
                throw new ArgumentException("Gallery name is required");
            if (string.IsNullOrWhiteSpace(gallery.Location))
                throw new ArgumentException("Gallery location is required");
            if (gallery.Curator <= 0)
                throw new ArgumentException("Valid curator ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"UPDATE Gallery SET 
                                Name = @Name, 
                                Description = @Description, 
                                Location = @Location, 
                                Curator = @Curator, 
                                OpeningHours = @OpeningHours 
                                WHERE GalleryID = @GalleryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GalleryID", gallery.GalleryID);
                    command.Parameters.AddWithValue("@Name", gallery.Name);
                    command.Parameters.AddWithValue("@Description",
                        gallery.Description == null ? DBNull.Value : gallery.Description);
                    command.Parameters.AddWithValue("@Location", gallery.Location);
                    command.Parameters.AddWithValue("@Curator", gallery.Curator);
                    command.Parameters.AddWithValue("@OpeningHours",
                        gallery.OpeningHours == null ? DBNull.Value : gallery.OpeningHours);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new GalleryNotFoundException($"Gallery with ID {gallery.GalleryID} not found");
                    return rowsAffected > 0;
                }
            }
        }

        public bool RemoveGallery(int galleryID)
        {
            if (galleryID <= 0)
                throw new ArgumentException("Valid gallery ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                // First remove junction table value
                string removeArtworksQuery = "DELETE FROM Artwork_Gallery WHERE GalleryID = @GalleryID";
                using (SqlCommand removeArtworksCmd = new SqlCommand(removeArtworksQuery, connection))
                {
                    removeArtworksCmd.Parameters.AddWithValue("@GalleryID", galleryID);
                    removeArtworksCmd.ExecuteNonQuery();
                }

                // Then remove the gallery
                string query = "DELETE FROM Gallery WHERE GalleryID = @GalleryID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GalleryID", galleryID);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new GalleryNotFoundException($"Gallery with ID {galleryID} not found");
                    return rowsAffected > 0;
                }
            }
        }

        public Gallery GetGalleryById(int galleryID)
        {
            if (galleryID <= 0)
                throw new ArgumentException("Valid gallery ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Gallery WHERE GalleryID = @GalleryID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GalleryID", galleryID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Gallery(
                                (int)reader["GalleryID"],
                                reader["Name"].ToString(),
                                reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                reader["Location"].ToString(),
                                (int)reader["Curator"],
                                reader["OpeningHours"] is DBNull ? null : reader["OpeningHours"].ToString()
                            );
                        }
                        else
                        {
                            throw new GalleryNotFoundException($"Gallery with ID {galleryID} not found");
                        }
                    }
                }
            }
        }

        public List<Gallery> SearchGalleries(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Search keyword is required");

            List<Gallery> galleries = new List<Gallery>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"SELECT * FROM Gallery 
                                WHERE Name LIKE @Keyword 
                                OR Description LIKE @Keyword 
                                OR Location LIKE @Keyword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            galleries.Add(new Gallery(
                                (int)reader["GalleryID"],
                                reader["Name"].ToString(),
                                reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                reader["Location"].ToString(),
                                (int)reader["Curator"],
                                reader["OpeningHours"] is DBNull ? null : reader["OpeningHours"].ToString()
                            ));
                        }
                    }
                }
            }
            return galleries;
        }

        public List<Gallery> ListAllGalleries()
        {
            List<Gallery> galleries = new List<Gallery>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Gallery";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            galleries.Add(new Gallery(
                                (int)reader["GalleryID"],
                                reader["Name"].ToString(),
                                reader["Description"] is DBNull ? null : reader["Description"].ToString(),
                                reader["Location"].ToString(),
                                (int)reader["Curator"],
                                reader["OpeningHours"] is DBNull ? null : reader["OpeningHours"].ToString()
                            ));
                        }
                    }
                }
            }
            return galleries;
        }


    }
}
