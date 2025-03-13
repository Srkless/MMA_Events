using MMA_Fights.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Fights.Services
{
    public class FightService
    {
        private readonly string connectionString;
        private static FightService instance = null;

        private FightService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static FightService getInstance()
        {
            if (instance == null)
            {
                instance = new FightService();
            }
            return instance;

        }

        public bool UpdateFightResult(int idFightCard, int idWinner, string method, int roundEnded, int idFight)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                UPDATE Fight fight 
                JOIN EventDetails e ON fight.idFightCard = e.idFightCard 
                SET fight.idWinner = @idWinner, 
                    fight.RoundEnded = @roundEnded, 
                    fight.Method = @method
                WHERE fight.idFightCard = @idFightCard 
                AND fight.idFight = @idFight;";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idWinner", idWinner);
                    cmd.Parameters.AddWithValue("@roundEnded", roundEnded);
                    cmd.Parameters.AddWithValue("@method", method);
                    cmd.Parameters.AddWithValue("@idFightCard", idFightCard);
                    cmd.Parameters.AddWithValue("@idFight", idFight);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Fight GetFightByID(int idFight)
        {
            Fight fight = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from Fight f where f.idFight=@idFight";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@idFight", idFight);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fight = new Fight
                                {
                                    id = reader.GetInt32("idFight"),
                                    roundEnded = reader.IsDBNull(reader.GetOrdinal("RoundEnded"))
                                        ? (int?)null
                                        : reader.GetInt32("RoundEnded"),
                                    redCornedId = reader.GetInt32("idRedCorner"),
                                    blueCornedId = reader.GetInt32("idBlueCorner"),
                                    winnerId =  reader.IsDBNull(reader.GetOrdinal("idWinner"))
                                        ? (int?)null
                                        : reader.GetInt32("idWinner"),
                                    idEvent = reader.GetInt32("idEvent"),
                                    idCard = reader.GetInt32("idFightCard"),
                                    Method = reader.IsDBNull(reader.GetOrdinal("Method"))
                                        ? (FightMethod?)null
                                        : (FightMethod)Enum.Parse(typeof(FightMethod), reader.GetString("Method"))
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

            return fight;
        }
    }
}
