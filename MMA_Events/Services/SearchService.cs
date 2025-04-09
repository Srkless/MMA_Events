using MMA_Events.Model;
using MMA_Events.Model;
using MMA_Events.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Services
{
    public class SearchService
    {
        private readonly string connectionString;
        static private SearchService instance = null;

        private SearchService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static SearchService getInstance()
        {
            if (instance == null)
            {
                instance = new SearchService();
            }
            return instance;

        }

        public List<SearchDetails> SearchFightersAndEvents(string searchTerm)
        {
            List<SearchDetails> results = new List<SearchDetails>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string fighterQuery = @"
            SELECT *
            FROM FighterDetails
            WHERE Nickname LIKE @searchTerm 
               OR Name LIKE @searchTerm 
               OR Surname LIKE @searchTerm 
               OR (Name + ' ' + Surname) LIKE @searchTerm";

                using (MySqlCommand cmd = new MySqlCommand(fighterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SearchDetails
                            {
                                Fighter = new FighterDetails
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
                                },
                                Event = null // Ostavlja se null jer je ovo borac
                            });
                        }
                    }
                }


                FighterService fighterService = FighterService.getInstance();
                AuthService authService = AuthService.getInstance();
                string eventQuery = @"
            SELECT *
            FROM EventDetails
            WHERE Name LIKE @searchTerm or
                  Location like @searchTerm";

                using (MySqlCommand cmd = new MySqlCommand(eventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SearchDetails
                            {
                                Fighter = null, // Ostavlja se null jer je ovo događaj
                                Event = new EventDetails
                                {
                                    idEvent = reader.GetInt32("idEvent"),
                                    idFightCard = reader.GetInt32("idFightCard"),
                                    idOrganization = reader.GetInt32("idOrganization"),
                                    Name = reader.GetString("Name"),
                                    Date = reader.GetDateTime("Date").ToString("dd-MM-yyyy"),
                                    Location = reader.GetString("Location"),
                                    idRedCorner = reader.GetInt32("idRedCorner"),
                                    idBlueCorner = reader.GetInt32("idBlueCorner"),
                                    RedCorner = fighterService.GetFighterByID(authService.GetOrganizatorByID(reader.GetInt32("idOrganization")), reader.GetInt32("idRedCorner")),
                                    BlueCorner = fighterService.GetFighterByID(authService.GetOrganizatorByID(reader.GetInt32("idOrganization")), reader.GetInt32("idBlueCorner"))

                                }
                            });
                        }
                    }
                }
            }
            return results;
        }

        public List<SearchDetails> SearchFighters(string searchTerm)
        {
            List<SearchDetails> results = new List<SearchDetails>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string fighterQuery = @"
            SELECT *
            FROM FighterDetails
            WHERE Nickname LIKE @searchTerm 
               OR Name LIKE @searchTerm 
               OR Surname LIKE @searchTerm 
               OR (Name + ' ' + Surname) LIKE @searchTerm";

                using (MySqlCommand cmd = new MySqlCommand(fighterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SearchDetails
                            {
                                Fighter = new FighterDetails
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
                                },
                                Event = null // Ostavlja se null jer je ovo borac
                            });
                        }
                    }
                }

            }
            return results;
        }

        public List<SearchDetails> SearchEvents(string searchTerm)
        {
            List<SearchDetails> results = new List<SearchDetails>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                FighterService fighterService = FighterService.getInstance();
                AuthService authService = AuthService.getInstance();
                string eventQuery = @"
            SELECT *
            FROM EventDetails
            WHERE Name LIKE @searchTerm or
                  Location like @searchTerm";

                using (MySqlCommand cmd = new MySqlCommand(eventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new SearchDetails
                            {
                                Fighter = null, // Ostavlja se null jer je ovo događaj
                                Event = new EventDetails
                                {
                                    idEvent = reader.GetInt32("idEvent"),
                                    idFightCard = reader.GetInt32("idFightCard"),
                                    idOrganization = reader.GetInt32("idOrganization"),
                                    Name = reader.GetString("Name"),
                                    Date = reader.GetDateTime("Date").ToString("dd-MM-yyyy"),
                                    Location = reader.GetString("Location"),
                                    idRedCorner = reader.GetInt32("idRedCorner"),
                                    idBlueCorner = reader.GetInt32("idBlueCorner"),
                                    RedCorner = fighterService.GetFighterByID(authService.GetOrganizatorByID(reader.GetInt32("idOrganization")), reader.GetInt32("idRedCorner")),
                                    BlueCorner = fighterService.GetFighterByID(authService.GetOrganizatorByID(reader.GetInt32("idOrganization")), reader.GetInt32("idBlueCorner"))

                                }
                            });
                        }
                    }
                }
            }
            return results;
        }

    }
}
