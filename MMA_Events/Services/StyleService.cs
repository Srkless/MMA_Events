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
    public class StyleService
    {
        private static StyleService instance = null;
        private readonly string connectionString;

        private StyleService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FightsDatabase"].ConnectionString;
        }

        public static StyleService GetInstance()
        {
            if (instance == null)
            {
                instance = new StyleService();
            }
            return instance;
        }

        public void UpdateUserStyleAndLanguage(User user)
        {
            // Proverite da li je korisnik validan (ako je potrebno)
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Veza sa bazom
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Upit za ažuriranje stila i jezika
                    string query = "UPDATE User SET `style` = @style, `language` = @language WHERE `IdUser` = @idUser";

                    using (var command = new MySqlCommand(query, conn))
                    {
                        // Dodavanje parametara u upit
                        command.Parameters.AddWithValue("@style", user.Style ?? "Style1.xaml");
                        command.Parameters.AddWithValue("@language", user.Language ?? "en");
                        command.Parameters.AddWithValue("@IdUser", user.IdUser);

                        // Izvršavanje upita
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Obrada grešaka
                    Console.WriteLine("Error while updating user style and language: " + ex.Message);
                }
            }
        }

        public void UpdateOrganizatorStyleAndLanguage(Organizator org)
        {
            // Proverite da li je korisnik validan (ako je potrebno)
            if (org == null)
                throw new ArgumentNullException(nameof(org));

            // Veza sa bazom
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Upit za ažuriranje stila i jezika
                    string query = "UPDATE Organization SET `style` = @style, `language` = @language WHERE `idOrganization` = @idOrg";

                    using (var command = new MySqlCommand(query, conn))
                    {
                        // Dodavanje parametara u upit
                        command.Parameters.AddWithValue("@style", org.Style ?? "Style1.xaml");
                        command.Parameters.AddWithValue("@language", org.Language ?? "en");
                        command.Parameters.AddWithValue("@idOrg", org.IdOrganizator);

                        // Izvršavanje upita
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Obrada grešaka
                    Console.WriteLine("Error while updating user style and language: " + ex.Message);
                }
            }
        }
    }
}
