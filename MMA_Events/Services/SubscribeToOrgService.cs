
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Services
{
    class SubscribeToOrgService
    {
        private static SubscribeToOrgService instance = null;
        private readonly string connectionString;

        private SubscribeToOrgService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static SubscribeToOrgService GetInstance()
        {
            if (instance == null)
            {
                instance = new SubscribeToOrgService();
            }
            return instance;
        }

        public bool IsSubscribed(int idUser, int idOrganization)
        {
            // SQL upit koji proverava da li postoji zapis sa datim idUser i idOrganization
            string query = "SELECT COUNT(*) FROM mydb.User_Organization WHERE idUser = @idUser AND idOrganization = @idOrganization";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                // Parametrizovanje upita da se izbegne SQL injection
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@idOrganization", idOrganization);

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar()); // Očekujemo broj kao rezultat
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greška pri povezivanju sa bazom: " + ex.Message);
                    return false;
                }
            }
        }

        public bool subscribe(int idUser, int idOrg)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertPersonQuery = @"
                        INSERT INTO user_organization (idUser, idOrganization)
                        VALUES (@idUser, @idOrg);";

                    using (MySqlCommand personCommand = new MySqlCommand(insertPersonQuery, connection))
                    {
                        personCommand.Parameters.AddWithValue("@idUser", idUser);
                        personCommand.Parameters.AddWithValue("@idOrg", idOrg);
                        personCommand.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška: " + ex.Message);
                return false;
            }
        }

        public bool unsubscribe(int idUser, int idOrg)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertPersonQuery = @"
                        DELETE FROM user_organization WHERE (idUser = @idUser) and (idOrganization = @idOrg);
";

                    using (MySqlCommand personCommand = new MySqlCommand(insertPersonQuery, connection))
                    {
                        personCommand.Parameters.AddWithValue("@idUser", idUser);
                        personCommand.Parameters.AddWithValue("@idOrg", idOrg);
                        personCommand.ExecuteNonQuery();
                    }

                    return true;
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
