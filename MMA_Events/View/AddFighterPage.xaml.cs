using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MMA_Fights.Model;
using MMA_Fights.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMA_Fights.View
{
    /// <summary>
    /// Interaction logic for ShowFighters.xaml
    /// </summary>
    public partial class AddFighterPage : Page
    {

        Organizator org = null;
        //public AddFighterPage()
        //{
        //    InitializeComponent();
        //}

        public AddFighterPage(Organizator org)
        {
            InitializeComponent();
            this.org = org;
        }

        private void PickImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Kreiraj OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filtriranje da prikaže samo slike
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Ako korisnik odabere fajl
            if (openFileDialog.ShowDialog() == true)
            {
                // Kreiranje BitmapImage objekta
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));

                // Postavljanje slike na Image kontrolu
                SelectedImage.Source = bitmap;

                if (bitmap != null)
                {
                    bAddImage.Visibility = Visibility.Hidden;
                    RePickImageButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void RePickImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Ponovno otvaranje OpenFileDialog-a za odabir nove slike
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filtriranje da prikaže samo slike
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Ako korisnik odabere fajl
            if (openFileDialog.ShowDialog() == true)
            {
                // Kreiranje BitmapImage objekta sa novom slikom
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));

                // Postavljanje nove slike na Image kontrolu
                SelectedImage.Source = bitmap;
            }
        }

        private void OnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Dozvoljava samo brojeve
            e.Handled = !int.TryParse(e.Text, out _);
            

        }
        private void OnlyText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Blokira unos nedozvoljenih znakova
            }

        }
        private void OnlyNumbersDotAndMinus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9.\-]+$");

            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Blokira unos nedozvoljenih znakova
            }
        }


        private void AllowControlKeys(object sender, KeyEventArgs e)
        {
            // Dozvoljava Backspace, Delete, strelice, Tab
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
        }
        private void getCategoryName(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(fieldFighterWeight.Text))
                tbfighterWeight.Text = GetWeightCategory(float.Parse(fieldFighterWeight.Text));
            else
                tbfighterWeight.Text = "";
        }

        private void AddFighter_Click(object sender, RoutedEventArgs e)
        {
            string Nickname = fieldUserNickname.Text;
            string[] Fullname = fieldUserFullname.Text.Split(" ");
            float Weight = float.Parse(fieldFighterWeight.Text);
            string Country = fieldFighterCountry.Text;
            DateTime BirthDate_DateTime = fieldFighterBirthDate.DisplayDate;
            string BirthDate = BirthDate_DateTime.ToString("yyyy-MM-dd");
            string[] score = fieldFighterScore.Text.Split("-");
            int KOs = int.Parse(fieldFighterKOs.Text);
            int Submissions = int.Parse(fieldFighterSubmissions.Text);
            string imagePath = "";

            if (SelectedImage.Source is BitmapImage bitmapImage && bitmapImage.UriSource != null)
            {
                imagePath = bitmapImage.UriSource.LocalPath; // Dobijanje putanje do slike
            }
            int idOrg = org.IdOrganizator;

            Fighter fighter = new Fighter
            {
                Name = Fullname[0],
                Surname = Fullname[1],
                Country = Country,
                Nickname = Nickname,
                BirthDate = BirthDate,
                IsReady = true,
                FightWeight = Weight,
                Image = imagePath,
                IdOrganization = idOrg
            };

            FighterStats stats = new FighterStats
            {
                Wins = int.Parse(score[0]),
                Losses = int.Parse(score[0]),
                Draws = int.Parse(score[0]),
                KOs = KOs,
                Submissions = Submissions
            };

            FighterService _fService = FighterService.getInstance();

            if (_fService.addFighter(fighter, stats))
            {

            }


        }

        public static string GetWeightCategory(float fighterWeight)
        {
            if (fighterWeight >= 0 && fighterWeight <= 52.2)
                return "Strawweight";
            else if (fighterWeight > 52.2 && fighterWeight <= 56.7)
                return "Flyweight";
            else if (fighterWeight > 56.7 && fighterWeight <= 61.2)
                return "Bantamweight";
            else if (fighterWeight > 61.2 && fighterWeight <= 65.8)
                return "Featherweight";
            else if (fighterWeight > 65.8 && fighterWeight <= 70.3)
                return "Lightweight";
            else if (fighterWeight > 70.3 && fighterWeight <= 77.1)
                return "Welterweight";
            else if (fighterWeight > 77.1 && fighterWeight <= 83.9)
                return "Middleweight";
            else if (fighterWeight > 83.9 && fighterWeight <= 93.0)
                return "Light Heavyweight";
            else if (fighterWeight > 93.0 && fighterWeight <= 120.2)
                return "Heavyweight";
            else
                return "Super Heavyweight";
        }

    }
}
