using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MMA_Events.Model;
using MySql.Data.MySqlClient;

namespace MMA_Events.Services
{
    public class AuthService
    {
        private readonly string connectionString;
        static private AuthService instance = null;

        private AuthService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static AuthService getInstance()
        {
            if (instance == null)
            {
                instance = new AuthService();
            }
            return instance;

        }
        public bool registerUser(string name, string surname, string email, string country, string password)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertPersonQuery = @"
                        INSERT INTO Person (Name, Surname, Country)
                        VALUES (@Name, @Surname, @Country);";

                    using (MySqlCommand personCommand = new MySqlCommand(insertPersonQuery, connection))
                    {
                        personCommand.Parameters.AddWithValue("@Name", name);
                        personCommand.Parameters.AddWithValue("@Surname", surname);
                        personCommand.Parameters.AddWithValue("@Country", country);
                        personCommand.ExecuteNonQuery();
                    }

                    string getPersonIdQuery = "SELECT LAST_INSERT_ID();";
                    int personId;
                    using (MySqlCommand getIdCommand = new MySqlCommand(getPersonIdQuery, connection))
                    {
                        personId = Convert.ToInt32(getIdCommand.ExecuteScalar());
                    }

                    string insertUserQuery = @"
                        INSERT INTO User (Email, Password, idUser, style, language)
                        VALUES (@Email, @Password, @idUser, @style, @language);";

                    using (MySqlCommand userCommand = new MySqlCommand(insertUserQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue("@Email", email);
                        userCommand.Parameters.AddWithValue("@Password", password);
                        userCommand.Parameters.AddWithValue("@idUser", personId);
                        userCommand.Parameters.AddWithValue("@style", "Style1.xaml");
                        userCommand.Parameters.AddWithValue("@language", "en");
                        userCommand.ExecuteNonQuery();
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

        public User GetUserByEmail(string email)
        {
            User user = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT " +
                        "User.idUser, " +
                        "User.Email, " +
                        "User.Password," +
                        "Person.Name, " +
                        "Person.Surname, " +
                        "Person.Country, " +
                        "User.Style, " +
                        "User.Language " +
                        "FROM User INNER JOIN Person ON User.idUser = Person.idPerson WHERE User.Email = @Email;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    IdUser = reader.GetInt32("idUser"),
                                    Email = reader.GetString("Email"),
                                    Password = reader.GetString("Password"),
                                    Name = reader.GetString("Name"),
                                    Surname = reader.GetString("Surname"),
                                    Country = reader.GetString("Country"),
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

            return user;
        }

        public Organizator GetByName(string name)
        {
            Organizator organizator = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT *" +
                        "FROM Organization WHERE Organization.Name = @Name;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                organizator = new Organizator
                                {
                                    IdOrganizator = reader.GetInt32("idOrganization"),
                                    Password = reader.GetString("Password"),
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

            return organizator;
        }

        public Organizator GetOrganizatorByID(int id)
        {
            Organizator organizator = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT *" +
                        "FROM Organization WHERE Organization.idOrganization = @id;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                organizator = new Organizator
                                {
                                    IdOrganizator = reader.GetInt32("idOrganization"),
                                    Password = reader.GetString("Password"),
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

            return organizator;
        }
    }
}
