using MMA_Events.Model;
using MMA_Events.Services;
using MMA_Events.View;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MMA_Events.Services
{

    public class EventService
    {
        private readonly string connectionString;
        private static EventService instance = null;

        private EventService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static EventService getInstance()
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;

        }

        public List<Event> GetAllEvents(Organizator org)
        {
            List<Event> events = new List<Event>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from Event WHERE idOrganization=@Id;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", org.IdOrganizator);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(new Event
                                {
                                    Id = reader.GetInt32("idEvent"),
                                    IdOrganization = reader.GetInt32("idOrganization"),
                                    Name = reader.GetString("Name"),
                                    Date = reader.GetDateTime("Date").ToString(),
                                    Location = reader.GetString("Location"),
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

            return events;
        }
        public List<EventDetails> GetAllEventsByOrganization(Organizator org)
        {
            List<EventDetails> details = new List<EventDetails>();
            FighterService fighterService = FighterService.getInstance();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from EventDetails WHERE idOrganization=@Id;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", org.IdOrganizator);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                details.Add(new EventDetails
                                {
                                    idEvent = reader.GetInt32("idEvent"),
                                    idFightCard = reader.GetInt32("idFightCard"),
                                    idOrganization = reader.GetInt32("idOrganization"),
                                    Name = reader.GetString("Name"),
                                    Date = reader.GetDateTime("Date").ToString("dd-MM-yyyy"),
                                    Location = reader.GetString("Location"),
                                    idRedCorner = reader.GetInt32("idRedCorner"),
                                    idBlueCorner = reader.GetInt32("idBlueCorner"),
                                    RedCorner = fighterService.GetFighterByID(org, reader.GetInt32("idRedCorner")),
                                    BlueCorner = fighterService.GetFighterByID(org, reader.GetInt32("idBlueCorner")),
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
        public int? createEvent(Event e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Event (Name, Location, Date, idOrganization) 
                VALUES (@Name, @Location, @Date, @idOrganization);
                SELECT LAST_INSERT_ID();
                ";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", e.Name);
                        command.Parameters.AddWithValue("@Location", e.Location);
                        command.Parameters.AddWithValue("@Date", e.Date);
                        command.Parameters.AddWithValue("@idOrganization", e.IdOrganization);
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
                return null;
            }
        }

        public int? createCard(FightCard e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Fight_Card (idEvent) 
                VALUES (@idEvent);
                SELECT LAST_INSERT_ID();
                ";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idEvent", e.idEvent);
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
                return null;
            }
        }

        public bool addFight(Fight f)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO Fight (RoundEnded, idRedCorner, idBlueCorner, idWinner, idEvent, idFightCard, Method) 
                VALUES (@RoundEnded, @idRedCorner, @idBlueCorner, @idWinner, @idEvent, @idFightCard, @Method);
            ";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoundEnded", null);
                        command.Parameters.AddWithValue("@idRedCorner", f.redCornedId);
                        command.Parameters.AddWithValue("@idBlueCorner", f.blueCornedId);
                        command.Parameters.AddWithValue("@idWinner", null);
                        command.Parameters.AddWithValue("@idEvent", f.idEvent);
                        command.Parameters.AddWithValue("@idFightCard", f.idCard);
                        command.Parameters.AddWithValue("@Method", null);
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

        public List<(int idFight, FighterDetails redCorner, FighterDetails blueCorner)> GetFightersFromCard(Organizator org, EventDetails e)
        {
            List<(int, FighterDetails, FighterDetails)> fighters = new List<(int, FighterDetails, FighterDetails)>();
            FighterDetails redCornder = null;
            FighterDetails blueCorder = null;
            FighterService service = FighterService.getInstance();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT fight.* from Fight fight JOIN EventDetails e on fight.idFightCard = e.idFightCard where fight.idFightCard=@Id;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", e.idFightCard);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Fight f = new Fight
                                {
                                    blueCornedId = reader.GetInt32("idBlueCorner"),
                                    redCornedId = reader.GetInt32("idRedCorner"),
                                };

                                fighters.Add((reader.GetInt32("idFight"), service.GetFighterByID(org, f.redCornedId), service.GetFighterByID(org, f.blueCornedId)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
            }

            return fighters;
        }


    }
}