using MMA_Events.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Services
{
    public class OrganizationService
    {
        private static OrganizationService instance = null;
        private readonly string connectionString;

        private OrganizationService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static OrganizationService GetInstance()
        {
            if (instance == null)
            {
                instance = new OrganizationService();
            }
            return instance;
        }

        public List<Organizator> GetAllOrganizators()
        {
            List<Organizator> organizators = new List<Organizator>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * from Organization;";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                organizators.Add(new Organizator
                                {
                                    IdOrganizator = reader.GetInt32("idOrganization"),
                                    Name = reader.GetString("Name"),
                                    Image = reader.GetString("Image"),
                                    Style = reader.GetString("Style"),
                                    Language = reader.GetString("Language"),
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

            return organizators;
        }


            public Organizator GetOrganizatorByID(int id)
            {
            Organizator org = null;

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = @"SELECT * from Organization WHERE idOrganization=@id;";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                org = new Organizator
                                    {
                                        IdOrganizator = reader.GetInt32("idOrganization"),
                                        Name = reader.GetString("Name"),
                                        Image = reader.GetString("Image"),
                                    Style = reader.GetString("Style"),
                                    Language = reader.GetString("Language"),
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

                return org;
            }
        }
}
