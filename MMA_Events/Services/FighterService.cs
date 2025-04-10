using MMA_Events.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MMA_Events.Services
{
   public class FighterService
    {
        private readonly string connectionString;
        static private FighterService instance = null;

        private FighterService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static FighterService getInstance()
        {
            if (instance == null)
            {
                instance = new FighterService();
            }
            return instance;

        }
        public List<FighterDetails> GetAllFighters()
        {
            List<FighterDetails> details = new List<FighterDetails>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from FighterDetails;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                details.Add(new FighterDetails
                                {
                                    idFighter = reader.GetInt32("idFighter"),
                                    Nickname = reader.IsDBNull("Nickname") ? string.Empty : reader.GetString("Nickname"),
                                    Name = reader.GetString("Name"),
                                    Surname = reader.GetString("Surname"),
                                    Country = reader.GetString("Country"),
                                    CategoryName = reader.GetString("CategoryName"),
                                    BirthDate = reader.GetDateTime("BirthDate").Date.ToString("dd.MM.yyyy"),
                                    FightWeight = reader.GetFloat("FightWeight"),
                                    Image = reader.GetString("ProfileImage"),
                                    Wins = reader.GetInt32("Wins"),
                                    Losses = reader.GetInt32("Losses"),
                                    Draws = reader.GetInt32("Draws"),
                                    KOs = reader.GetInt32("KOs"),
                                    Submissions = reader.GetInt32("Submissions")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
            }

            return details;
        }
        public List<FighterDetails> GetAllFighters(Organizator org)
        {
            List<FighterDetails> details = new List<FighterDetails>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from FighterDetails WHERE idOrganization=@Id;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", org.IdOrganizator);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                details.Add(new FighterDetails
                                {
                                    idFighter = reader.GetInt32("idFighter"),
                                    Nickname = reader.IsDBNull("Nickname") ? string.Empty : reader.GetString("Nickname"),
                                    Name = reader.GetString("Name"),
                                    Surname = reader.GetString("Surname"),
                                    Country = reader.GetString("Country"),
                                    CategoryName = reader.GetString("CategoryName"),
                                    BirthDate = reader.GetDateTime("BirthDate").Date.ToString("dd.MM.yyyy"),
                                    FightWeight = reader.GetFloat("FightWeight"),
                                    Image = reader.GetString("ProfileImage"),
                                    Wins = reader.GetInt32("Wins"),
                                    Losses = reader.GetInt32("Losses"),
                                    Draws = reader.GetInt32("Draws"),
                                    KOs = reader.GetInt32("KOs"),
                                    Submissions = reader.GetInt32("Submissions")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
            }

            return details;
        }

        public FighterDetails GetFighterByID(Organizator org, int id)
        {
            FighterDetails details = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from FighterDetails WHERE idOrganization=@Id and idFighter=@IdFighter;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", org.IdOrganizator);
                        command.Parameters.AddWithValue("@IdFighter", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                details = new FighterDetails
                                {
                                    idFighter = reader.GetInt32("idFighter"),
                                    Nickname = reader.IsDBNull("Nickname") ? string.Empty : reader.GetString("Nickname"),
                                    Name = reader.GetString("Name"),
                                    Surname = reader.GetString("Surname"),
                                    Country = reader.GetString("Country"),
                                    CategoryName = reader.GetString("CategoryName"),
                                    BirthDate = reader.GetDateTime("BirthDate").Date.ToString("dd.MM.yyyy"),
                                    FightWeight = reader.GetFloat("FightWeight"),
                                    Image = reader.GetString("ProfileImage"),
                                    Wins = reader.GetInt32("Wins"),
                                    Losses = reader.GetInt32("Losses"),
                                    Draws = reader.GetInt32("Draws"),
                                    KOs = reader.GetInt32("KOs"),
                                    Submissions = reader.GetInt32("Submissions")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
            }

            return details;
        }

        public bool addFighter(Fighter fighter, FighterStats stats)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Person (Name, Surname, Country) 
                VALUES (@Name, @Surname, @Country);

                INSERT INTO Fighter (idFighter, Nickname, idCategory, BirthDate, isReady, idOrganization, FightWeight, ProfileImage) 
                VALUES (LAST_INSERT_ID(), @Nickname, @Category, @BirthDate, @IsReady, @Organization, @FightWeight, @ProfileImage);

                INSERT INTO Fighter_Stats (idFighter, Wins, Losses, Draws, KOs, Submissions) 
                VALUES (LAST_INSERT_ID(), @Wins, @Losses, @Draws, @KOs, @Submissions);
            ";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", fighter.Name);
                        command.Parameters.AddWithValue("@Surname", fighter.Surname);
                        command.Parameters.AddWithValue("@Nickname", fighter.Nickname);
                        command.Parameters.AddWithValue("@Country", fighter.Country);
                        command.Parameters.AddWithValue("@Category", fighter.IdCategory);
                        command.Parameters.AddWithValue("@BirthDate", fighter.BirthDate);
                        command.Parameters.AddWithValue("@IsReady", fighter.IsReady);
                        command.Parameters.AddWithValue("@Organization", fighter.IdOrganization);
                        command.Parameters.AddWithValue("@FightWeight", fighter.FightWeight);
                        command.Parameters.AddWithValue("@ProfileImage", fighter.Image);
                        command.Parameters.AddWithValue("@Wins", stats.Wins);
                        command.Parameters.AddWithValue("@Losses", stats.Losses);
                        command.Parameters.AddWithValue("@Draws", stats.Draws);
                        command.Parameters.AddWithValue("@KOs", stats.KOs);
                        command.Parameters.AddWithValue("@Submissions", stats.Submissions);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
                return false;
            }
        }

    }
}
