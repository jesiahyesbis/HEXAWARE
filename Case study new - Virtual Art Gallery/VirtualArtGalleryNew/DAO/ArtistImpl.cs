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
    public class ArtistImpl:IArtistService
    {
        public bool AddArtist(Artist artist)
        {
            if (artist == null)
                throw new ArgumentNullException("Artist cannot be null");
            if (string.IsNullOrWhiteSpace(artist.Name))
                throw new ArgumentException("Artist name is required");
            
            if (string.IsNullOrWhiteSpace(artist.Nationality))
                throw new ArgumentException("Nationality is required");
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string checkQuery = "SELECT COUNT(*) FROM Artist WHERE ArtistID = @ArtistID";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ArtistID", artist.ArtistID);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        throw new ArgumentException($"Artist ID {artist.ArtistID} already exists. IDs must be unique.");
                    }
                }
                
                string query = @"INSERT INTO Artist (ArtistID, Name, Biography, BirthDate, Nationality, Website, ContactInformation) 
                                VALUES (@ArtistID, @Name, @Biography, @BirthDate, @Nationality, @Website, @ContactInformation)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArtistID", artist.ArtistID);
                        command.Parameters.AddWithValue("@Name", artist.Name);
                        command.Parameters.AddWithValue("@Biography",
                            artist.Biography == null ? DBNull.Value : artist.Biography);
                        command.Parameters.AddWithValue("@BirthDate", artist.BirthDate);
                        command.Parameters.AddWithValue("@Nationality", artist.Nationality);
                        command.Parameters.AddWithValue("@Website",
                            artist.Website == null ? DBNull.Value : artist.Website);
                        command.Parameters.AddWithValue("@ContactInformation",
                          artist.ContactInformation == null ? DBNull.Value : artist.ContactInformation);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }

                catch (SqlException ex) when (ex.Number == 2627)
                {
                    throw new ArgumentException($"Artwork ID {artist.ArtistID} already exists. IDs must be unique.");
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database error occurred: {ex.Message}");
                }

            }
        }

        public bool UpdateArtist(Artist artist)
        {
            if (artist == null)
                throw new ArgumentNullException("Artist cannot be null");
            if (artist.ArtistID <= 0)
                throw new ArgumentException("Valid Artist ID is required");
            if (string.IsNullOrWhiteSpace(artist.Name))
                throw new ArgumentException("Artist name is required");
            if (string.IsNullOrWhiteSpace(artist.Nationality))
                throw new ArgumentException("Nationality is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"UPDATE Artist SET 
                                Name = @Name, 
                                Biography = @Biography, 
                                BirthDate = @BirthDate, 
                                Nationality = @Nationality, 
                                Website = @Website, 
                                ContactInformation = @ContactInformation
                                WHERE ArtistID = @ArtistID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistID", artist.ArtistID);
                    command.Parameters.AddWithValue("@Name", artist.Name);
                    command.Parameters.AddWithValue("@Biography",
                        artist.Biography == null ? DBNull.Value : artist.Biography);
                    command.Parameters.AddWithValue("@BirthDate", artist.BirthDate);
                    command.Parameters.AddWithValue("@Nationality", artist.Nationality);
                    command.Parameters.AddWithValue("@Website",
                        artist.Website == null ? DBNull.Value : artist.Website);
                    command.Parameters.AddWithValue("@ContactInformation",
                        artist.ContactInformation == null ? DBNull.Value : artist.ContactInformation);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new ArtistNotFoundException($"Artist with ID {artist.ArtistID} not found");
                    return rowsAffected > 0;
                }
            }
        }

        public bool RemoveArtist(int artistId)
        {
            if (artistId <= 0)
                throw new ArgumentException("Valid Artist ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                // First check if artist exists
                string checkQuery = "SELECT COUNT(*) FROM Artist WHERE ArtistID = @ArtistID";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ArtistID", artistId);
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count == 0)
                        throw new ArtistNotFoundException($"Artist with ID {artistId} not found");
                }

                // Then remove the artist
                string query = "DELETE FROM Artist WHERE ArtistID = @ArtistID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistID", artistId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Artist GetArtistById(int artistId)
        {
            if (artistId <= 0)
                throw new ArgumentException("Valid Artist ID is required");

            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Artist WHERE ArtistID = @ArtistID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArtistID", artistId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Artist(
                                (int)reader["ArtistID"],
                                reader["Name"].ToString(),
                                reader["Biography"] is DBNull ? null : reader["Biography"].ToString(),
                                (DateTime)reader["BirthDate"],
                                reader["Nationality"].ToString(),
                                reader["Website"] is DBNull ? null : reader["Website"].ToString(),
                                reader["ContactInformation"] is DBNull ? null : reader["ContactInformation"].ToString()
                            );
                        }
                        else
                        {
                            throw new ArtistNotFoundException($"Artist with ID {artistId} not found");
                        }
                    }
                }
            }
        }

        public List<Artist> SearchArtists(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new ArgumentException("Search keyword is required");

            List<Artist> artists = new List<Artist>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = @"SELECT * FROM Artist 
                                WHERE Name LIKE @Keyword 
                                OR Biography LIKE @Keyword 
                                OR Nationality LIKE @Keyword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artists.Add(new Artist(
                                (int)reader["ArtistID"],
                                reader["Name"].ToString(),
                                reader["Biography"] is DBNull ? null : reader["Biography"].ToString(),
                                (DateTime)reader["BirthDate"],
                                reader["Nationality"].ToString(),
                                reader["Website"] is DBNull ? null : reader["Website"].ToString(),
                                reader["ContactInformation"] is DBNull ? null : reader["ContactInformation"].ToString()
                            ));
                        }
                    }
                }
            }
            return artists;
        }

        public List<Artist> ListAllArtists()
        {
            List<Artist> artists = new List<Artist>();
            using (SqlConnection connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Artist";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            artists.Add(new Artist(
                                (int)reader["ArtistID"],
                                reader["Name"].ToString(),
                                reader["Biography"] is DBNull ? null : reader["Biography"].ToString(),
                                (DateTime)reader["BirthDate"],
                                reader["Nationality"].ToString(),
                                reader["Website"] is DBNull ? null : reader["Website"].ToString(),
                                reader["ContactInformation"] is DBNull ? null : reader["ContactInformation"].ToString()
                            ));
                        }
                    }
                }
            }
            return artists;
        }

    }
}
